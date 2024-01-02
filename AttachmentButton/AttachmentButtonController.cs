using System;
using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vmail_attachments;

public class AttachmentButtonController : MonoBehaviour
{
    public VMailApp App;
    
    public Button Button { get; private set; }
    public TextMeshProUGUI Label { get; private set; }

    private AttachmentDatabaseEntry _currentDatabaseEntry;
    
    private void Awake()
    {
        NukeAutoTextController();
        SetupLabel();
        SetupButton();
    }

    private void OnEnable()
    {
        AddButtonListeners();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    private void NukeAutoTextController()
    {
        ComputerAutoTextController autoTextController = gameObject.GetComponentInChildren<ComputerAutoTextController>();

        if (autoTextController != null)
        {
            DestroyImmediate(autoTextController);
        }
        else
        {
            Utilities.Log("AttachmentButtonController.NukeAutoTextController: Expected autoTextController but it was null.", LogLevel.Debug);
        }
    }

    private void SetupLabel()
    {
        Label = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (Label != null)
        {
            Label.text = "Attached";
            Label.overflowMode = TextOverflowModes.Overflow;
            Label.enableWordWrapping = false;
        }
        else
        {
            Utilities.Log("AttachmentButtonController.SetupLabel: Expected textMeshPro but it was null.", LogLevel.Debug);
        }
    }

    private void SetupButton()
    {
        Button = gameObject.GetComponentInChildren<Button>();

        if (Button == null)
        {
            Utilities.Log("AttachmentButtonController.SetupButton: Expected attachmentButton but it was null.", LogLevel.Debug);
        }
    }

    private void RemoveButtonListeners()
    {
        if (Button != null)
        {
            Button.onClick = new Button.ButtonClickedEvent();
        }
    }

    private void AddButtonListeners()
    {
        RemoveButtonListeners();

        if (Button != null)
        {
            Action action = OnAttachmentButtonClicked;
            Button.onClick.AddListener(action);
        }
    }
    
    private void OnAttachmentButtonClicked()
    {
        PrintCurrentDatabaseEntry(App?.controller);
    }

    private void PrintCurrentDatabaseEntry(ComputerController currentCruncher)
    {
        if (currentCruncher == null)
        {
            Utilities.Log("AttachmentButtonController.PrintCurrentDatabaseEntry: Tried to print an attachment, but not at a computer!", LogLevel.Debug);
            return;
        }
        
        if (currentCruncher.printedDocument == null && currentCruncher.printTimer <= 0f)
        {
            currentCruncher.printTimer = 1f;
            currentCruncher.printerParent.localPosition = new Vector3(currentCruncher.printerParent.localPosition.x, currentCruncher.printerParent.localPosition.y, -0.05f);
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerPrint, Player.Instance, currentCruncher.ic.interactable.node, currentCruncher.ic.interactable.wPos);
            currentCruncher.printedDocument = _currentDatabaseEntry.CreateAttachmentDocument(currentCruncher.printerParent.position, currentCruncher.ic.transform.eulerAngles);
            
            Action onRemoved = App.OnPlayerTakePrint;
            currentCruncher.printedDocument.OnRemovedFromWorld += onRemoved;
            
            Utilities.Log("AttachmentButtonController.PrintCurrentDatabaseEntry: Successfully printed an attachment!", LogLevel.Debug);
        }
        else
        {
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerInvalidPasscode, Player.Instance, currentCruncher.ic.interactable.node, currentCruncher.ic.interactable.wPos);
            Utilities.Log("AttachmentButtonController.PrintCurrentDatabaseEntry: Tried to print an attachment, but the print cooldown isn't done!", LogLevel.Debug);
        }
    }

    public void RefreshMessage(StateSaveData.MessageThreadSave message)
    {
        _currentDatabaseEntry = null;
        
        if (message == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        if (Plugin.Database.TryGetAttachment(message.threadID, out AttachmentDatabaseEntry entry))
        {
            gameObject.SetActive(true);
            _currentDatabaseEntry = entry;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
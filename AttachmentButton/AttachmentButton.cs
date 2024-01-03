using System;
using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VmailAttachments;

public class AttachmentButton : MonoBehaviour
{
    private VMailApp _app;
    private AttachmentButtonHandler _handler;
    private Button _button;
    private TextMeshProUGUI _label;

    // Harmony won't let us pass AttachmentDatabaseEntry, so we'll pass these individually.
    private bool _currentAttachmentExists = false;
    private string _currentAttachmentInteractablePresetName = string.Empty;
    private int _currentAttachmentWriterID = -1;
    private int _currentAttachmentReceiverID = -1;

    
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

    public void Initialize(VMailApp app, Component handlerComponent)
    {
        _app = app;

        if (handlerComponent is AttachmentButtonHandler handler)
        {
            _handler = handler;
        }

        SetCurrentAttachment(false);
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
            Utilities.Log("AttachmentButton.NukeAutoTextController: Expected autoTextController but it was null.", LogLevel.Debug);
        }
    }

    private void SetupLabel()
    {
        _label = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (_label != null)
        {
            _label.text = "Attached";
            _label.overflowMode = TextOverflowModes.Overflow;
            _label.enableWordWrapping = false;
        }
        else
        {
            Utilities.Log("AttachmentButton.SetupLabel: Expected textMeshPro but it was null.", LogLevel.Debug);
        }
    }

    private void SetupButton()
    {
        _button = gameObject.GetComponentInChildren<Button>();

        if (_button == null)
        {
            Utilities.Log("AttachmentButton.SetupButton: Expected attachmentButton but it was null.", LogLevel.Debug);
        }
    }

    private void RemoveButtonListeners()
    {
        if (_button != null)
        {
            // RemoveAllListeners is not sufficient because there's a serialized "persistent" listener in there.
            _button.onClick = new Button.ButtonClickedEvent();
        }
    }

    private void AddButtonListeners()
    {
        RemoveButtonListeners();

        if (_button != null)
        {
            Action action = OnAttachmentButtonClicked;
            _button.onClick.AddListener(action);
        }
    }
    
    private void OnAttachmentButtonClicked()
    {
        PrintCurrentDatabaseEntry(_app?.controller);
    }

    private void PrintCurrentDatabaseEntry(ComputerController currentCruncher)
    {
        if (!_currentAttachmentExists)
        {
            return;
        }
        
        if (currentCruncher == null)
        {
            Utilities.Log("AttachmentButton.PrintCurrentDatabaseEntry: Tried to print an attachment, but not at a computer!", LogLevel.Debug);
            return;
        }
        
        if (currentCruncher.printedDocument == null && currentCruncher.printTimer <= 0f)
        {
            currentCruncher.printTimer = 1f;
            currentCruncher.printerParent.localPosition = new Vector3(currentCruncher.printerParent.localPosition.x, currentCruncher.printerParent.localPosition.y, -0.05f);
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerPrint, Player.Instance, currentCruncher.ic.interactable.node, currentCruncher.ic.interactable.wPos);
            currentCruncher.printedDocument = CreateAttachmentDocument(currentCruncher.printerParent.position, currentCruncher.ic.transform.eulerAngles);
            
            Action onRemoved = _app.OnPlayerTakePrint;
            currentCruncher.printedDocument.OnRemovedFromWorld += onRemoved;
            
            Utilities.Log("AttachmentButton.PrintCurrentDatabaseEntry: Successfully printed an attachment!", LogLevel.Debug);
        }
        else
        {
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerInvalidPasscode, Player.Instance, currentCruncher.ic.interactable.node, currentCruncher.ic.interactable.wPos);
            Utilities.Log("AttachmentButton.PrintCurrentDatabaseEntry: Tried to print an attachment, but the print cooldown isn't done!", LogLevel.Debug);
        }
    }

    public void SetCurrentAttachment(bool exists, string interactablePresetName = "", int writerID = -1, int receiverID = -1)
    {
        _currentAttachmentExists = exists;
        _currentAttachmentInteractablePresetName = interactablePresetName;
        _currentAttachmentWriterID = writerID;
        _currentAttachmentReceiverID = receiverID;
    }
    
    private InteractablePreset GetInteractablePreset()
    {
        return Toolbox.Instance.GetInteractablePreset(_currentAttachmentInteractablePresetName);
    }

    private Human GetWriterHuman()
    {
        return CityData.Instance.citizenDirectory.Find((Il2CppSystem.Predicate<Citizen>)FindWriterPredicate);
    }

    private Human GetReceiverHuman()
    {
        return CityData.Instance.citizenDirectory.Find((Il2CppSystem.Predicate<Citizen>)FindReceiverPredicate);
    }

    private bool FindWriterPredicate(Citizen item)
    {
        return item.humanID == _currentAttachmentWriterID;
    }
    
    private bool FindReceiverPredicate(Citizen item)
    {
        return item.humanID == _currentAttachmentReceiverID;
    }

    public Interactable CreateAttachmentDocument(Vector3 worldPosition, Vector3 worldRotation)
    {
        Interactable document = InteractableCreator.Instance.CreateWorldInteractable(GetInteractablePreset(),
                                                                                     Player.Instance,
                                                                                     GetWriterHuman(),
                                                                                     GetReceiverHuman(),
                                                                                     worldPosition,
                                                                                     worldRotation,
                                                                                     null,
                                                                                     null);
        
        if (document != null)
        {
            document.MarkAsTrash(val: true);
        }

        return document;
    }
}
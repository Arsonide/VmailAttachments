using System;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace VmailAttachments;

/// <summary>
/// This component is always active, even if the button is not. This allows us to manage button visibility without doing a lot of GetComponent calls.
/// </summary>
public class AttachmentButtonHandler : MonoBehaviour
{
    public static Action<VMailApp, StateSaveData.MessageThreadSave> OnVmailSelected;

    private VMailApp _app;
    private AttachmentButton _button;
    
    private void OnEnable()
    {
        RefreshCurrentAppState();
        OnVmailSelected -= OnVmailSelection;
        OnVmailSelected += OnVmailSelection;
    }

    private void OnDisable()
    {
        OnVmailSelected -= OnVmailSelection;
    }

    public void Initialize(VMailApp app)
    {
        _app = app;
        _button = FindOrCreateButton();
        _button.Initialize(app, this);
        RefreshCurrentAppState();
    }

    private void RefreshCurrentAppState()
    {
        if (_app != null)
        {
            OnVmailSelection(_app, _app?.selectedThread);
        }
    }

    private void OnVmailSelection(VMailApp app, StateSaveData.MessageThreadSave vmail)
    {
        if (app == _app)
        {
            CheckButtonVisibility(vmail);
        }
    }

    private void CheckButtonVisibility(StateSaveData.MessageThreadSave vmail)
    {
        if (vmail == null)
        {
            _button.SetCurrentAttachment(false);
            _button.gameObject.SetActive(false);
            return;
        }
        
        if (VmailAttachmentsPlugin.Database.TryGetAttachment(vmail.threadID, out AttachmentDatabaseEntry entry))
        {
            _button.SetCurrentAttachment(true, entry.InteractablePresetName, entry.WriterID, entry.ReceiverID);
            _button.gameObject.SetActive(true);
        }
        else
        {
            _button.SetCurrentAttachment(false);
            _button.gameObject.SetActive(false);
        }
    }
    
    private AttachmentButton FindOrCreateButton()
    {
        AttachmentButton attachmentButton = gameObject.GetComponentInChildren<AttachmentButton>();

        if (attachmentButton != null)
        {
            return attachmentButton;
        }

        Button printButton = null;

        foreach (Button button in gameObject.GetComponentsInChildren<Button>())
        {
            if (button.gameObject.name != "Print")
            {
                continue;
            }

            printButton = button;
            break;
        }
        
        if (printButton == null)
        {
            Utilities.Log("AttachmentButtonHandler.FindOrCreateButton: Needed to create attachment button but print button doesn't exist!", LogLevel.Debug);
            return null;
        }

        GameObject printGameObject = printButton.gameObject;
        GameObject attachmentGameObject = Instantiate(printGameObject, printGameObject.transform.parent);
        
        // Move button up a bit.
        Vector3 attachmentOriginalPosition = attachmentGameObject.transform.localPosition;
        attachmentGameObject.transform.localPosition = new Vector3(attachmentOriginalPosition.x, attachmentOriginalPosition.y + 0.06f, attachmentOriginalPosition.z);

        attachmentButton = attachmentGameObject.AddComponent<AttachmentButton>();
        return attachmentButton;
    }
}
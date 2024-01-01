using System;
using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace vmail_attachments;

public class AttachmentButtonController : MonoBehaviour
{
    public Button Button { get; private set; }
    public TextMeshProUGUI Label { get; private set; }

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
            Utilities.Log("AttachmentButtonController: Expected autoTextController but it was null.", LogLevel.Debug);
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
            Utilities.Log("AttachmentButtonController: Expected textMeshPro but it was null.", LogLevel.Debug);
        }
    }

    private void SetupButton()
    {
        Button = gameObject.GetComponentInChildren<Button>();

        if (Button == null)
        {
            Utilities.Log("AttachmentButtonController: Expected attachmentButton but it was null.", LogLevel.Debug);
        }
    }

    private void RemoveButtonListeners()
    {
        if (Button != null)
        {
            Button.onClick.RemoveAllListeners();
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
        Utilities.Log("AttachmentButtonController: The attachment button was clicked!", LogLevel.Debug);
    }
}
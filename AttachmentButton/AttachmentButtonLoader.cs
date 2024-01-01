using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vmail_attachments;

[HarmonyPatch]
public class AttachmentButtonLoader
{
    [HarmonyPatch(typeof(VMailApp), "OnSetup")]
    private static void Postfix(VMailApp __instance)
    {
        // TODO We only want to do this if the email we are viewing has an attachment.
        AttachmentButtonController attachmentButton = FindOrCreateAttachmentButton(__instance.gameObject);
    }

    private static AttachmentButtonController FindOrCreateAttachmentButton(GameObject parent)
    {
        AttachmentButtonController attachmentButton = parent.GetComponentInChildren<AttachmentButtonController>();

        if (attachmentButton != null)
        {
            return attachmentButton;
        }

        Button printButton = null;

        foreach (Button button in parent.GetComponentsInChildren<Button>())
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
            Utilities.Log("AttachmentButtonLoader: Needed to create attachment button but print button doesn't exist!", LogLevel.Debug);
            return null;
        }

        GameObject printGameObject = printButton.gameObject;
        GameObject attachmentGameObject = Object.Instantiate(printGameObject, printGameObject.transform.parent);
        
        // Move button up a bit.
        Vector3 attachmentOriginalPosition = attachmentGameObject.transform.localPosition;
        attachmentGameObject.transform.localPosition = new Vector3(attachmentOriginalPosition.x, attachmentOriginalPosition.y + 0.06f, attachmentOriginalPosition.z);

        attachmentButton = attachmentGameObject.AddComponent<AttachmentButtonController>();
        return attachmentButton;
    }
}
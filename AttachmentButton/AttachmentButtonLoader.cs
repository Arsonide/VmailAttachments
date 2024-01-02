using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace vmail_attachments;

[HarmonyPatch]
public class AttachmentButtonLoader
{
    [HarmonyPatch(typeof(VMailApp), "OnSetup")]
    private static void Postfix(VMailApp __instance)
    {
        AttachmentButtonController attachmentButton = FindOrCreateAttachmentButton(__instance.gameObject);
        
        if (attachmentButton != null)
        {
            attachmentButton.App = __instance;
            attachmentButton.RefreshMessage(__instance?.selectedThread);
        }
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
            Utilities.Log("AttachmentButtonLoader.FindOrCreateAttachmentButton: Needed to create attachment button but print button doesn't exist!", LogLevel.Debug);
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
using HarmonyLib;

namespace vmail_attachments;

[HarmonyPatch]
public class AttachmentButtonEnabler
{
    [HarmonyPatch(typeof(VMailApp), "SetSelectedVmail")]
    private static void Postfix(VMailApp __instance)
    {
        // TODO Find a better way to do this that's more performant.
        AttachmentButtonController attachmentButton = __instance.GetComponentInChildren<AttachmentButtonController>(true);
        
        if (attachmentButton != null)
        {
            attachmentButton.RefreshMessage(__instance?.selectedThread);
        }
    }
}
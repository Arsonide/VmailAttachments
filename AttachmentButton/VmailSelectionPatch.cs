using HarmonyLib;

namespace vmail_attachments;

[HarmonyPatch]
public class VmailSelectionPatch
{
    [HarmonyPatch(typeof(VMailApp), "SetSelectedVmail")]
    private static void Postfix(VMailApp __instance)
    {
        AttachmentButtonHandler.OnVmailSelected?.Invoke(__instance, __instance?.selectedThread);
    }
}
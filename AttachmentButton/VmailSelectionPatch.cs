using BepInEx.Logging;
using HarmonyLib;

namespace VmailAttachments;

[HarmonyPatch]
public class VmailSelectionPatch
{
    [HarmonyPatch(typeof(VMailApp), "SetSelectedVmail")]
    private static void Postfix(VMailApp __instance)
    {
        Utilities.Log("VmailSelectionPatch.Postfix: Triggered!", LogLevel.Debug);
        AttachmentButtonHandler.OnVmailSelected?.Invoke(__instance, __instance?.selectedThread);
    }
}
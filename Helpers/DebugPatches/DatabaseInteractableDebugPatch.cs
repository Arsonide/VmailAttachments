#if false

using BepInEx.Logging;
using HarmonyLib;

namespace VmailAttachments;

[HarmonyPatch(typeof(DatabaseApp), "OnPrintEntry")]
public class DatabaseInteractableDebugPatch
{
    [HarmonyPostfix]
    static void Postfix(DatabaseApp __instance)
    {
        Utilities.Log($"DatabaseInteractableDebugPatch.Postfix: Interactable Name: {__instance.ddsPrintout.presetName}!", LogLevel.Debug);
    }
}

#endif
#if false

using BepInEx.Logging;
using HarmonyLib;

namespace vmail_attachments;

[HarmonyPatch(typeof(DatabaseApp), "OnPrintEntry")]
public class PrintDatabaseInteractable
{
    [HarmonyPostfix]
    static void Postfix(DatabaseApp __instance)
    {
        Utilities.Log($"PrintDatabaseInteractable.Postfix: Interactable Name: {__instance.ddsPrintout.presetName}!", LogLevel.Debug);
    }
}

#endif
#if false

using BepInEx.Logging;
using HarmonyLib;

namespace VmailAttachments;

[HarmonyPatch(typeof(VMailApp), "SetSelectedVmail")]
public class VmailIDDebugPatch
{
    [HarmonyPostfix]
    static void Postfix(VMailApp __instance, ComputerOSMultiSelectElement newSelection)
    {
        StateSaveData.MessageThreadSave selectedThread = __instance?.selectedThread;

        if (selectedThread != null)
        {
            Utilities.Log($"VmailIDDebugPatch.Postfix: User selected Vmail #{selectedThread.threadID}! Participant A is {selectedThread.participantA}!", LogLevel.Debug);
        }
    }
}

#endif
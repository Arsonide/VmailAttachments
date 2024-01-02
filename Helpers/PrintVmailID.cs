#if false

using BepInEx.Logging;
using HarmonyLib;

namespace vmail_attachments;

[HarmonyPatch(typeof(VMailApp), "SetSelectedVmail")]
public class PrintVmailID
{
    [HarmonyPostfix]
    static void Postfix(VMailApp __instance, ComputerOSMultiSelectElement newSelection)
    {
        StateSaveData.MessageThreadSave selectedThread = __instance?.selectedThread;

        if (selectedThread != null)
        {
            Utilities.Log($"PrintVmailID.Postfix: User selected Vmail #{selectedThread.threadID}! Participant A is {selectedThread.participantA}!", LogLevel.Debug);
        }
    }
}

#endif
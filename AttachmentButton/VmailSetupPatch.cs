using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace VmailAttachments;

[HarmonyPatch]
public class VmailSetupPatch
{
    [HarmonyPatch(typeof(VMailApp), "OnSetup")]
    private static void Postfix(VMailApp __instance)
    {
        Utilities.Log("VmailSetupPatch.Postfix: Triggered!", LogLevel.Debug);
        GameObject go = __instance.gameObject;
        AttachmentButtonHandler handler = go.GetComponent<AttachmentButtonHandler>() ?? go.AddComponent<AttachmentButtonHandler>();
        handler.Initialize(__instance);
    }
}
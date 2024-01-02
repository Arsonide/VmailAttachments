using HarmonyLib;
using UnityEngine;

namespace vmail_attachments;

[HarmonyPatch]
public class VmailSetupPatch
{
    [HarmonyPatch(typeof(VMailApp), "OnSetup")]
    private static void Postfix(VMailApp __instance)
    {
        GameObject go = __instance.gameObject;
        AttachmentButtonHandler handler = go.GetComponent<AttachmentButtonHandler>() ?? go.AddComponent<AttachmentButtonHandler>();
        handler.Initialize(__instance);
    }
}
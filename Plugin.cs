using BepInEx;
using HarmonyLib;
using SOD.Common.BepInEx;

namespace vmail_attachments;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : PluginController<Plugin>
{
    public override void Load()
    {
        Utilities.Log("VMail Attachments Plugin Loaded!");
        Harmony harmony = new Harmony($"{MyPluginInfo.PLUGIN_GUID}");
        harmony.PatchAll();
    }
}

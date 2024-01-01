using BepInEx;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SOD.Common.BepInEx;

namespace vmail_attachments;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : PluginController<Plugin>
{
    public override void Load()
    {
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        Harmony harmony = new Harmony($"{MyPluginInfo.PLUGIN_GUID}");
        harmony.PatchAll();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
        
        ClassInjector.RegisterTypeInIl2Cpp<AttachmentButtonController>();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} has added custom types!");
    }
}

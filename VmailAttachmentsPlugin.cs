using System;
using BepInEx;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SOD.Common;
using SOD.Common.BepInEx;
using SOD.Common.Helpers;

namespace VmailAttachments;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class VmailAttachmentsPlugin : PluginController<VmailAttachmentsPlugin>
{
    public static AttachmentDatabase Database = new AttachmentDatabase();
    
    public override void Load()
    {
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        Harmony harmony = new Harmony($"{MyPluginInfo.PLUGIN_GUID}");
        harmony.PatchAll();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
        
        ClassInjector.RegisterTypeInIl2Cpp<AttachmentButton>();
        ClassInjector.RegisterTypeInIl2Cpp<AttachmentButtonHandler>();

        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} has added custom types!");

        Lib.SaveGame.OnAfterSave += OnAfterSave;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;
        Lib.SaveGame.OnAfterNewGame += OnAfterNewGame;
    }

    private void OnAfterSave(object sender, SaveGameArgs args)
    {
        Database.OnSave(args.FilePath);
    }

    private void OnAfterLoad(object sender, SaveGameArgs args)
    {
        Database.OnLoad(args.FilePath);
    }
    
    private void OnAfterNewGame(object sender, EventArgs args)
    {
        Database.OnNewGame();
    }
}

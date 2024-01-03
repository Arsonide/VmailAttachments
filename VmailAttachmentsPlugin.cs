using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SOD.Common;
using SOD.Common.BepInEx;
using SOD.Common.Helpers;

namespace VmailAttachments;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class VmailAttachmentsPlugin : PluginController<VmailAttachmentsPlugin>
{
    private static AttachmentDatabase _db = new AttachmentDatabase();
    
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
        if (_db == null)
        {
            Utilities.Log("VmailAttachmentsPlugin.OnAfterSave: Cannot save a null map.", LogLevel.Error);
            return;
        }

        string pluginPath = GetPluginSavePath(args.FilePath);
        
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            string json = JsonSerializer.Serialize(_db, options);
            File.WriteAllText(pluginPath, json);
            Utilities.Log($"VmailAttachmentsPlugin.OnAfterSave: Successfully saved attachment database to {pluginPath}! {_db.Database.Count}");
        }
        catch (Exception e)
        {
            Utilities.Log($"VmailAttachmentsPlugin.OnAfterSave: {e.Message}", LogLevel.Error);
        }
    }

    private void OnAfterLoad(object sender, SaveGameArgs args)
    {
        string pluginPath = GetPluginSavePath(args.FilePath);

        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            string json = File.ReadAllText(pluginPath);
            _db = JsonSerializer.Deserialize<AttachmentDatabase>(json, options);
            Utilities.Log($"VmailAttachmentsPlugin.OnAfterLoad: Successfully loaded attachment database from {pluginPath}!");
        }
        catch (Exception e)
        {
            _db = new AttachmentDatabase();
            Utilities.Log($"VmailAttachmentsPlugin.OnAfterLoad: {e.Message}", LogLevel.Error);
        }
    }
    
    private void OnAfterNewGame(object sender, EventArgs args)
    {
        _db = new AttachmentDatabase();
        Utilities.Log($"VmailAttachmentsPlugin.OnAfterNewGame: Initializing new attachment database!");
    }
    
    private static string GetPluginSavePath(string savePath)
    {
        string uniqueString = Lib.SaveGame.GetUniqueString(savePath);
        string fileName = $"VmailAttachments_{uniqueString}.json";
        return Lib.SaveGame.GetSavestoreDirectoryPath(Assembly.GetExecutingAssembly(), fileName);
    }

    public static void AddAttachment(int vmailID, string preset, int writer, int receiver)
    {
        AttachmentDatabaseEntry newEntry = new AttachmentDatabaseEntry()
        {
            InteractablePresetName = preset, WriterID = writer, ReceiverID = receiver
        };
        
        _db.Database[vmailID] = newEntry;
        Utilities.Log($"VmailAttachmentsPlugin.AddAttachment: Added {newEntry.ToString()} to vmail {vmailID}!", LogLevel.Debug);
    }
    
    public static void AddAttachment(StateSaveData.MessageThreadSave vmail, InteractablePreset preset, Human writer, Human receiver)
    {
        AddAttachment(vmail.threadID, preset.presetName, writer.humanID, receiver.humanID);
    }

    public static bool TryGetAttachment(int vmailID, out AttachmentDatabaseEntry attachment)
    {
        bool result = _db.Database.TryGetValue(vmailID, out attachment);

        if (attachment != null)
        {
            Utilities.Log($"VmailAttachmentsPlugin.TryGetAttachment: Vmail {vmailID}...found {attachment.ToString()}!", LogLevel.Debug);
        }
        else
        {
            Utilities.Log($"VmailAttachmentsPlugin.TryGetAttachment: Vmail {vmailID}...found NULL!", LogLevel.Debug);
        }
        
        return result;
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using BepInEx.Logging;
using SOD.Common;

namespace VmailAttachments;

public class AttachmentDatabase
{
    private AttachmentDatabaseMap _map = new AttachmentDatabaseMap();

    private string GetPluginSavePath(string savePath)
    {
        string uniqueString = Lib.SaveGame.GetUniqueString(savePath);
        string fileName = $"VmailAttachments_{uniqueString}.json";
        return Lib.SaveGame.GetSavestoreDirectoryPath(Assembly.GetExecutingAssembly(), fileName);
    }

    public void OnSave(string savePath)
    {
        if (_map == null)
        {
            Utilities.Log("AttachmentDatabase.OnSave: Cannot save a null map.", LogLevel.Error);
            return;
        }

        string pluginPath = GetPluginSavePath(savePath);
        
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            string json = JsonSerializer.Serialize(_map, options);
            File.WriteAllText(pluginPath, json);
            Utilities.Log($"AttachmentDatabase.OnSave: Successfully saved attachment database to {pluginPath}! {_map.Map.Count}");
        }
        catch (Exception e)
        {
            Utilities.Log($"AttachmentDatabase.OnSave: {e.Message}", LogLevel.Error);
        }
    }

    public void OnLoad(string savePath)
    {
        string pluginPath = GetPluginSavePath(savePath);

        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            string json = File.ReadAllText(pluginPath);
            _map = JsonSerializer.Deserialize<AttachmentDatabaseMap>(json, options);
            Utilities.Log($"AttachmentDatabase.OnLoad: Successfully loaded attachment database from {pluginPath}!");
        }
        catch (Exception e)
        {
            _map = new AttachmentDatabaseMap();
            Utilities.Log($"AttachmentDatabase.OnLoad: {e.Message}", LogLevel.Error);
        }
    }

    public void OnNewGame()
    {
        _map = new AttachmentDatabaseMap();
        Utilities.Log($"AttachmentDatabase.OnNewGame: Initializing new attachment database!");
    }

    public void AddAttachment(int vmailID, string preset, int writer, int receiver)
    {
        AttachmentDatabaseEntry newEntry = new AttachmentDatabaseEntry()
        {
            InteractablePresetName = preset, WriterID = writer, ReceiverID = receiver
        };
        
        _map.Map[vmailID] = newEntry;
        Utilities.Log($"AttachmentDatabase.AddAttachment: Added {newEntry.ToString()} to vmail {vmailID}!", LogLevel.Debug);
    }
    
    public void AddAttachment(StateSaveData.MessageThreadSave vmail, InteractablePreset preset, Human writer, Human receiver)
    {
        AddAttachment(vmail.threadID, preset.presetName, writer.humanID, receiver.humanID);
    }

    public bool TryGetAttachment(int vmailID, out AttachmentDatabaseEntry attachment)
    {
        bool result = _map.Map.TryGetValue(vmailID, out attachment);

        if (attachment != null)
        {
            Utilities.Log($"AttachmentDatabase.TryGetAttachment: Vmail {vmailID}...found {attachment.ToString()}!", LogLevel.Debug);
        }
        else
        {
            Utilities.Log($"AttachmentDatabase.TryGetAttachment: Vmail {vmailID}...found NULL!", LogLevel.Debug);
        }
        
        return result;
    }
}
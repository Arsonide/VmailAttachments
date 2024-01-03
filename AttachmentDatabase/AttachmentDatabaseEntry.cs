using System;

namespace VmailAttachments;

[Serializable]
public class AttachmentDatabaseEntry
{
    public string InteractablePresetName;
    public int WriterID;
    public int ReceiverID;

    public override string ToString()
    {
        return $"AttachmentDatabaseEntry: {InteractablePresetName}, {WriterID}, {ReceiverID}";
    }
}
using System;
using System.Collections.Generic;

namespace VmailAttachments;

[Serializable]
public class AttachmentDatabaseMap
{
    public Dictionary<int, AttachmentDatabaseEntry> Map = new Dictionary<int, AttachmentDatabaseEntry>();
}
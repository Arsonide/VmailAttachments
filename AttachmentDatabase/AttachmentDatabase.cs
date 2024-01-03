using System;
using System.Collections.Generic;

namespace VmailAttachments;

[Serializable]
public class AttachmentDatabase
{
    public Dictionary<int, AttachmentDatabaseEntry> Database = new Dictionary<int, AttachmentDatabaseEntry>();
}
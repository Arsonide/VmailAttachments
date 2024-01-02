using System;
using System.Collections.Generic;

namespace vmail_attachments;

[Serializable]
public class AttachmentDatabaseMap
{
    public Dictionary<int, AttachmentDatabaseEntry> Map = new Dictionary<int, AttachmentDatabaseEntry>();
}
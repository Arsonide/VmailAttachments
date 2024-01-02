using System;
using UnityEngine;

namespace vmail_attachments;

[Serializable]
public class AttachmentDatabaseEntry
{
    public string InteractablePresetName;
    public int WriterID;
    public int ReceiverID;

    private InteractablePreset GetInteractablePreset()
    {
        return Toolbox.Instance.GetInteractablePreset(InteractablePresetName);
    }

    private Human GetWriterHuman()
    {
        return CityData.Instance.citizenDirectory.Find((Il2CppSystem.Predicate<Citizen>)FindWriterPredicate);
    }

    private Human GetReceiverHuman()
    {
        return CityData.Instance.citizenDirectory.Find((Il2CppSystem.Predicate<Citizen>)FindReceiverPredicate);
    }

    private bool FindWriterPredicate(Citizen item)
    {
        return item.humanID == WriterID;
    }
    
    private bool FindReceiverPredicate(Citizen item)
    {
        return item.humanID == ReceiverID;
    }

    public Interactable CreateAttachmentDocument(Vector3 worldPosition, Vector3 worldRotation)
    {
        Interactable document = InteractableCreator.Instance.CreateWorldInteractable(GetInteractablePreset(),
                                                                                     Player.Instance,
                                                                                     GetWriterHuman(),
                                                                                     GetReceiverHuman(),
                                                                                     worldPosition,
                                                                                     worldRotation,
                                                                                     null,
                                                                                     null);
        
        if (document != null)
        {
            document.MarkAsTrash(val: true);
        }

        return document;
    }

    public override string ToString()
    {
        return $"AttachmentDatabaseEntry: {InteractablePresetName}, {WriterID}, {ReceiverID}";
    }
}
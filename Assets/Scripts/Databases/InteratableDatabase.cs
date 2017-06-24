using UnityEngine;
using System.Collections.Generic;

public static class InteractableDatabase
{
    public static bool DataExists = false;

    public static InteractableArray GetInteractableArray()
    {
        string raw = Helpers.LoadGameFile(InformationSet.InteractableInformation);
        return JsonUtility.FromJson<InteractableArray>(raw);
    }

    private static Dictionary<string, InteractableData> _interactableData;
    private static Dictionary<string, InteractableData> _interactables
    {
        get
        {
            if (_interactableData == null)
            {
                LoadInteractableData();
            }
            return _interactableData;
        }
    }

    public static bool LoadInteractableData()
    {
        _interactableData = new Dictionary<string, InteractableData>();
        InteractableArray interactableArray = GetInteractableArray();
        if (interactableArray != null)
        { 
            for (int i = 0; i < interactableArray.Interactables.Length; i++)
            {
                InteractableData interactable = interactableArray.Interactables[i];
                _interactables.Add(interactable.InteractableId, interactable);
            }
            return true;
        }

        return false;
    }

    public static InteractableData GetInteractableData(string interactableId)
    {
        return _interactables[interactableId];
    }
}

[System.Serializable]
public class InteractableData : IEditable
{
    public string InteractableId;
    public string Name;
    public string Description;
    public Interaction[] Interactions;
}

[System.Serializable]
public class InteractableArray : IEditable
{
    public InteractableData[] Interactables;
}

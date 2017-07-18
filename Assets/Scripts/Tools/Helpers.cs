using UnityEngine;
using System;
using System.IO;
using System.Linq;

public static class Helpers
{
    public static bool LooseCompare(string stringa, string stringb)
    {
        return stringa.Trim().ToLower() == stringb.Trim().ToLower();
    }

    public static string LoadGameFile(InformationSet informationSet)
    {
        string path = GetPathFromInformationSet(informationSet);
        if (File.Exists(path))
            return File.ReadAllText(path);

        return "";
    }
    
    public static string LoadNameGenerationFile(NameGenerationStringType type)
    {
        string path = GetPathFromNameGenerationSet(type);
        if (File.Exists(path))
            return File.ReadAllText(path);

        return "";
    }

    public static void CreateAllGameFiles()
    {
        if (!Directory.Exists(GameInfo.CurrentGamePath))
            Directory.CreateDirectory(GameInfo.CurrentGamePath);

        InformationSet[] infoSets = Enum.GetValues(typeof(InformationSet)).Cast<InformationSet>().ToArray();
        for (int i = 0; i < infoSets.Length; i++)
        {
            if (infoSets[i] != InformationSet.NameGeneration)
            {
                string path = GetPathFromInformationSet(infoSets[i]);

                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!File.Exists(path))
                {
                    Type type = GetTypeFromInformationSet(infoSets[i]);
                    object data = Activator.CreateInstance(type);
                    string json = JsonUtility.ToJson(data);
                    SaveGameFile(json, infoSets[i]);
                }
            }
        }

        NameGenerationStringType[] nameGenerationInfoSets = Enum.GetValues(typeof(NameGenerationStringType)).Cast<NameGenerationStringType>().ToArray();
        for (int i = 0; i < nameGenerationInfoSets.Length; i++)
        {
            string path = GetPathFromNameGenerationSet(nameGenerationInfoSets[i]);

            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(path))
                File.Create(path);
        }
    }

    public static bool SaveGameFile(string json, InformationSet informationSet)
    {
        string path = GetPathFromInformationSet(informationSet);
        return SaveFile(json, path);     
    }

    public static bool SaveNameGenerationFile(string text, NameGenerationStringType type)
    {
        string path = GetPathFromNameGenerationSet(type);
        return SaveFile(text, path);
    }

    public static bool SaveFile(string text, string path)
    {
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        try
        {
            Debug.Log("Saving to: " + path);
            File.WriteAllText(path, text);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Save Failed: " + e.Message);
            return false;
        }
    }

    public static string GetPathFromInformationSet(InformationSet infoSet)
    {
        switch (infoSet)
        {
            case InformationSet.WorldInformation:
                return GameInfo.WorldDataPath;
            case InformationSet.LevelInformation:
                return GameInfo.LevelDataPath;
            case InformationSet.RoomInformation:
                return GameInfo.RoomDataPath;
            case InformationSet.InteractableInformation:
                return GameInfo.InteractableDataPath;
            case InformationSet.ObstacleInformation:
                return GameInfo.ObstacleDataPath;
            case InformationSet.ItemInformation:
                return GameInfo.ItemDataPath;
            case InformationSet.MessageInformation:
                return GameInfo.MessageDataPath;
        }
        return "";
    }

    public static string GetPathFromNameGenerationSet(NameGenerationStringType type)
    {
        switch (type)
        {
            case NameGenerationStringType.RoomNoun:
                return GameInfo.RoomNounPath;
            case NameGenerationStringType.Noun:
                return GameInfo.NounPath;
            case NameGenerationStringType.Verb:
                return GameInfo.VerbPath;
        }
        return "";
    }

    public static Type GetTypeFromInformationSet(InformationSet infoSet)
    {
        switch (infoSet)
        {
            case InformationSet.WorldInformation:
                return typeof(WorldData);
            case InformationSet.LevelInformation:
                return typeof(LevelArray);
            case InformationSet.RoomInformation:
                return typeof(RoomArray);
            case InformationSet.InteractableInformation:
                return typeof(InteractableArray);
            case InformationSet.ObstacleInformation:
                return typeof(ObstacleArray);
            case InformationSet.ItemInformation:
                return typeof(ItemArray);
        }
        return null;
    }

    public static void ShowCanvasGroup(CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public static void HideCanvasGroup(CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}

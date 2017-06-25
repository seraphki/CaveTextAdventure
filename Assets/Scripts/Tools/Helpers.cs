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

    public static void CreateAllGameFiles()
    {
        if (!Directory.Exists(GameInfo.CurrentGamePath))
            Directory.CreateDirectory(GameInfo.CurrentGamePath);

        InformationSet[] infoSets = Enum.GetValues(typeof(InformationSet)).Cast<InformationSet>().ToArray();
        for (int i = 0; i < infoSets.Length; i++)
        {
            string path = GetPathFromInformationSet(infoSets[i]);
            if (!File.Exists(path))
            {
                Type type = GetTypeFromInformationSet(infoSets[i]);
                object data = Activator.CreateInstance(type);
                string json = JsonUtility.ToJson(data);
                SaveGameFile(json, infoSets[i]);
            }
        }
    }

    public static bool SaveGameFile(string json, InformationSet informationSet)
    {
        string directory = GameInfo.CurrentGamePath;
        string path = GetPathFromInformationSet(informationSet);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        try
        {
            File.WriteAllText(path, json);
            return true;
        }
        catch
        {
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
}

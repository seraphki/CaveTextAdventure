using UnityEngine;
using System.IO;

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
}

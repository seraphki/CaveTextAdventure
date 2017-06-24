using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class LevelDatabase
{
    public static LevelArray GetLevelArrayData()
    {
        string raw = Helpers.LoadGameFile(InformationSet.LevelInformation);
        return  JsonUtility.FromJson<LevelArray>(raw);
    }

    private static Dictionary<int, LevelData> _levelsData;
    private static Dictionary<int, LevelData> _levels
    {
        get
        {
            if (_levelsData == null)
            {
                LoadLevelData();
            }
            return _levelsData;
        }
    }

    public static bool LoadLevelData()
    {
        _levelsData = new Dictionary<int, LevelData>();

        LevelArray levelArrayData = GetLevelArrayData();
        if (levelArrayData != null)
        {
            LevelData[] levels = levelArrayData.Levels.OrderBy(l => l.LevelIndex).ToArray();
            for (int i = 0; i < levelArrayData.Levels.Length; i++)
            {
                LevelData level = levelArrayData.Levels[i];
                _levels.Add(i + 1, level);
            }
            return true;
        }

        return false;
    }

    private static int? _levelCount;
    public static int LevelCount
    {
        get
        {
            if (_levelCount == null)
            {
                _levelCount = _levels.Count;
            }
            return (int)_levelCount;
        }
    }

    public static LevelData GetLevelByIndex(int index)
    {
        return _levels[index];
    }

    public static string[] GetRoomIdsForLevel(int index)
    {
        return _levels[index].RoomIds;
    }
}

[System.Serializable]
public class LevelData : IEditable
{
    public int LevelIndex;
    public string StartMessage;
    public int RoomCount;
    public string[] RoomIds;
}

[System.Serializable]
public class LevelArray : IEditable
{
    public LevelData[] Levels;
}
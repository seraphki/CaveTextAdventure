using UnityEngine;

public static class WorldDatabase
{
    private static WorldData _worldData;
    private static WorldData _world
    {
        get
        {
            if (_worldData == null)
            {
                LoadWorldData();
            }
            return _worldData;
        }
    }

    public static bool LoadWorldData()
    {
        string raw = Helpers.LoadGameFile(InformationSet.WorldInformation);
        _worldData = JsonUtility.FromJson<WorldData>(raw);
        if (_worldData != null)
            return true;

        return false;
    }

    public static WorldData GetWorldData()
    {
        return _world;
    }
}

public class WorldData : IEditable
{
    public string WorldName;
    public string StartMessage;
    public string EndMessage;
    public bool ProgressDown = true;
}

using UnityEngine;

public static class GameInfo
{
    private const string WORLDDATASUBPATH = "WorldData.json";
    public static string WorldDataPath { get { return CurrentGamePath + WORLDDATASUBPATH; } }

    private const string LEVELDATASUBPATH = "LevelData.json";
    public static string LevelDataPath { get { return CurrentGamePath + LEVELDATASUBPATH; } }

    private const string ROOMDATASUBPATH = "RoomData.json";
    public static string RoomDataPath { get { return CurrentGamePath + ROOMDATASUBPATH; } }

    private const string INTERACTABLEDATASUBPATH = "InteractableData.json";
    public static string InteractableDataPath { get { return CurrentGamePath + INTERACTABLEDATASUBPATH; } }

    private const string OBSTACLEDATASUBPATH = "ObstacleData.json";
    public static string ObstacleDataPath { get { return CurrentGamePath + OBSTACLEDATASUBPATH; } }

    private const string ITEMDATASUBPATH = "ItemData.json";
    public static string ItemDataPath { get { return CurrentGamePath + ITEMDATASUBPATH; } }

    private static string _currentGamePath;
    public static string CurrentGamePath
    {
        get
        {
            if (string.IsNullOrEmpty(_currentGamePath))
            {
                _currentGamePath = GamesPath + "/" + CurrentGame + "/Data/";
            }
            return _currentGamePath;
        }
    }

    public static string GamesPath
    {
        get { return Application.persistentDataPath; }
    }

    private static string _currentGame = "Test";
    public static string CurrentGame
    {
        get { return _currentGame; }
    }

    public static void SetCurrentGame(string gameName)
    {
        _currentGame = gameName;
    }
}

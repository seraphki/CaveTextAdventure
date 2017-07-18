using UnityEngine;

public static class GameInfo
{
    private const string DATASUBPATH = "Data";
    private const string TEXTSUBPATH = "Text";

    private const string WORLDDATASUBPATH = "WorldData.json";
    public static string WorldDataPath { get { return CurrentGamePath + "/" + DATASUBPATH + "/" + WORLDDATASUBPATH; } }

    private const string LEVELDATASUBPATH = "LevelData.json";
    public static string LevelDataPath { get { return CurrentGamePath + "/" + DATASUBPATH + "/" + LEVELDATASUBPATH; } }

    private const string ROOMDATASUBPATH = "RoomData.json";
    public static string RoomDataPath { get { return CurrentGamePath + "/" + DATASUBPATH + "/" + ROOMDATASUBPATH; } }

    private const string INTERACTABLEDATASUBPATH = "InteractableData.json";
    public static string InteractableDataPath { get { return CurrentGamePath + "/" + DATASUBPATH + "/" + INTERACTABLEDATASUBPATH; } }

    private const string OBSTACLEDATASUBPATH = "ObstacleData.json";
    public static string ObstacleDataPath { get { return CurrentGamePath + "/" + DATASUBPATH + "/" + OBSTACLEDATASUBPATH; } }

    private const string ITEMDATASUBPATH = "ItemData.json";
    public static string ItemDataPath { get { return CurrentGamePath + "/" + DATASUBPATH + "/" + ITEMDATASUBPATH; } }

    private const string MESSAGEDATASUBPATH = "MessageData.json";
    public static string MessageDataPath { get { return CurrentGame + "/" + DATASUBPATH + "/" + MESSAGEDATASUBPATH; } }

    private const string ROOMNOUNSUBPATH = "RoomNouns.txt";
    public static string RoomNounPath { get { return CurrentGamePath + "/" + TEXTSUBPATH + "/" + ROOMNOUNSUBPATH; } }

    private const string NOUNSUBPATH = "Nouns.txt";
    public static string NounPath { get { return CurrentGamePath + "/" + TEXTSUBPATH + "/" + NOUNSUBPATH; } }

    private const string VERBSUBPATH = "Verbs.txt";
    public static string VerbPath { get { return CurrentGamePath + "/" + TEXTSUBPATH + "/" + VERBSUBPATH; } }

    private static string _currentGamePath;
    public static string CurrentGamePath
    {
        get
        {
            if (string.IsNullOrEmpty(_currentGamePath))
            {
                _currentGamePath = GamesPath + "/" + CurrentGame;
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

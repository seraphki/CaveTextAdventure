using UnityEngine;
using System.Collections.Generic;

public class CaveManager : MonoSingleton<CaveManager>
{
    public int CaveDepth;

    private Cave _cave;

    private Location _location;
    public struct Location
    {
        public int LevelId;
        public int RoomId;

        public Location(int level, int room)
        {
            LevelId = level;
            RoomId = room;
        }
    }

    private Dictionary<int, int[]> _levelItems;
    private Dictionary<int, int[]> _levelEnemies;

	void Start()
    {
        GetLevelData();
        LoadCave();

        _location = new Location(0, 0);
        UIController.Instance.TextOutputUpdate("You have entered the cave.");
        GetCurrentRoom().Enter();
    }

    #region Public Methods

    public void DoAction(string actionString)
    {
        string[] actionParse = actionString.Split(' ');
        string action = actionParse[0];

        if (action == "Look")
        {
            GetCurrentRoom().Look();
        }
    }

    public void Move(int direction)
    {
        UIController.Instance.TextOutputNewLine();
        Advance((int)direction);
    }

    public void MoveLevel(int direction)
    {
        UIController.Instance.TextOutputNewLine();
        AdvanceLevel(direction);
    }

    public Room GetCurrentRoom()
    {
        return _cave.Levels[_location.LevelId].Rooms[_location.RoomId];
    }

    public int GetRandomLevelItemId(int level)
    {
        if (_levelItems[level].Length > 0)
        {
            int index = Random.Range(0, _levelItems[level].Length);
            return _levelItems[level][index];
        }
        return 0;
    }

    public int GetRandomLevelEnemyId(int level)
    { 
        if (_levelEnemies[level].Length > 0)
        {
            int index = Random.Range(0, _levelEnemies[level].Length);
            int enemyId = _levelEnemies[level][index];
            return enemyId;
        }
        return 0;
    }

    #endregion

    #region Private Methods

    private void Advance(int direction)
    {
        Level level = _cave.Levels[_location.LevelId];
        Room room = level.Rooms[_location.RoomId];

        int connectingRoom = room.GetConnectingRoom(direction);
        if (connectingRoom >= 0)
        {
            _location.RoomId = connectingRoom;
            Room newRoom = GetCurrentRoom();
            UIController.Instance.TextOutputUpdate("You went " + ((Direction)direction).ToString() + " into the next room: " + newRoom.RoomName);
            newRoom.Enter();
        }
        else
        {
            UIController.Instance.TextOutputUpdate("There are no rooms in that direction.");
        }
    }

    private void AdvanceLevel(int direction)
    {
        _location.LevelId += direction;
        _location.RoomId = 0;

        if (_location.LevelId > _cave.Levels.Length - 1)
        {
            UIController.Instance.DisableInteractionButtons();
            UIController.Instance.TextOutputUpdate("You've reached the end of the cave");
        }
        else if (_location.LevelId < 0)
        {
            UIController.Instance.DisableInteractionButtons();
            UIController.Instance.TextOutputUpdate("You've left the cave");
        }
        else
        {
            GetCurrentRoom().Enter();
        }
    }

    private void GetLevelData()
    {
        _levelItems = new Dictionary<int, int[]>();
        _levelEnemies = new Dictionary<int, int[]>();
        
        for (int level = 0; level < CaveDepth; level++)
        {        
            _levelItems.Add(level, ItemLibrary.Instance.GetItemIdsByLevel(level));
            _levelEnemies.Add(level, EnemyLibrary.Instance.GetEnemyIdsByLevel(level));
        }
    }

    private void LoadCave()
    {
        //TODO: load from file
        _cave = new Cave(CaveDepth);
    }

    #endregion
}


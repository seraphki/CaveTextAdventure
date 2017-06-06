using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Room
{
    public string RoomName;
    public int EnemyId;
    public Chest RoomChest;
    public bool Exit = false;
    public bool Entrance = false;
    public Vector2 Position;
	public int LastEnteredDirection;

    private int _level;
    private int[] _connectedRooms;

    private int _connections = 0;
    public int Connections
    {
        get { return _connections; }
    }

    public Room(int level)
    {
        _connectedRooms = new int[4] { -1, -1, -1, -1 };
        _level = level;
        RoomName = RoomNameGenerator.GetRandomName();
    }

    public void GenerateChest()
    {
        RoomChest = new Chest(_level);
    }

    public void GenerateEnemy()
    {
        EnemyId = CaveManager.Instance.GetRandomLevelEnemyId(_level);
    }

    public void Enter(int direction)
    {
		LastEnteredDirection = direction;
        Look();

        if (Entrance)
        {
            UIController.Instance.EnablePreviousLevelButton();
        }
        else if (Exit)
        {
            UIController.Instance.EnableNextLevelButton();
        }
        else if (EnemyId > 0)
        {
            //Hook up engage enemy button
            BattleManager.Instance.EnemyId = EnemyId;
            UIController.Instance.SetEnemyButton(new UnityAction(BattleManager.Instance.StartBattle));
        }     
        else if (RoomChest != null)
        {
            UIController.Instance.SetChestButton(new UnityAction(RoomChest.OpenChest));
        }
        else
        {
            UIController.Instance.DisableInteractionButtons();
        }
    }

    public void Look()
    {
		MessageManager.Instance.SendRoomMessage(this);
    }

    public int GetConnectingRoom(int direction)
    {
        return _connectedRooms[direction];
    }

	public List<int> GetConnectingDirections()
	{
		List<int> directions = new List<int>();
		for (int i = 0; i < _connectedRooms.Length; i++)
		{
			if (_connectedRooms[i] >= 0)
			{
				directions.Add(i);
			}
		}
		return directions;
	}

    public int GetConnectionDirection(List<int> possibleDirections)
    {
        int direction = GetRandomRemainingDirection(possibleDirections);
        if (direction >= 0)
        {
            return direction;
        }
        return -1;
    }

    public void AddConnectingRoom(int direction, int roomId)
    {
        _connectedRooms[direction] = roomId;
        _connections++;
    }

    private int GetRandomRemainingDirection(List<int> possibleDirections)
    {
        List<int> emptyRooms = new List<int>();
        for (int i = 0; i < _connectedRooms.Length; i++)
        {
            if (_connectedRooms[i] == -1 && possibleDirections.Contains(i))
                emptyRooms.Add(i);
        }

        if (emptyRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyRooms.Count);
            return emptyRooms[randomIndex];
        }
        return -1;
    }
}

public class RoomInformation
{
	public string RoomName;
	public int EnemyId;
	public Chest RoomChest;
	public bool Exit = false;
	public bool Entrance = false;
	public Vector2 Position;
}

public static class RoomNameGenerator
{
    private enum StringType { RoomNoun, Noun, Adjective }

    private static string[] _roomNouns;
    public static string[] RoomNouns
    {
        get
        {
            if (_roomNouns == null)
            {
                TextAsset textAsset = Resources.Load("Text/RoomNouns") as TextAsset;
                _roomNouns = textAsset.text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _roomNouns;
        }
    }

    private static string[] _nouns;
    public static string[] Nouns
    {
        get
        {
            if (_nouns == null)
            {
                TextAsset textAsset = Resources.Load("Text/Nouns") as TextAsset;
                _nouns = textAsset.text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _nouns;
        }
    }

    private static string[] _adjectives;
    public static string[] Adjectives
    {
        get
        {
            if (_adjectives == null)
            {
                TextAsset textAsset = Resources.Load("Text/Adjectives") as TextAsset;
                _adjectives = textAsset.text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _adjectives;
        }
    }

    public static string GetRandomName()
    {
        int style = Random.Range(0, 3);
        string name = "";

        if (style == 0)
        {
            name = GetRandomString(StringType.RoomNoun) + " of " + GetRandomString(StringType.Noun);
        }
        else if (style == 1)
        {
            name = "The " + GetRandomString(StringType.Adjective) + " " + GetRandomString(StringType.RoomNoun);
        }
        else
        {
            name = "The " + GetRandomString(StringType.Adjective) + " " + GetRandomString(StringType.RoomNoun)
                + " of " + GetRandomString(StringType.Noun);
        }

        return name;
    }


    private static string GetRandomString(StringType type)
    {
        if (type == StringType.Adjective)
        {
            int index = Random.Range(0, Adjectives.Length);
            return Adjectives[index];
        }
        else if (type == StringType.Noun)
        {
            int index = Random.Range(0, Nouns.Length);
            return Nouns[index];
        }
        else if (type == StringType.RoomNoun)
        {
            int index = Random.Range(0, RoomNouns.Length);
            return RoomNouns[index];
        }
        return "";
    }
}

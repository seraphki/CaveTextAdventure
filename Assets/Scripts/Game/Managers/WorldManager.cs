using UnityEngine;

public class WorldManager
{
    private World _world;

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

	public WorldManager()
    {
        _location = new Location(0, 0);
        _world = new World();
	}

    public void EnterWorld()
    {
        //Send World Enter Message
        MessageManager.SendStringMessage(_world.StaticData.StartMessage);
        
        Level level = GetCurrentLevel();
        if (level != null)
        {
            //Send Level Enter Message
            MessageManager.SendStringMessage(level.StaticData.StartMessage);

            Room room = GetCurrentRoom();
            if (room != null)
            {
                room.Enter(-1);
            }
            else
            {
                Debug.LogWarning("Room information Missing!");
            }
        }
        else
        {
            Debug.LogWarning("Level information Missing!");
        }
    }

    #region Public Methods

    public void SubmitAction(string action, string target)
    {
        if (Helpers.LooseCompare(action, "go"))
        {
            //Movement Actions
            Direction direction = (Direction)System.Enum.Parse(typeof(Direction), target.ToLower());
            Advance((int)direction);
        }
        else if (Helpers.LooseCompare(action, "examine"))
        {
            //Examine Action
            GetCurrentRoom().SubmitExamineAction(target);
        }
        else
        {
            //Send action to Room
            GetCurrentRoom().SubmitAction(action, target);
        }
    }

    public void Look()
    {
        GetCurrentRoom().Look();
    }

    public void Move(int direction)
    {
        Advance(direction);
    }

    public void MoveLevel(int direction)
    {
        AdvanceLevel(direction);
    }

    public Level GetCurrentLevel()
    {
        Level level = _world.GetLevel(_location.LevelId);
        if (level != null)
        {
            return level;
        }

        return null;
    }

    public Room GetCurrentRoom()
    {
        Level level = _world.GetLevel(_location.LevelId);
        if (level != null)
        {
            Room room = level.GetRoom(_location.RoomId);
            if (room != null)
            {
                return room;
            }
        }
        return null;
    }

    #endregion

    #region Private Methods

    private void Advance(int direction)
    {
        Level level = _world.GetLevel(_location.LevelId);
        Room room = level.GetRoom(_location.RoomId);

        int connectingRoom = room.GetConnectingRoom(direction);
        if (connectingRoom >= 0)
        {
            _location.RoomId = connectingRoom;
            Room newRoom = GetCurrentRoom();
            newRoom.Enter(direction);
        }
        else
        {
			MessageManager.SendNoRoomMessage();
        }
    }

    private void AdvanceLevel(int direction)
    {
        _location.LevelId += direction;
        _location.RoomId = 0;

        if (_location.LevelId > _world.LevelCount - 1)
        {
			MessageManager.SendEndOfCaveMessage();
        }
        else if (_location.LevelId < 0)
        {
			MessageManager.SendLeftCaveMessage();
        }
        else
        {
            GetCurrentRoom().Enter(-1);
        }
    }

    #endregion
}


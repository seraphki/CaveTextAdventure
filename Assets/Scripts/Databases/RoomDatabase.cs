using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class RoomDatabase
{
    public static RoomArray GetRoomArayData()
    {
        string raw = Helpers.LoadGameFile(InformationSet.RoomInformation);
        return JsonUtility.FromJson<RoomArray>(raw);
    }

    private static Dictionary<string, RoomData> _roomsData;
    private static Dictionary<string, RoomData> _rooms
    {
        get
        {
            if (_roomsData == null)
            {
                LoadRoomData();
            }
            return _roomsData;
        }
    }

    public static bool LoadRoomData()
    {
        _roomsData = new Dictionary<string, RoomData>();
        _usedRooms = new List<string>();

        RoomArray roomArray = GetRoomArayData();
        if (roomArray != null)
        {
            for (int i = 0; i < roomArray.Rooms.Length; i++)
            {
                RoomData room = roomArray.Rooms[i];
                if (room.Name.ToLower() == "random")
                {
                    room.Name = RoomNameGenerator.GetRandomName();
                }

                _roomsData.Add(room.RoomId, room);
            }
            return true;
        }

        return false;
    }

    public static int RoomCount
    {
        get
        {
            return _rooms.Keys.Count;
        }
    }

    private static List<string> _usedRooms;
    private static List<string> _unusedRooms
    {
        get
        {
            return _rooms.Keys.Where(r => !_usedRooms.Contains(r)).ToList();
        }
    }

    public static RoomData GetRoomData(string roomId)
    {
        if (_rooms.ContainsKey(roomId))
            return _rooms[roomId];
        else
        {
            Debug.LogWarning("Couldn't find roomId " + roomId);
            return GetFillerRoom();
        }
    }

    public static RoomData GetRandomUnusedRoomId()
    {
        if (_unusedRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, _unusedRooms.Count);
            string roomId = _unusedRooms[randomIndex];
            _usedRooms.Add(roomId);
            return _rooms[roomId];
        }

        return GetFillerRoom();
    }

    public static RoomData GetFillerRoom()
    {
        RoomData data = new RoomData();
        data.Name = RoomNameGenerator.GetRandomName();
        return data;
    }
}

[System.Serializable]
public class RoomData : IEditable
{
    public string RoomId;
    public string Name;
    public string Description;
    public string ObstacleId;
    public string[] InteractableIds;
}

[System.Serializable]
public class RoomArray : IEditable
{
    public RoomData[] Rooms;
}
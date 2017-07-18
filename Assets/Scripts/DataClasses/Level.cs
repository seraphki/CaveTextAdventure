using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Level
{
    public LevelData StaticData;

    private Room[] _rooms;
    private int _exitPoint;
    private float _roomDataRate;
    private List<Vector2> _roomPositions;

    public Level(int level)
    {
        StaticData = LevelDatabase.GetLevelByIndex(level);
        GenerateMap(StaticData.RoomCount);
    }

    public Room GetRoom(int index)
    {
        if (index < _rooms.Length)
            return _rooms[index];

        return null;
    }

    private void GenerateMap(int roomCount)
    {
        _rooms = new Room[roomCount];
        for (int i = 0; i < roomCount; i++)
        {
            _rooms[i] = new Room();
        }

        //Create network
        Queue<int> roomsToBeConneted = new Queue<int>();
        for (int i = 1; i < _rooms.Length; i++)
            roomsToBeConneted.Enqueue(i);

        Stack<int> connectedRooms = new Stack<int>();

        List<int> roomsToConnect = new List<int>() { 0 };
        _roomPositions = new List<Vector2>();
        _roomPositions.Add(Vector2.zero);
        _rooms[0].Entrance = true;

        while (roomsToBeConneted.Count > 0)
        {
            //Get room from list of rooms to be connected
            int nextRoomIndex = Random.Range(0, roomsToConnect.Count);
            int roomIndex = roomsToConnect[nextRoomIndex];
            roomsToConnect.RemoveAt(nextRoomIndex);
            Room currentRoom = _rooms[roomIndex];

            //Create Connections - I want a max of 3 (including "entrance")
            int connections = Random.Range(1, 3);
            for (int i = 0; i < connections; i++)
            {
                bool connected = false;
                if (roomsToBeConneted.Count > 0)
                {
                    //Get a branching direction
                    List<int> possibleDirections = GetPossibleDirections(currentRoom.Position);
                    int direction = currentRoom.GetConnectionDirection(possibleDirections);

                    if (direction >= 0)
                    {
                        connected = true;

                        int connectingRoom = roomsToBeConneted.Dequeue();
                        currentRoom.AddConnectingRoom(direction, connectingRoom);
                        _rooms[connectingRoom].AddConnectingRoom((direction + 2) % 4, roomIndex);

                        //Set position of newly connected room
                        Vector2 position = currentRoom.Position + VectorFromDirection(direction);
                        _rooms[connectingRoom].Position = position;
                        _roomPositions.Add(position);

                        //Add connected room to queue for connection
                        roomsToConnect.Add(connectingRoom);
                    }

                    if (!connected)
                    {
                        //Backtrack to find another branch route
                        roomsToConnect.Add(connectedRooms.Pop());
                    }
                }
            }
        }

        //Get Endpoints
        Stack<int> endCaps = new Stack<int>();
        for (int i = 1; i < _rooms.Length; i++)
        {
            if (_rooms[i].Connections <= 1)
            {
                endCaps.Push(i);
            }
        }

        //TODO: If user wants random content, put in endcaps

        //Last end cap is the exit point
        _exitPoint = endCaps.Pop();
        _rooms[_exitPoint].StaticData = RoomDatabase.GetFillerRoom();
        _rooms[_exitPoint].Exit = true;

        //Fill start point
        _rooms[0].StaticData = RoomDatabase.GetFillerRoom();

        //Fill in data
        List<string> roomDataIndicies = StaticData.RoomIds.ToList();
        List<int> roomIndecies = new List<int>();
        for (int i = 1; i < roomCount; i++)
        {
            if (i != _exitPoint)
                roomIndecies.Add(i);
        }
        
        while (roomIndecies.Count > 0)
        {
            int i = Random.Range(0, roomIndecies.Count);
            int roomIndex = roomIndecies[i];
            roomIndecies.Remove(roomIndex);

            string roomId = null;
            if (roomDataIndicies.Count > 0)
            {
                int index = Random.Range(0, roomDataIndicies.Count);
                roomId = roomDataIndicies[index];
                roomDataIndicies.Remove(roomId);
            }

            RoomData data = roomId == null ? RoomDatabase.GetFillerRoom() : RoomDatabase.GetRoomData(roomId);
            _rooms[roomIndex].Setup(data);
        }
    }

    private Vector2 VectorFromDirection(int direction)
    {
        if (direction == 0)
            return Vector2.up;
        else if (direction == 1)
            return Vector2.right;
        else if (direction == 2)
            return Vector2.down;
        else
            return Vector2.left;
    }

    private List<int> GetPossibleDirections(Vector2 position)
    {
        List<int> possibleDirections = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (!_roomPositions.Contains(position + VectorFromDirection(i)))
            {
                possibleDirections.Add(i);
            }
        }
        return possibleDirections;
    }
}
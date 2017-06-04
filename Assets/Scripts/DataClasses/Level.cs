using UnityEngine;
using System.Collections.Generic;

public class Level
{
    public Room[] Rooms;

    private int _level;
    private int _exitPoint;
    private List<Vector2> _roomPositions;

    public Level(int level, int roomCount)
    {
        _level = level;
        GenerateLevel(roomCount);
    }

    private void GenerateLevel(int roomCount)
    {
        //TODO: Optimize checking for overlaps

        Rooms = new Room[roomCount];
        
        //Generate all rooms
        for (int i = 0; i < roomCount; i++)
        {
            Rooms[i] = new Room(_level);
        }

        //Create network
        Queue<int> roomsToBeConneted = new Queue<int>();
        for (int i = 1; i < Rooms.Length; i++)
            roomsToBeConneted.Enqueue(i);

        Stack<int> connectedRooms = new Stack<int>();

        List<int> roomsToConnect = new List<int>() { 0 };
        _roomPositions = new List<Vector2>();
        _roomPositions.Add(Vector2.zero);
        Rooms[0].Entrance = true;

        while (roomsToBeConneted.Count > 0)
        {
            //Get room from list of rooms to be connected
            int nextRoomIndex = Random.Range(0, roomsToConnect.Count);
            int roomIndex = roomsToConnect[nextRoomIndex];
            roomsToConnect.RemoveAt(nextRoomIndex);
            Room currentRoom = Rooms[roomIndex];

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
                        Rooms[connectingRoom].AddConnectingRoom((direction + 2) % 4, roomIndex);

                        //Set position of newly connected room
                        Vector2 position = currentRoom.Position + VectorFromDirection(direction);
                        Rooms[connectingRoom].Position = position;
                        _roomPositions.Add(position);

                        //Add connected room to queue for connection
                        roomsToConnect.Add(connectingRoom);

                        //Debug.Log("Connecting " + roomIndex + " to " + connectingRoom + " to the " + ((Direction)direction).ToString());
                        //Debug.Log("Connecting " + connectingRoom + " to " + roomIndex + " to the " + ((Direction)((direction + 2) % 4)).ToString());
                    }

                    if (!connected)
                    {
                        //Backtrack to find another branch route
                        roomsToConnect.Add(connectedRooms.Pop());
                    }
                }
            }
        }

        //Now that all the rooms are connected, generate conent in end points
        Stack<int> endCaps = new Stack<int>();
        for (int i = 1; i < Rooms.Length; i++)
        {
            if (Rooms[i].Connections <= 1)
            {
                endCaps.Push(i);
            }
        }
        Debug.Log("Endcaps: " + endCaps.Count);

        //Last end cap is the exit point
        _exitPoint = endCaps.Pop();
        Rooms[_exitPoint].Exit = true;

        //Generate items and enemies
        while (endCaps.Count > 0)
        {
            int roomId = endCaps.Pop();
            if (Random.Range(0f, 1f) < 0.5f)
            {
                Debug.Log("Adding enemy to room " + roomId);
                Rooms[roomId].GenerateEnemy();
            }
            else
            {
                Debug.Log("Adding chest to room " + roomId);
                Rooms[roomId].GenerateChest();
            }
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
        //TODO: Check in on making this more efficient
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

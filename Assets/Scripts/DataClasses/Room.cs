using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Room
{
    public RoomData StaticData;
    public bool Exit = false;
    public bool Entrance = false;
    public Vector2 Position;
	public int LastEnteredDirection;
    
    private int[] _connectedRooms;

    private bool _visited = false;
    private int _connections = 0;
    public int Connections
    {
        get { return _connections; }
    }

    public Room()
    {
        _connectedRooms = new int[4] { -1, -1, -1, -1 };
    }

    public void Setup(RoomData staticData)
    {
        StaticData = staticData;
    }

    public void Enter(int direction)
    {
        LastEnteredDirection = direction;

        //Send room message. If visited previously, long message. Otherwise, short.
        if (!_visited)
        {
            MessageManager.SendFullRoomMessage(this);
            _visited = true;
        }
        else
        {
            MessageManager.SendShortRoomMessage(this);
        }

        //Notify the Obstacle Controller that we've encountered an obstacle
        if (!string.IsNullOrEmpty(StaticData.ObstacleId))
        {
            Debug.Log("There is an obstacle. Sending to obstacle controller");
            ObstacleController.Instance.SetObstacle(StaticData.ObstacleId);

            //Remove from static data - once obstacle is complete, there is no need for it again
            StaticData.ObstacleId = null;
        }
    }

    public bool SubmitAction(string action, string target)
    {
        bool actionExecuted = false;

        //Check obstacle
        if (ObstacleController.Instance.ObstaclePresent())
        {
            actionExecuted = ObstacleController.Instance.SubmitObstacleAction(action, target);
        }

        if (!actionExecuted)
        {
            //Check through interactables
            for (int i = 0; i < StaticData.InteractableIds.Length; i++)
            {
                string interactableId = StaticData.InteractableIds[i];
                InteractableData data = InteractableDatabase.GetInteractableData(interactableId);
                for (int j = 0; j < data.Interactions.Length; j++)
                {
                    Interaction interaction = data.Interactions[j];
                    if ((Helpers.LooseCompare(target, interaction.Target) || Helpers.LooseCompare(target, data.Name)) && Helpers.LooseCompare(action, interaction.Action))
                    {
                        interaction.ExecuteInteractionOutcome();
                        actionExecuted = true;
                    }
                }
            }
        }

        return actionExecuted;
    }

    public bool SubmitExamineAction(string target)
    {
        for (int i = 0; i < StaticData.InteractableIds.Length; i++)
        {
            string interactableId = StaticData.InteractableIds[i];
            InteractableData data = InteractableDatabase.GetInteractableData(interactableId);
            if (Helpers.LooseCompare(target, data.Name))
            {
                MessageManager.SendStringMessage(data.Description);
                return true;
            }
        }

        return false;
    }

    public void Look()
    {
        Debug.Log("Look: " + StaticData.Description);
        MessageManager.SendStringMessage(StaticData.Description);
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


using UnityEngine;
using System.Collections.Generic;

public class Room
{
    public RoomData StaticData;
    public bool Exit = false;
    public bool Entrance = false;
    public Vector2 Position;
	public int LastEnteredDirection;

    private int[] _connectedRooms;

    private int _connections = 0;
    public int Connections
    {
        get { return _connections; }
    }

    public Room()
    {
        _connectedRooms = new int[4] { -1, -1, -1, -1 };
    }

    public void SubmitAction(string action, string target)
    {
        Debug.Log("Action submitted. " + action + " " + target);
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
                }
            }
        }
    }

    public void SubmitExamineAction(string target)
    {
        for (int i = 0; i < StaticData.InteractableIds.Length; i++)
        {
            string interactableId = StaticData.InteractableIds[i];
            InteractableData data = InteractableDatabase.GetInteractableData(interactableId);
            if (Helpers.LooseCompare(target, data.Name))
            {
                MessageManager.SendStringMessage(data.Description);
                break;
            }
        }
    }

    public void Enter(int direction)
    {
        Debug.Log("Enter");
		LastEnteredDirection = direction;
        MessageManager.SendEnterRoomMessage(this);
    }

    public void Look()
    {
        Debug.Log("Look");
		MessageManager.SendRoomMessage(this);
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


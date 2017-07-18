using UnityEngine;
using System.Collections.Generic;

public static class MessageManager
{
    private static MessageData _data;
    private static MessageData _messageData
    {
        get
        {
            if (_data == null)
            {
                _data = MessageDatabase.GetMessageData();
            }
            return _data;
        }
    }

    public static void SendStringMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            UIController.Instance.TextOutputUpdate(message);
        }
    }

    //Markup variables available:
    //{roomName} = Room Name
    //{description} = Room Description
    //{enterDirection} = Direction that room was entered from
    //{directionsAvailable} = Available directions message
    public static void SendFullRoomMessage(Room roomInfo)
    {
        string message = _messageData.FullRoomEnterMessage;

        //TODO: Make more efficient
        if (roomInfo.LastEnteredDirection == -1)
        {
            message = message.Replace("{enterDirection}", _messageData.WordForPreviousLevel);
        }
        else if (roomInfo.LastEnteredDirection == -2)
        {
            message = message.Replace("{enterDirection}", _messageData.WordForNextLevel);
        }
        else
        {
            Direction enterDirection = (Direction)(roomInfo.LastEnteredDirection + 2 % 3);
            message = message.Replace("{enterDirection}", GetWordForDirection(enterDirection));
        }

        message = message.Replace("{roomName}", roomInfo.StaticData.Name);
        message = message.Replace("{description}", roomInfo.StaticData.Description);
        message = message.Replace("{directionsAvailable}", GetDirectionsAvailableMessage(roomInfo.GetConnectingDirections(), roomInfo.Entrance, roomInfo.Exit));

        UIController.Instance.TextOutputUpdate(message);
    }

    //Markup variables available:
    //{roomName} = Room Name
    //{description} = Room Description
    //{enterDirection} = Direction that room was entered from
    //{directionsAvailable} = Available directions message
    public static void SendShortRoomMessage(Room roomInfo)
    {
        Direction enterDirection = (Direction)(roomInfo.LastEnteredDirection + 2 % 3);
        string message = _messageData.ShortRoomEnterMesssage;
        message = message.Replace("{roomName}", roomInfo.StaticData.Name);
        message = message.Replace("{description}", roomInfo.StaticData.Description);
        message = message.Replace("{enterDirection}", GetWordForDirection(enterDirection));
        message = message.Replace("{directionsAvailable}", GetDirectionsAvailableMessage(roomInfo.GetConnectingDirections(), roomInfo.Entrance, roomInfo.Exit));

        UIController.Instance.TextOutputUpdate(message);
    }

    //Markup variables available:
    //{direction1} first direction available
    //{direction2} second direction available
    //{direction3} third direction available
    //{direction4} fourth direction available
    private static string[] directionMarkups = { "{direction1}", "{direction2}", "{direction3}", "{direction4}" };
    private static string GetDirectionsAvailableMessage(List<int> directionsAvailable, bool entrance, bool exit)
    {
        string message = "";
        switch (directionsAvailable.Count)
        {
            case 1:
                message = _messageData.OneDirectionAvailableMessage;
                break;
            case 2:
                message = _messageData.TwoDirectionsAvailableMessage;
                break;
            case 3:
                message = _messageData.ThreeDirectionsAvailableMessage;
                break;
            case 4:
                message = _messageData.FourDirectionsAvailableMessage;
                break;
        }

        for (int i = 0; i < directionsAvailable.Count; i++)
        {
            message = message.Replace(directionMarkups[i], GetWordForDirection((Direction)directionsAvailable[i]));
        }

        if (entrance)
        {
            message += " " + _messageData.RoomIsEntranceMessage;
        }

        if (exit)
        {
            message += " " + _messageData.RoomIsExitMessage;
        }

        return message;
    }

    private static string GetWordForDirection(Direction direction)
    {
        if (direction == Direction.north)
            return _messageData.WordForNorth;
        else if (direction == Direction.south)
            return _messageData.WordForSouth;
        else if (direction == Direction.east)
            return _messageData.WordForEast;
        else
            return _messageData.WordForWest;
    }

	public static void SendNoRoomMessage()
	{
        string message = _messageData.NoRoomsInDirectionMessage;
		UIController.Instance.TextOutputUpdate(message);
	}


    public static void SendInventoryMessage()
    {
        string message = "";
        string[] items = GameController.InventoryManagerInst.GetOwnedItemIds();
        
        if (items.Length <= 0)
        {
            message += _messageData.InventoryNoItemsMessage;
        }
        else
        {
            message += _messageData.InventoryListHeaderMessage + "\n";
            for (int i = 0; i < items.Length; i++)
            {
                int owned = GameController.InventoryManagerInst.GetItemAmountOwned(items[i]);
                string name = ItemDatabase.GetItemData(items[i]).Name;
                message += GetListItemMessage(name, owned.ToString());
            }
        }

        UIController.Instance.TextOutputUpdate(message);
    }

    //Markup variables available:
    //{itemName} Name of Item
    //{itemCount} Number of Item
    private static string GetListItemMessage(string itemName, string amount)
    {
        string message = _messageData.InventoryListItemFormat;
        message = message.Replace("{itemName}", itemName);
        message = message.Replace("{itemCount}", amount);
        return message;
    }

    //Markup variables available:
    //{itemName} Name of Item
    //{itemCount} Number of Items Remaining
	public static void SendItemUsedMessage(string itemName, int itemsRemaining)
	{
        string message = _messageData.ItemUsedMessage;
        message = message.Replace("{itemName}", itemName);
        message = message.Replace("{itemCount}", itemsRemaining.ToString());

		UIController.Instance.TextOutputUpdate(message);
	}

    //Markup variables available:
    //{itemName} Name of Item
    //{itemCount} Number of Items Owned
    public static void SendItemAddedMessage(string itemName, int itemsOwned)
    {
        string message = _messageData.ItemAddedMessage;
        message = message.Replace("{itemName}", itemName);
        message = message.Replace("{itemCount}", itemsOwned.ToString());

        UIController.Instance.TextOutputUpdate(message);
    }

    public static void SendBlockedByObstacleMessage()
    {
        string message = _messageData.BlockedByObstacleMessage;
        UIController.Instance.TextOutputUpdate(message);
    }
}

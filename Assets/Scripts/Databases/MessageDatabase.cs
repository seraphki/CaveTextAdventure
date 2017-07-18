using UnityEngine;

public static class MessageDatabase
{
    private static MessageData _messageData;
    private static MessageData _messages
    {
        get
        {
            if (_messageData == null)
            {
                LoadMessageData();
            }
            return _messageData;
        }
    }

    public static bool LoadMessageData()
    {
        string raw = Helpers.LoadGameFile(InformationSet.MessageInformation);
        _messageData = JsonUtility.FromJson<MessageData>(raw);
        if (_messageData != null)
            return true;

        return false;
    }

    public static MessageData GetMessageData()
    {
        return _messages;
    }
}

public class MessageData : IEditable
{
    public string FullRoomEnterMessage;
    public string ShortRoomEnterMesssage;
    public string RoomIsEntranceMessage;
    public string RoomIsExitMessage;
    public string OneDirectionAvailableMessage;
    public string TwoDirectionsAvailableMessage;
    public string ThreeDirectionsAvailableMessage;
    public string FourDirectionsAvailableMessage;
    public string WordForPreviousLevel = "previous level";
    public string WordForNextLevel = "next level";
    public string WordForNorth = "north";
    public string WordForSouth = "south";
    public string WordForEast = "east";
    public string WordForWest = "west";
    public string NoRoomsInDirectionMessage;
    public string InventoryListHeaderMessage;
    public string InventoryListItemFormat;
    public string InventoryNoItemsMessage;
    public string ItemAddedMessage;
    public string ItemUsedMessage;
    public string BlockedByObstacleMessage;
}

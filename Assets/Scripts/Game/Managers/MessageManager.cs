using UnityEngine;
using System.Collections.Generic;

public static class MessageManager
{
    public static void SendStringMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            UIController.Instance.TextOutputUpdate("\n" + message);
        }
    }

    public static void SendEnterRoomMessage(Room roomInfo)
    {
        //Information about entrance direction
        string message = "\n";
        if (roomInfo.LastEnteredDirection == -1)
        {
            message += "You entered the " + roomInfo.StaticData.Name + ".\n";
        }
        else
        {
            message += "You went " + ((Direction)roomInfo.LastEnteredDirection).ToString() + " into the " + roomInfo.StaticData.Name + ".\n";
        }

        SendRoomMessage(roomInfo, message);
    }

	public static void SendRoomMessage(Room roomInfo, string currentString = null)
	{
        string message = currentString ?? "\n";

        //Message in room data
        if (!string.IsNullOrEmpty(roomInfo.StaticData.StartMessage))
        {
            message += roomInfo.StaticData.StartMessage + "\n";
        }
        

		List<int> connectingDirections = roomInfo.GetConnectingDirections();
		if (connectingDirections.Count > 1)
		{
			message += "There are exits to the ";
			for (int i = 0; i < connectingDirections.Count; i++)
			{
				message += ((Direction)connectingDirections[i]).ToString();

				if (i < connectingDirections.Count - 2)
				{
					message += ", ";
				}
				else if (i < connectingDirections.Count - 1)
				{
					message += " and ";
				}
				else
				{
					message += ".";
				}
			}
		}
		else
		{
			message += "There is an exit to the " + ((Direction)connectingDirections[0]).ToString() + ".";
		}

		if (roomInfo.Exit)
		{
			message += "\nThis is the exit to the next level.";
		}
		else if (roomInfo.Entrance)
		{
			message += "\nThis is the entrance to this level.";
		}

		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendEndOfCaveMessage()
	{
		string message = "\nYou've reached the end of the cave.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendLeftCaveMessage()
	{
		string message = "\nYou've left the cave.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendNoRoomMessage()
	{
		string message = "\nThere are no rooms in that direction.";
		UIController.Instance.TextOutputUpdate(message);
	}

    public static void SendInventoryMessage()
    {
        string[] items = GameController.InventoryManagerInst.GetOwnedItemIds();

        string message = "\n";
        if (items.Length <= 0)
        {
            message += "You have no items.";
        }
        else
        {
            message += "Items in your inventory: ";
            for (int i = 0; i < items.Length; i++)
            {
                int owned = GameController.InventoryManagerInst.GetItemAmountOwned(items[i]);
                string name = ItemDatabase.GetItemData(items[i]).Name;
                message += "\n" + name + ": " + owned;
            }
        }

        UIController.Instance.TextOutputUpdate(message);
    }

	public static void SendCoinsAddedMessage(int amount, int total)
	{
		string message = "\n" + amount + " coins were added to your inventory.\nYou now have " + total + " coins.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendItemAddedMessage(string name, int count)
	{
		string message = "\nA " + name + " was added to your inventory. You now have " + count + ".";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendItemUsedMessage(string itemName)
	{
		string message = "\nYou used a " + itemName + "!";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendBeginBattleMessage(string enemyName)
	{
		string message = "\nYou've begun a battle with " + enemyName + "!";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendBattleWonMessage(string enemyName, string prizeMessage)
	{
		string message = "\nYou defeated the " + enemyName + "!";
		if (!string.IsNullOrEmpty(prizeMessage))
		{
			message += "\n" + prizeMessage;
		}
		UIController.Instance.TextOutputUpdate(message);			
	}

	public static void SendEnemyAttackMessage(string enemyName, int damage, int remainingHealth)
	{
		CameraController.Instance.CameraShake();
		string message = "\nThe " + enemyName + " is attacking!\n";
		message += "You've been hit for " + damage + "!\n";
		message += "You have " + remainingHealth + " health remaining.";

		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendPlayerAttackedMessage(string enemyName, int damage, bool critical)
	{
		string message = "\nYou hit the " + enemyName + " for " + damage + "!\n";
		if (critical)
		{
			message += "The " + enemyName + " is severely wounded";
		}

		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendActionMessage(string action, string target, string enemy)
	{
		string targetName = string.IsNullOrEmpty(target) || Helpers.LooseCompare(target, target) ? enemy : enemy + " in the " + target;       
		string message = "\nYou " + action + " the " + targetName + "!";
		UIController.Instance.TextOutputUpdate(message);
	}

	public static void SendOutcomeMessage(string message)
	{
		UIController.Instance.TextOutputUpdate("\n" + message);
	}

	public static void SendPlayerHealedMessage(int amount, int newHealth)
	{
		string message = "You've been healed for " + amount + "!\n";
		message += "You now have " + newHealth + " health.";

		UIController.Instance.TextOutputUpdate(message);
	}
}

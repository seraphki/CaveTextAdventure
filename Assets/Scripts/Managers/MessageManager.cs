using UnityEngine;
using System.Collections.Generic;

public class MessageManager : MonoSingleton<MessageManager>
{
	public void SendRoomMessage(Room roomInfo)
	{
		UIController.Instance.NewLine();

		string message = "";
		if (roomInfo.LastEnteredDirection == -1)
		{
			message = "You entered the " + roomInfo.RoomName + ".\n";
		}
		else
		{
			message = "You went " + ((Direction)roomInfo.LastEnteredDirection).ToString() + "into the " + roomInfo.RoomName + ".\n";
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
			message += "There is an exit to the " + ((Direction)connectingDirections[0]).ToString();
		}

		if (roomInfo.Exit)
		{
			message += "\nThis is the exit to the next level.";
		}
		else if (roomInfo.Entrance)
		{
			message += "\nThis is the entrance to this level.";
		}
		else if (roomInfo.EnemyId > 0)
		{
			string enemyName = EnemyLibrary.Instance.GetEnemyData(roomInfo.EnemyId).Name;
			message += "\nThere is a " + enemyName + ".";
		}
		else if (roomInfo.RoomChest != null)
		{
			message += "\nThere is a chest.";
		}

		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendEndOfCaveMessage()
	{
		UIController.Instance.NewLine();
		string message = "You've reached the end of the cave.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendLeftCaveMessage()
	{
		UIController.Instance.NewLine();
		string message = "You've left the cave.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendNoRoomMessage()
	{
		UIController.Instance.NewLine();
		string message = "There are no rooms in that direction.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendCoinsAddedMessage(int amount, int total)
	{
		string message = amount + " coins were added to your inventory.\nYou now have " + total + " coins.";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendItemAddedMessage(string name, int count)
	{
		UIController.Instance.NewLine();
		string message = "A " + name + " was added to your inventory. You now have " + count + ".";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendItemUsedMessage(string itemName)
	{
		UIController.Instance.NewLine();
		string message = "You used a " + itemName + "!";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendBeginBattleMessage(string enemyName)
	{
		UIController.Instance.NewLine();
		string message = "You've begun a battle with " + enemyName + "!";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendBattleWonMessage(string enemyName, string prizeMessage)
	{
		UIController.Instance.NewLine();
		string message = "You defeated the " + enemyName + "!";
		if (!string.IsNullOrEmpty(prizeMessage))
		{
			message += "\n" + prizeMessage;
		}
		UIController.Instance.TextOutputUpdate(message);			
	}

	public void SendEnemyAttackMessage(string enemyName, int damage, int remainingHealth)
	{
		UIController.Instance.NewLine();
		CameraController.Instance.CameraShake();

		string message = "The " + enemyName + " is attacking!\n";
		message += "You've been hit for " + damage + "!\n";
		message += "You have " + remainingHealth + " health remaining.";

		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendPlayerAttackedMessage(string enemyName, int damage, bool critical)
	{
		UIController.Instance.NewLine();
		string message = "You hit the " + enemyName + " for " + damage + "!\n";
		if (critical)
		{
			message += "The " + enemyName + " is severely wounded";
		}

		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendActionMessage(string action, string target, string enemy)
	{
		string targetName = string.IsNullOrEmpty(target) || Helpers.StringLooseCompare(target, target) ? enemy : enemy + " in the " + target;

		UIController.Instance.NewLine();
		string message = "You " + action + " the " + target + "!";
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendOutcomeMessage(string message)
	{
		UIController.Instance.TextOutputUpdate(message);
	}

	public void SendPlayerHealedMessage(int amount, int newHealth)
	{
		string message = "You've been healed for " + amount + "!\n";
		message += "You now have " + newHealth + " health.";

		UIController.Instance.TextOutputUpdate(message);
	}
}

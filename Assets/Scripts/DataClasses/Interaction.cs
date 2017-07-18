using UnityEngine;
using System;

[Serializable]
public class Interaction : IEditable
{
    public string Action;
    public string Target;
    public bool RemoveUponCompletion;
    public Outcome InteractionOutcome;

    public void ExecuteInteractionOutcome()
    {
        if (InteractionOutcome != null)
        {
            InteractionOutcome.ExecuteOutcome();
            if (RemoveUponCompletion)
            {
                InteractionOutcome = null;
            }
        }
    }

    public Interaction()
    {
        InteractionOutcome = new Outcome();
    }
}

[Serializable]
public class Outcome : IEditable
{
    public string ItemIdToAdd;
    public string ItemIdToRemove;
    public int HealthDifference;
    public int Damage;
    public string Message;

    public void ExecuteOutcome()
    {
        //Message
        if (!string.IsNullOrEmpty(Message))
        {
            MessageManager.SendStringMessage(Message);
        }

        //Add Items
        if (!string.IsNullOrEmpty(ItemIdToAdd))
        {
            GameController.InventoryManagerInst.AddItem(ItemIdToAdd, 1);
        }

        //Remove Items
        if (!string.IsNullOrEmpty(ItemIdToRemove))
        {
            GameController.InventoryManagerInst.RemoveItem(ItemIdToRemove);
        }

        //Health Difference
        if (HealthDifference != 0)
        {
            PlayerController.Instance.ModifyHealth(HealthDifference);
        }

        if (Damage != 0)
        {
            ObstacleController.Instance.DamageObstacle(Damage);
        }
    }
}

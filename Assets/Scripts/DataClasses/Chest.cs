using UnityEngine;

public class Chest
{
    public int ItemId = 0;
    public int Coins = 0;

    private const float _coinLevelMod = 10;

    public Chest(int level)
    {
        if (Random.Range(0f, 1f) < 0.5f)
        {
            ItemId = CaveManager.Instance.GetRandomLevelItemId(level);
        }

        if (ItemId == 0)
        {
            Coins = Random.Range(1, 10 * (level + 1));
        }
    }

    public void OpenChest()
    {
        UIController.Instance.NewLine();
        if (ItemId > 0)
        {
            InventoryManager.Instance.AddItem(ItemId, 1);
        }
        if (Coins > 0)
        {
            InventoryManager.Instance.AddCoins(Coins);
        }

        UIController.Instance.DisableInteractionButtons();
        CaveManager.Instance.GetCurrentRoom().RoomChest = null;
        CaveManager.Instance.GetCurrentRoom().Look();
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager
{
    private Dictionary<string, int> _itemsOwned;
    private int _coins = 0;

    public InventoryManager()
    {
        _itemsOwned = new Dictionary<string, int>();
        LoadItems();
    }
    
    #region PublicMethods

	public string[] GetOwnedItemIds()
	{
		return _itemsOwned.Keys.ToArray();
	}

	public int GetItemAmountOwned(string itemId)
	{
		return _itemsOwned[itemId];
	}

    public void AddItem(string itemId, int count = 1)
    {
        Debug.Log("Adding item " + itemId);

        if (_itemsOwned.ContainsKey(itemId))
            _itemsOwned[itemId] += count;
		else
			_itemsOwned.Add(itemId, count);
        
		ItemData item = ItemDatabase.GetItemData(itemId);
		MessageManager.SendItemAddedMessage(item.Name, _itemsOwned[itemId]);
	}

    public void RemoveItem(string itemId)
    {
        if (_itemsOwned.ContainsKey(itemId) && _itemsOwned[itemId] > 0)
        {
            _itemsOwned[itemId] -= 1;
        }
    }

    public void UseItem(string itemId)
    {
        if (_itemsOwned.ContainsKey(itemId))
        {
    //        if (_itemsOwned[itemId] >= 0)
    //        {
				//ItemData data = ItemDatabase.GetItemData(itemId);
				//MessageManager.SendItemUsedMessage(data.Name);
				
				//if (data.HealthRestored != 0)
				//{
				//	PlayerController.Instance.ModifyHealth(data.HealthRestored);
				//}

				//_itemsOwned[itemId] -= 1;
    //        }
        }
        else
        {
            Debug.LogWarning("ITEM ID NOT FOUND: " + itemId);
        }
    }

    public void AddCoins(int coins)
    {
        _coins += coins;
		MessageManager.SendCoinsAddedMessage(coins, _coins);
    }

    public void RemoveCoins(int coins)
    {
        _coins = Mathf.Max(0, _coins - coins);
    }

    #endregion

    #region Private Methods

    private void LoadItems()
    {
        //Load items from save file
        //Load coin count from save file
    }

    #endregion
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private Dictionary<int, int> _itemsOwned;

    private int _coins = 0;

	void Awake()
	{
		_itemsOwned = new Dictionary<int, int>();
	}

    void Start()
    {
        LoadItems();
    }
    
    #region PublicMethods

	public int[] GetOwnedItemIds()
	{
		return _itemsOwned.Keys.ToArray();
	}

	public int GetItemAmountOwned(int itemId)
	{
		return _itemsOwned[itemId];
	}

    public void AddItem(int itemId, int count = 1)
    {
        if (_itemsOwned.ContainsKey(itemId))
            _itemsOwned[itemId] += count;
		else
			_itemsOwned.Add(itemId, count);
        
		ItemData item = ItemLibrary.Instance.GetItemData(itemId);
		MessageManager.Instance.SendItemAddedMessage(item.Name, _itemsOwned[itemId]);
	}

    public void UseItem(int itemId)
    {
        if (_itemsOwned.ContainsKey(itemId))
        {
            if (_itemsOwned[itemId] >= 0)
            {
				ItemData data = ItemLibrary.Instance.GetItemData(itemId);
				MessageManager.Instance.SendItemUsedMessage(data.Name);
				
				if (data.HealthRestored != 0)
				{
					PlayerController.Instance.Heal(data.HealthRestored);
				}

				_itemsOwned[itemId] -= 1;
            }
        }
        else
        {
            Debug.LogWarning("ITEM ID NOT FOUND: " + itemId);
        }
    }

    public void AddCoins(int coins)
    {
        _coins += coins;
		MessageManager.Instance.SendCoinsAddedMessage(coins, _coins);
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

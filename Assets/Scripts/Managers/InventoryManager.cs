using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private Dictionary<int, int> _itemsOwned;

    private int _coins = 0;

    void Start()
    {
        PopulateItemList();
        LoadItems();
    }
    
    #region PublicMethods

    public void AddItem(int itemId, int count)
    {
        if (_itemsOwned.ContainsKey(itemId))
        {
            _itemsOwned[itemId] += count;
            ItemData item = ItemLibrary.Instance.GetItemData(itemId);
            UIController.Instance.TextOutputUpdate("A " + item.Name + " was added to your inventory.");
        }
        else
        {
            Debug.LogWarning("ITEM ID NOT FOUND: " + itemId);
        }
    }

    public void UseItem(int itemId)
    {
        if (_itemsOwned.ContainsKey(itemId))
        {
            if (_itemsOwned[itemId] >= 0)
            {
                //Use Item
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
        string text = coins + " coins were added to your inventory.\nYou now have " + _coins + " coins.";
        UIController.Instance.TextOutputUpdate(text);
    }

    public void RemoveCoins(int coins)
    {
        _coins = Mathf.Max(0, _coins - coins);
    }

    #endregion

    #region Private Methods

    private void PopulateItemList()
    {
        _itemsOwned = new Dictionary<int, int>();
        int[] itemList = ItemLibrary.Instance.GetAllItemIds();
        for (int i = 0; i < itemList.Length; i++)
        {
            _itemsOwned.Add(itemList[i], 0);
        }
    }

    private void LoadItems()
    {
        //Load items from save file
        //Load coin count from save file
    }

    #endregion
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ItemDatabase
{
    public static ItemArray GetItemArray()
    {
        string raw = Helpers.LoadGameFile(InformationSet.ItemInformation);
        return JsonUtility.FromJson<ItemArray>(raw);
    }

    private static Dictionary<string, ItemData> _itemsData;
    private static Dictionary<string, ItemData> _items
    {
        get
        {
            if (_itemsData == null)
            {
                LoadItemData();
            }
            return _itemsData;
        }
    }

    public static bool LoadItemData()
    {
        _itemsData = new Dictionary<string, ItemData>();

        ItemArray itemArray = GetItemArray();
        if (itemArray != null)
        {
            for (int i = 0; i < itemArray.Items.Length; i++)
            {
                ItemData item = itemArray.Items[i];
                _items.Add(item.ItemId, item);
            }
            return true;
        }

        return false;
    }

    public static int ItemCount
    {
        get { return _items.Count; }
    }

    public static string[] GetAllItemIds()
    {
        return _items.Keys.ToArray();
    }

    //public static string[] GetItemIdsByLevel(int level)
    //{
    //    string[] levelItems = _items.Where(i => i.Value.RewardLvl <= level).Select(k => k.Key).ToArray();
    //    return levelItems;
    //}

    public static ItemData GetItemData(string itemId)
    {
        return _items[itemId];
    }
}

[System.Serializable]
public class ItemData : IEditable
{
    public string ItemId;
    public string Name;
    public Outcome UseOutcome;
}

public class ItemArray : IEditable
{
    public ItemData[] Items;
}

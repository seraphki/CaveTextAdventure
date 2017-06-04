using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemLibrary
{
    public static ItemLibrary Instance;

    private Dictionary<int, ItemData> _items;
    private const string _path = "Data/Items";

    public int ItemCount
    {
        get { return _items.Count; }
    }

    public ItemLibrary()
    {
        Instance = this;

        _items = new Dictionary<int, ItemData>();
        ItemData[] items = Resources.LoadAll<ItemData>(_path);
        for (int i = 0; i < items.Length; i++)
        {
            _items.Add(items[i].ItemId, items[i]);
        }
        Debug.Log("Items Loaded: " + _items.Count);
    }

    public int[] GetAllItemIds()
    {
        return _items.Keys.ToArray();
    }

    public int[] GetItemIdsByLevel(int level)
    {
        int[] levelItems = _items.Where(i => i.Value.RewardLvl <= level).Select(k => k.Key).ToArray();
        return levelItems;
    }

    public ItemData GetItemData(int itemId)
    {
        return _items[itemId];
    }
}

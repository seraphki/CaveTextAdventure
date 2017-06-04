using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public int ItemId;
    public int RewardLvl;
    public string Name;
    public Sprite Icon;
    public int HealthRestored;
}

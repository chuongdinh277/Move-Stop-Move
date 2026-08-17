using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccessoryData", menuName = "ScriptableObjects/AccessoryData", order = 3)]
public class AccessoryData : ScriptableObject
{
    public List<AccessoryItemData> accessoryList;

    public AccessoryItemData GetAccessory(AccessoryType type)
    {
        foreach (AccessoryItemData item in accessoryList)
        {
            if (item.accessoryType == type)
            {
                return item;
            }
        }
        return null;
    }
}
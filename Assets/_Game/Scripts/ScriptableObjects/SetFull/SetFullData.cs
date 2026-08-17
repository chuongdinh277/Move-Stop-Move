using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetFullData", menuName = "ScriptableObjects/SetFullData", order = 1)]
public class SetFullData : ScriptableObject
{
    public List<SetFullItemData> setFullList;

    public SetFullItemData GetSetFull(SetFullItemType type)
    {
        foreach (var item in setFullList)
        {
            if (item.setFullType == type) return item;
        }
        return null;
    }
}
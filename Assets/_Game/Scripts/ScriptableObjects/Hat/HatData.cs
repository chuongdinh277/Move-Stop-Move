using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "HatData", menuName = "ScriptableObjects/HatData", order = 1)]
public class HatData : ScriptableObject
{
    public List<HatItemData> hatList;

    public HatItemData GetHat(HatType type)
    {
        foreach (HatItemData item in hatList)
        {
            if (item.hatType == type)
            {
                return item;
            }
        }

        return null;
    }
}

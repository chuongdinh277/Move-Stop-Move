using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "PantData", menuName = "ScriptableObjects/PantData", order = 1)]
public class PantData : ScriptableObject
{
    public List<PantItemData> pantList;

    public PantItemData GetPant(PantType type)
    {
        foreach (PantItemData item in pantList)
        {
            if (item.pantType == type)
            {
                return item;
            }
        }

        return null;
    }
}

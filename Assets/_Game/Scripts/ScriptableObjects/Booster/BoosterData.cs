using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterData", menuName = "ScriptableObjects/BoosterData")]
public class BoosterData : ScriptableObject
{
    public List<BoosterItemData> boosterList;

    public BoosterItemData GetBooster(BoosterType type)
    {
        for (int i = 0; i < boosterList.Count; i++)
        {
            if (boosterList[i].boosterType == type)
            {
                return boosterList[i];
            }
        }
        return null;
    }
}
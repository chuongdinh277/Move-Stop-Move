using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterLevelData", menuName = "ScriptableObjects/CharacterLevelData", order = 1)]
public class CharacterLevelData : ScriptableObject
{
    public List<CharacterLevelItemData> levelList;

    public CharacterLevelItemData GetLevelData(int level)
    {
        int index = level - 1;
        if (levelList == null || levelList.Count == 0) return null;
        
        if (index >= levelList.Count)
        {
            return levelList[levelList.Count - 1]; 
        }

        return levelList[index];
    }
}

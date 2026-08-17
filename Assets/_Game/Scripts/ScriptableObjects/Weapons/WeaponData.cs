using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public List<WeaponItemData> weaponList;

    public Weapon GetWeaponPrefab(WeaponType type)
    {
        foreach (WeaponItemData item in weaponList)
        {
            if (item.weaponType == type)
            {
                return item.weaponPrefab;
            }
        }

        return null;
    }
}

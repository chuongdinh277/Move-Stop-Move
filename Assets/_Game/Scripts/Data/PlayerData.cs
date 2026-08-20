using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int gold;

    public int weaponEquipID;
    public int hatEquipID;
    public int pantEquipID;
    public int accessoryEquipID;
    public int setFullEquipID;

    public List<int> weaponShopState;
    public List<int> hatShopState;
    public List<int> pantShopState;
    public List<int> accessoryShopState;
    public List<int> setFullShopState;
}

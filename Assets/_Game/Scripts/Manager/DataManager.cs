using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
public class DataManager : Singleton<DataManager>
{
    private PlayerData data;

    private const string DATA_KEY = "PlayerData";

    private void Awake()
    {
        LoadData();  
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(DATA_KEY))
        {
            string json = PlayerPrefs.GetString(DATA_KEY);
            data = JsonUtility.FromJson<PlayerData>(json);
        }

        else
        {
            OnInitData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(DATA_KEY, json);
        PlayerPrefs.Save();
    }

    private void OnInitData()
    {
        data = new PlayerData();

        data.level = 1;
        data.gold = 0;

        data.weaponEquipID = (int)WeaponType.Hammer;
        data.hatEquipID = 0;
        data.pantEquipID = 0;
        data.accessoryEquipID = 0;
        data.setFullEquipID = 0;

        data.weaponShopState = new List<int>(new int[30]);
        data.hatShopState = new List<int>(new int[30]);
        data.pantShopState = new List<int>(new int[30]);
        data.accessoryShopState = new List<int>(new int[30]);
        data.setFullShopState = new List<int>(new int[30]);

        SetWeaponState(WeaponType.Hammer, 1);

        SaveData();

    }
    public WeaponType currentWeapon => (WeaponType)data.weaponEquipID;
    public void SetCurrentWeapon(WeaponType type) { data.weaponEquipID = (int)type; SaveData(); }
    public HatType currentHat => (HatType)data.hatEquipID;
    public void SetCurrentHat(HatType type) { data.hatEquipID = (int)type; SaveData(); }
    public PantType currentPant => (PantType)data.pantEquipID;
    public void SetCurrentPant(PantType type) { data.pantEquipID = (int)type; SaveData(); }
    public AccessoryType currentAccessory => (AccessoryType)data.accessoryEquipID;
    public void SetCurrentAccessory(AccessoryType type) { data.accessoryEquipID = (int)type; SaveData(); }
    public SetFullItemType currentSetFull => (SetFullItemType)data.setFullEquipID;
    public void SetCurrentSetFull(SetFullItemType type) { data.setFullEquipID = (int)type; SaveData(); }

    public int GetGold()
    {
        return data.gold;
    }

    public void AddGold(int amount)
    {
        data.gold += amount;
        SaveData();
    }


    public void SetWeaponState(WeaponType type, int state)
    {
        int id = (int)type;
        if (id < data.weaponShopState.Count) 
        {
            data.weaponShopState[id] = state;
            SaveData();
        }
    }

    public int GetWeaponState(WeaponType type)
    {
        int id = (int)type;
        if (id < data.weaponShopState.Count) return data.weaponShopState[id];
        return 0;
    }

    public void SetHatState(HatType type, int state)
    {
        int id = (int)type;
        if (id < data.hatShopState.Count) 
        {
            data.hatShopState[id] = state;
            SaveData();
        }
    }

    public int GetHatState(HatType type)
    {
        int id = (int)type;
        if (id < data.hatShopState.Count) return data.hatShopState[id];
        return 0;
    }

    public void SetPantState(PantType type, int state)
    {
        int id = (int)type;
        if (id < data.pantShopState.Count) 
        {
            data.pantShopState[id] = state;
            SaveData();
        }
    }

    public int GetPantState(PantType type)
    {
        int id = (int)type;
        if (id < data.pantShopState.Count) return data.pantShopState[id];
        return 0;
    }

    public void SetAccessoryState(AccessoryType type, int state)
    {
        int id = (int)type;
        if (id < data.accessoryShopState.Count) 
        {
            data.accessoryShopState[id] = state;
            SaveData();
        }
    }

    public int GetAccessoryState(AccessoryType type)
    {
        int id = (int)type;
        if (id < data.accessoryShopState.Count) return data.accessoryShopState[id];
        return 0;
    }
    public void SetSetFullState(SetFullItemType type, int state)
    {
        int id = (int)type;
        if (id < data.setFullShopState.Count) 
        { 
            data.setFullShopState[id] = state; 
            SaveData(); 
        }
    }
    public int GetSetFullState(SetFullItemType type)
    {
        int id = (int)type;
        if (id < data.setFullShopState.Count) return data.setFullShopState[id];
        return 0;
    }
}

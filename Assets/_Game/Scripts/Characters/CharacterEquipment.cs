using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment
{
    private CharacterBase character;
    private const string HAT = "Hat_";
    private const string WEAPON = "Weapon_";
    private const string SHIELD = "Shield_";
    private const string WING = "Wing_";
    private Dictionary<string, GameObject> wardrobe = new Dictionary<string, GameObject>();

    public CharacterEquipment(CharacterBase character)
    {
        this.character = character;
    }

    public void ChangeWeapon(Weapon weaponPrefab)
    {
        if (character.GetCurrentWeapon() != null) character.GetCurrentWeapon().gameObject.SetActive(false);

        if (weaponPrefab != null && character.GetWeaponHolder() != null)
        {
            string itemName = WEAPON + weaponPrefab.name;
            if (wardrobe.ContainsKey(itemName))
            {
                character.SetCurrentWeapon(wardrobe[itemName].GetComponent<Weapon>());
                character.GetCurrentWeapon().gameObject.SetActive(true);
            }
            else
            {
                Weapon newWeapon = Object.Instantiate(weaponPrefab, character.GetWeaponHolder(), false);
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = Quaternion.identity;
                wardrobe.Add(itemName, newWeapon.gameObject);
                character.SetCurrentWeapon(newWeapon);
            }
        }
    }

    public void ChangeHat(GameObject hatPrefab)
    {
        if (character.GetCurrentHat() != null) character.GetCurrentHat().SetActive(false);

        if (hatPrefab != null && character.GetHeadHolder() != null)
        {
            string itemName = HAT + hatPrefab.name;
            if (wardrobe.ContainsKey(itemName))
            {
                character.SetCurrentHat(wardrobe[itemName]);
                character.GetCurrentHat().SetActive(true);
            }
            else
            {
                GameObject newHat = Object.Instantiate(hatPrefab, character.GetHeadHolder(), false);
                newHat.transform.localPosition = Vector3.zero;
                newHat.transform.localRotation = Quaternion.identity;
                wardrobe.Add(itemName, newHat);
                character.SetCurrentHat(newHat);
            }
        }
    }

    public void ChangeShield(GameObject shieldPrefab)
    {
        if (character.GetCurrentShield() != null) character.GetCurrentShield().SetActive(false);
        
        character.SetIsHoldingShield(shieldPrefab != null);

        if (shieldPrefab != null && character.GetLeftHandHolder() != null)
        {
            string itemName = SHIELD + shieldPrefab.name;
            if (wardrobe.ContainsKey(itemName))
            {
                character.SetCurrentShield(wardrobe[itemName]);
                character.GetCurrentShield().SetActive(true);
            }
            else
            {
                GameObject newShield = Object.Instantiate(shieldPrefab, character.GetLeftHandHolder(), false);
                newShield.transform.localPosition = Vector3.zero;
                newShield.transform.localRotation = Quaternion.identity;
                wardrobe.Add(itemName, newShield);
                character.SetCurrentShield(newShield);
            }
        }
    }

    public void ChangeWing(GameObject wingPrefab)
    {
        if (character.GetCurrentWing() != null) character.GetCurrentWing().SetActive(false);

        if (wingPrefab != null && character.GetBackHolder() != null)
        {
            string itemName = WING + wingPrefab.name;
            if (wardrobe.ContainsKey(itemName))
            {
                character.SetCurrentWing(wardrobe[itemName]);
                character.GetCurrentWing().SetActive(true);
            }
            else
            {
                GameObject newWing = Object.Instantiate(wingPrefab, character.GetBackHolder(), false);
                newWing.transform.localPosition = Vector3.zero;
                newWing.transform.localRotation = Quaternion.identity;
                wardrobe.Add(itemName, newWing);
                character.SetCurrentWing(newWing);
            }
        }
    }

    public void ChangeTail(GameObject tailPrefab)
    {
        if (character.GetCurrentTail() != null) character.GetCurrentTail().SetActive(false);

        if (tailPrefab != null && character.GetTailHolder() != null)
        {
            string itemName = "Tail_" + tailPrefab.name;
            if (wardrobe.ContainsKey(itemName))
            {
                character.SetCurrentTail(wardrobe[itemName]);
                character.GetCurrentTail().SetActive(true);
            }
            else
            {
                GameObject newTail = Object.Instantiate(tailPrefab, character.GetTailHolder(), false);
                newTail.transform.localPosition = Vector3.zero;
                newTail.transform.localRotation = Quaternion.identity;
                wardrobe.Add(itemName, newTail);
                character.SetCurrentTail(newTail);
            }
        }
    }

    public void ChangePant(Material pantMat)
    {
        if (pantMat != null && character.GetPantMeshRenderer() != null)
        {
            character.GetPantMeshRenderer().material = pantMat;
        }
    }

    public void ChangeColor(Material colorMat)
    {
        if (colorMat != null && character.GetBodyMeshRenderer() != null)
        {
            character.GetBodyMeshRenderer().material = colorMat;
        }
    }

    public void ChangeSetFull(SetFullItemData setData)
    {
        if (setData == null) return;
        
        ChangeHat(setData.hatPrefab);
        ChangePant(setData.pantMat);
        ChangeShield(setData.leftHandPrefab);
        ChangeWing(setData.accessoryPrefab);
        ChangeTail(setData.tailPrefab);
    }
}
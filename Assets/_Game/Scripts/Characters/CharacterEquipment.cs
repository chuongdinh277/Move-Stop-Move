using UnityEngine;

public class CharacterEquipment
{
    private CharacterBase character;

    public CharacterEquipment(CharacterBase character)
    {
        this.character = character;
    }

    public void ChangeWeapon(Weapon weaponPrefab)
    {
        if (character.GetCurrentWeapon() != null)
        {
            Object.Destroy(character.GetCurrentWeapon().gameObject);
        }

        if (weaponPrefab != null && character.GetWeaponHolder() != null)
        {
            character.SetCurrentWeapon(Object.Instantiate(weaponPrefab, character.GetWeaponHolder()));
            character.GetCurrentWeapon().transform.localPosition = Vector3.zero;
            character.GetCurrentWeapon().transform.localRotation = Quaternion.identity;
        }
    }

    public void ChangeHat(GameObject hatPrefab)
    {
        if (character.GetCurrentHat() != null) Object.Destroy(character.GetCurrentHat());

        if (hatPrefab != null && character.GetHeadHolder() != null)
        {
            character.SetCurrentHat(Object.Instantiate(hatPrefab, character.GetHeadHolder()));
            character.GetCurrentHat().transform.localPosition = Vector3.zero;
            character.GetCurrentHat().transform.localRotation = Quaternion.identity;
        }
    }

    public void ChangePant(Material pantMat)
    {
        if (pantMat != null && character.GetPantMeshRenderer() != null)
        {
            character.GetPantMeshRenderer().material = pantMat;
        }
    }

    public void ChangeShield(GameObject shieldPrefab)
    {
        if (character.GetCurrentShield() != null) Object.Destroy(character.GetCurrentShield());
        
        character.SetIsHoldingShield(shieldPrefab != null);

        if (shieldPrefab != null && character.GetLeftHandHolder() != null)
        {
            character.SetCurrentShield(Object.Instantiate(shieldPrefab, character.GetLeftHandHolder()));  
            character.GetCurrentShield().transform.localPosition = Vector3.zero;
            character.GetCurrentShield().transform.localRotation = Quaternion.identity;
        }
    }

    public void ChangeWing(GameObject wingPrefab)
    {
        if (character.GetCurrentWing() != null) Object.Destroy(character.GetCurrentWing());

        if (wingPrefab != null && character.GetBackHolder() != null)
        {
            character.SetCurrentWing(Object.Instantiate(wingPrefab, character.GetBackHolder()));
            character.GetCurrentWing().transform.localPosition = Vector3.zero;
            character.GetCurrentWing().transform.localRotation = Quaternion.identity;
        }
    }

    public void ChangeTail(GameObject tailPrefab)
    {
        if (character.GetCurrentTail() != null) Object.Destroy(character.GetCurrentTail());

        if (tailPrefab != null && character.GetTailHolder() != null)
        {
            character.SetCurrentTail(Object.Instantiate(tailPrefab, character.GetTailHolder()));
            character.GetCurrentTail().transform.localPosition = Vector3.zero;
            character.GetCurrentTail().transform.localRotation = Quaternion.identity;
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

    public void ChangeColor(Material colorMat)
    {
        if (colorMat != null && character.GetBodyMeshRenderer() != null)
        {
            character.GetBodyMeshRenderer().material = colorMat;
        }
    }
}

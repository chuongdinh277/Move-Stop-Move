using UnityEngine;

public class CharacterCombat
{
    private CharacterBase character;

    public CharacterCombat(CharacterBase character)
    {
        this.character = character;
    }

    public void Attack()
    {
        if (CanAttack())
        {
            ExecuteAttackFlow();
        }
    }

    public CharacterBase GetValidTarget()
    {
        character.GetTargetsInRange().RemoveAll(t => t == null || t.GetIsDead());
        
        float attackRangeSqr = (character.GetAttackRange() * character.GetSize()) * (character.GetAttackRange() * character.GetSize());
        
        foreach (CharacterBase t in character.GetTargetsInRange())
        {
            if ((t.TF.position - character.TF.position).sqrMagnitude <= attackRangeSqr)
            {
                return t;
            }
        }
        return null;
    }

    public bool CanAttack()
    {
        if (character.GetIsAttacking() || character.GetIsDead()) return false;
        return GetValidTarget() != null;
    }

    public void ExecuteAttackFlow()
    {
        CharacterBase target = GetValidTarget();
        if (target == null) return;

        character.SetIsAttacking(true);
        RotateToTarget(target);
        character.ChangeAnim(GameConfig.ANIM_ATTACK);
        
        character.Invoke(nameof(character.SpawnWeaponBullet), character.GetThrowDelay());
        character.Invoke(nameof(character.ResetAttackState), character.GetAttackSpeed());
    }

    public void RotateToTarget(CharacterBase target)
    {
        if (target == null) return;
        
        Vector3 directionToTarget = (target.TF.position - character.TF.position).normalized;
        directionToTarget.y = 0;
        
        if (directionToTarget != Vector3.zero)
        {
            character.TF.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    public void SpawnWeaponBullet()
    {
        if (character.GetCurrentWeapon() == null || character.GetIsDead()) return;

        Vector3 throwDir = character.TF.forward;
        CharacterBase target = GetValidTarget();

        if (target != null)
        {
            throwDir = (target.TF.position - character.TF.position).normalized;
            throwDir.y = 0;
            character.TF.rotation = Quaternion.LookRotation(throwDir);
        }
        
        character.GetCurrentWeapon().Throw(character, target.TF.position, true);
    }

    public void ResetAttackState()
    {
        character.SetIsAttacking(false);
        character.SetCurrentAnimName("");
        if (character.GetCurrentWeapon() != null)
        {
            character.GetCurrentWeapon().gameObject.SetActive(true);
        }
    }
    
    public void CancelAttack()
    {
        if (character.GetIsAttacking()) 
        {
            character.CancelInvoke(nameof(character.SpawnWeaponBullet));
            character.CancelInvoke(nameof(character.ResetAttackState));
            ResetAttackState();
        }
    }

    public void PlayBloodEffect()
    {
        if (character.GetBloodPrefab() != null && character.GetBodyMeshRenderer() != null)
        {
            ParticleSystemRenderer bloodEffect = Object.Instantiate(character.GetBloodPrefab(), character.TF.position, Quaternion.identity);
            bloodEffect.material = character.GetBodyMeshRenderer().material;

            bloodEffect.transform.localScale = character.TF.localScale * 2f;
            Object.Destroy(bloodEffect.gameObject, 2f);
        }
    }
}

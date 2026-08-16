using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [Header("Components")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected Collider charCollider;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Weapon currentWeapon;

    [Header("Status")]
    [SerializeField] protected float moveSpeed = GameConfig.SPEED;
    [SerializeField] protected float attackRange = GameConfig.ATTACK_RANGE;
    [SerializeField] protected float attackSpeed = GameConfig.ATTACK_SPEED;
    [SerializeField] protected float throwDelay = 0.4f;

    public int level { get; protected set; } = 1;
    public float size { get; protected set; } = 1f;
    public bool IsDead { get; protected set; } = false;
    public bool isAttacking { get; protected set; } = false;
    public float AttackRange => attackRange;

    protected string currentAnimName;
    protected List<Character> targetsInRange = new List<Character>();

    public virtual void OnInit()
    {
        IsDead = false;
        isAttacking = false;
        targetsInRange.Clear();
        SetSize(1f);
        Cache.Ins.RegisterCharacter(charCollider, this);
        if (currentWeapon != null && !currentWeapon.gameObject.scene.IsValid())
        {
            Weapon prefab = currentWeapon;
            currentWeapon = null;
            ChangeWeapon(prefab);
        }
    }

    protected virtual void Start()
    {
        OnInit();
    }

    public virtual void OnDespawn()
    {
    }
    
    protected virtual void Move(Vector3 direction)
    {
    }

    public virtual void SetSize(float newSize)
    {
        size = newSize;
        TF.localScale = Vector3.one * size;
    }

    public virtual void AddSize(float amount = 0.1f)
    {
        SetSize(size + amount);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName == animName) return;

        anim.ResetTrigger(currentAnimName);
        currentAnimName = animName;
        anim.SetTrigger(currentAnimName);
    }

    public void AddTarget(Character target)
    {
        if (target != null && !targetsInRange.Contains(target))
        {
            targetsInRange.Add(target);
        }
    }

    public void RemoveTarget(Character target)
    {
        if (targetsInRange.Contains(target))
        {
            targetsInRange.Remove(target);
        }
    }

    public virtual void Attack()
    {
        if (CanAttack())
        {
            ExecuteAttackFlow();
        }
    }

    protected Character GetValidTarget()
    {
        targetsInRange.RemoveAll(t => t == null || t.IsDead);
        
        float attackRangeSqr = (attackRange * size) * (attackRange * size);
        
        foreach (Character t in targetsInRange)
        {
            if ((t.TF.position - TF.position).sqrMagnitude <= attackRangeSqr)
            {
                return t;
            }
        }
        return null;
    }

    protected bool CanAttack()
    {
        if (isAttacking || IsDead) return false;
        return GetValidTarget() != null;
    }

    protected void ExecuteAttackFlow()
    {
        Character target = GetValidTarget();
        if (target == null) return;

        isAttacking = true;
        RotateToTarget(target);
        ChangeAnim(GameConfig.ANIM_ATTACK);
        
        Invoke(nameof(SpawnWeaponBullet), throwDelay);
        Invoke(nameof(ResetAttackState), attackSpeed);
    }

    protected void RotateToTarget(Character target)
    {
        if (target == null) return;
        
        Vector3 directionToTarget = (target.TF.position - TF.position).normalized;
        directionToTarget.y = 0;
        
        if (directionToTarget != Vector3.zero)
        {
            TF.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    protected void SpawnWeaponBullet()
    {
        if (currentWeapon == null || IsDead) return;

        Vector3 throwDir = TF.forward;
        Character target = GetValidTarget();

        if (target != null)
        {
            throwDir = (target.TF.position - TF.position).normalized;
            throwDir.y = 0;
            TF.rotation = Quaternion.LookRotation(throwDir);
        }
        
        currentWeapon.Throw(this, throwDir);
    }

    protected void ResetAttackState()
    {
        isAttacking = false;
        currentAnimName = "";
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(true);
        }
    }
    public void CancelAttack()
    {
        if (isAttacking) 
        {
            CancelInvoke(nameof(SpawnWeaponBullet));
            CancelInvoke(nameof(ResetAttackState));
            ResetAttackState();
        }
    }
    public void ChangeWeapon(Weapon weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        if (weaponPrefab != null && weaponHolder != null)
        {
            currentWeapon = Instantiate(weaponPrefab, weaponHolder);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
        }
    }
}

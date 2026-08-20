using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : GameUnit
{
    [Header("Components")]
    [SerializeField] private Animator anim;
    [SerializeField] private Collider charCollider;
    [SerializeField] private Weapon currentWeapon;

    [Header("Equipment Holders")]
    [SerializeField] private Transform headHolder;
    [SerializeField] private Transform leftHandHolder;
    [SerializeField] private SkinnedMeshRenderer pantMeshRenderer;
    [SerializeField] private Transform backHolder;
    [SerializeField] private Transform tailHolder;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform leftArmBone;
    [SerializeField] private Vector3 shieldHoldAngles;
    [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;

    private bool isHoldingShield = false;
    private GameObject currentHat;
    private GameObject currentWing;
    private GameObject currentShield; 
    private GameObject currentTail;

    [Header("Status")]
    [SerializeField] private float moveSpeed = GameConfig.SPEED;
    [SerializeField] private float attackRange = GameConfig.ATTACK_RANGE;
    [SerializeField] private float attackSpeed = GameConfig.ATTACK_SPEED;
    [SerializeField] private float throwDelay = 0.4f;

    [Header("Leveling")]
    [SerializeField] private CharacterLevelData levelData;

    [Header("Effect")]
    [SerializeField] private ParticleSystemRenderer bloodPrefab;

    private int level = 1;
    private float size = 1f;
    private bool isDead = false;
    private bool isAttacking = false;
    private string currentAnimName;
    private List<CharacterBase> targetsInRange = new List<CharacterBase>();

    private CharacterCombat combat;
    private CharacterEquipment equipment;
    private CharacterLeveling leveling;

    public Animator GetAnim() { return anim; }
    public Collider GetCharCollider() { return charCollider; }
    public Transform GetHeadHolder() { return headHolder; }
    public Transform GetLeftHandHolder() { return leftHandHolder; }
    public Transform GetBackHolder() { return backHolder; }
    public Transform GetTailHolder() { return tailHolder; }
    public Transform GetWeaponHolder() { return weaponHolder; }
    public Transform GetLeftArmBone() { return leftArmBone; }
    public SkinnedMeshRenderer GetPantMeshRenderer() { return pantMeshRenderer; }
    public SkinnedMeshRenderer GetBodyMeshRenderer() { return bodyMeshRenderer; }
    public ParticleSystemRenderer GetBloodPrefab() { return bloodPrefab; }
    public CharacterLevelData GetLevelData() { return levelData; }
    
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetThrowDelay() { return throwDelay; }
    public float GetAttackSpeed() { return attackSpeed; }
    public float GetSize() { return size; }
    public List<CharacterBase> GetTargetsInRange() { return targetsInRange; }

    public Weapon GetCurrentWeapon() { return currentWeapon; }
    public void SetCurrentWeapon(Weapon weapon) { currentWeapon = weapon; }

    public bool GetIsHoldingShield() { return isHoldingShield; }
    public void SetIsHoldingShield(bool value) { isHoldingShield = value; }

    public GameObject GetCurrentHat() { return currentHat; }
    public void SetCurrentHat(GameObject hat) { currentHat = hat; }

    public GameObject GetCurrentWing() { return currentWing; }
    public void SetCurrentWing(GameObject wing) { currentWing = wing; }

    public GameObject GetCurrentShield() { return currentShield; }
    public void SetCurrentShield(GameObject shield) { currentShield = shield; }

    public GameObject GetCurrentTail() { return currentTail; }
    public void SetCurrentTail(GameObject tail) { currentTail = tail; }

    public float GetAttackRange() { return attackRange; }
    public void SetAttackRange(float range) { attackRange = range; }
    
    public int GetLevel() { return level; }
    public void SetLevel(int newLevel) { level = newLevel; }

    public bool GetIsDead() { return isDead; }
    public void SetIsDead(bool deadState) { isDead = deadState; }

    public bool GetIsAttacking() { return isAttacking; }
    public void SetIsAttacking(bool attackingState) { isAttacking = attackingState; }

    public string GetCurrentAnimName() { return currentAnimName; }
    public void SetCurrentAnimName(string animName) { currentAnimName = animName; }

    public int GetCurrentExp()
    {
        return leveling != null ? leveling.currentExp : 0;
    }

    protected virtual void Awake()
    {
        combat = new CharacterCombat(this);
        equipment = new CharacterEquipment(this);
        leveling = new CharacterLeveling(this);
    }

    public virtual void OnInit()
    {
        SetIsDead(false);
        SetIsAttacking(false);
        targetsInRange.Clear();
        SetSize(1f);
        SetLevel(1);
        leveling.RessetLevel();
        
        Cache.Ins.RegisterCharacter(charCollider, this);
        
        if (currentWeapon != null && !currentWeapon.gameObject.scene.IsValid())
        {
            Weapon prefab = currentWeapon;
            currentWeapon = null;
            ChangeWeapon(prefab);
        }
        
        if (IndicatorManager.Ins != null && !IndicatorManager.targets.Contains(this))
        {
            IndicatorManager.targets.Add(this);
        }
    }

    protected virtual void Start()
    {
        OnInit();
    }

    public virtual void OnDespawn()
    {
        if (IndicatorManager.Ins != null)
        {
            IndicatorManager.targets.Remove(this);
        }
    }

    protected virtual void LateUpdate()
    {
        if (isHoldingShield && !isDead && leftArmBone != null)
        {
            leftArmBone.localRotation = Quaternion.Euler(shieldHoldAngles);
        } 
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

    public void ChangeAnim(string animName)
    {
        if (currentAnimName == animName) return;

        if (GetIsDead() && animName != GameConfig.ANIM_DEAD) return;
        anim.ResetTrigger(currentAnimName);
        currentAnimName = animName;
        anim.SetTrigger(currentAnimName);
    }

    public void AddTarget(CharacterBase target)
    {
        if (target != null && !targetsInRange.Contains(target))
        {
            targetsInRange.Add(target);
        }
    }

    public void RemoveTarget(CharacterBase target)
    {
        if (targetsInRange.Contains(target))
        {
            targetsInRange.Remove(target);
        }
    }

    public virtual void OnHit()
    {
        if (isDead) return;
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        isDead = true;
        CancelAttack();

        ChangeAnim(GameConfig.ANIM_DEAD);
        combat.PlayBloodEffect();
        Invoke(nameof(OnDespawn), 2f);
    }

    public virtual void Attack() => combat.Attack();
    public bool CanAttack() => combat.CanAttack();
    public void CancelAttack() => combat.CancelAttack();
    public void ExecuteThrow() => combat.ExecuteThrow();
    public void ResetAttackState() => combat.ResetAttackState();

    public void ChangeWeapon(Weapon weaponPrefab) => equipment.ChangeWeapon(weaponPrefab);
    public void ChangeHat(GameObject hatPrefab) => equipment.ChangeHat(hatPrefab);
    public void ChangePant(Material pantMat) => equipment.ChangePant(pantMat);
    public void ChangeShield(GameObject shieldPrefab) => equipment.ChangeShield(shieldPrefab);
    public void ChangeWing(GameObject wingPrefab) => equipment.ChangeWing(wingPrefab);
    public void ChangeTail(GameObject tailPrefab) => equipment.ChangeTail(tailPrefab);
    public void ChangeSetFull(SetFullItemData setData) => equipment.ChangeSetFull(setData);
    public void ChangeColor(Material colorMat) => equipment.ChangeColor(colorMat);

    public void OnKillBot(int victimLevel) => leveling.AddExp(victimLevel);
}

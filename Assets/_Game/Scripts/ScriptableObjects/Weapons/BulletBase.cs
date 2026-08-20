using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletBase : GameUnit
{
    [Header("Bullet Starts")]
    [SerializeField] protected float SPEED = GameConfig.WEAPON_SPEED;
    [SerializeField] protected float timeToDestroy = GameConfig.WEAPON_TIME_DESTROY;

    protected CharacterBase attacker;
    protected float timer;
    protected Vector3 moveDirection;
    protected Vector3 baseScale;
    protected Vector3 startPos;
    protected virtual void Awake()
    {
        baseScale = transform.localScale;
    }
    protected virtual void Update()
    {
        transform.Translate(moveDirection * SPEED * Time.deltaTime, Space.World);
        HandleLifeTime();
    }
    protected void SetAttacker(CharacterBase character)
    {
        this.attacker = character;
    }
    protected void ResetLifeTime()
    {
        timer = timeToDestroy;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Cache.Ins.TryGetCharacter(other, out CharacterBase target))
        {
            if (IsValidTarget(target))
            {
                HitTarget(target);
            }
        }
    }
    public virtual void OnInit(CharacterBase attacker, Vector3 direction)
    {
        SetAttacker(attacker);
        ResetLifeTime();
        SetDirection(direction);
        moveDirection = direction.normalized;
        
        startPos = transform.position;
        transform.localScale = baseScale * attacker.GetSize(); 
    }

    protected void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
    }

    protected void HandleLifeTime()
    {
        timer -= Time.deltaTime;

        float distanceFlow = (transform.position - startPos).sqrMagnitude;

        float maxRange = attacker.GetAttackRange() * attacker.GetSize();

        float maxRangeSqr = maxRange * maxRange;

        if (timer <= 0 || distanceFlow >= maxRangeSqr)
        {
            OnDespawn();
        }
    }

    protected bool IsTimeOut()
    {
        return timer <= 0;
    }
    
    protected bool IsValidTarget(CharacterBase target)
    {
        return target != null && target != attacker && !target.GetIsDead();
    }

    protected virtual void HitTarget(CharacterBase target)
    {
        target.OnHit();

        if (attacker != null && target != null)
        {
            attacker.OnKillBot(target.GetLevel());
        }
        OnDespawn();
    }

    public virtual void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}


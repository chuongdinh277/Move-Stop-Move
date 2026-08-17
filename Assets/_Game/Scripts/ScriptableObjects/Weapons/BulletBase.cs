using System;
using UnityEngine;

public class BulletBase : GameUnit
{
    [Header("Bullet Starts")]
    [SerializeField] protected float SPEED = GameConfig.WEAPON_SPEED;
    [SerializeField] protected float timeToDestroy = GameConfig.WEAPON_TIME_DESTROY;

    protected Character attacker;
    protected float timer;
    protected Vector3 moveDirection;

    protected virtual void Update()
    {
        // Bay theo hướng cố định trong không gian World, không phụ thuộc vào góc xoay của model
        transform.Translate(moveDirection * SPEED * Time.deltaTime, Space.World);
        HandleLifeTime();
    }
    protected void SetAttacker(Character character)
    {
        this.attacker = character;
    }
    protected void ResetLifeTime()
    {
        timer = timeToDestroy;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Cache.Ins.TryGetCharacter(other, out Character target))
        {
            if (IsValidTarget(target))
            {
                HitTarget(target);
            }
        }
    }
    public virtual void OnInit(Character attacker, Vector3 direction)
    {
        SetAttacker(attacker);
        ResetLifeTime();
        SetDirection(direction);
        moveDirection = direction.normalized;
    }

    protected void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
    }

    protected void HandleLifeTime()
    {
        timer -= Time.deltaTime;
        if (IsTimeOut())
        {
            OnDespawn();
        }
    }

    protected bool IsTimeOut()
    {
        return timer <= 0;
    }
    
    protected bool IsValidTarget(Character target)
    {
        return target != null && target != attacker && !target.IsDead;
    }

    protected virtual void HitTarget(Character target)
    {
        target.OnHit();
        OnDespawn();
    }

    public virtual void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}

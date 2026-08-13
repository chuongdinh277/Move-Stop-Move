using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Animator anim;

    [Header("Status")]
    [SerializeField] protected float moveSpeed = GameConfig.SPEED;
    [SerializeField] protected float attackRange = GameConfig.ATTACK_RANGE;
    [SerializeField] protected float attackSpeed = GameConfig.ATTACK_SPEED;

    public int level {get; protected set;} = 1;
    public float size {get; protected set;} = 1f;
    public bool IsDead {get; protected set;} = false;

    protected string currentAnimName;
    protected List<Character> targetsInRange = new List<Character>();

    protected virtual void Move(Vector3 direction)
    {
        
    }
    public virtual void OnInit()
    {
        
    }
    public virtual void OnDespawn()
    {
        
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName == animName) return;

        anim.ResetTrigger(currentAnimName);

        currentAnimName = animName;
        
        anim.SetTrigger(currentAnimName);
    }
}

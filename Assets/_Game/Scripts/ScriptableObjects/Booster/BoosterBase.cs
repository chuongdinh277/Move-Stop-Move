using UnityEngine;

public abstract class BoosterBase
{
    protected CharacterBase character;
    protected float amount;
    protected float duration;
    protected float timer;

    public BoosterBase(CharacterBase character, float amount, float duration)
    {
        this.character = character;
        this.amount = amount;
        this.duration = duration;
        this.timer = duration;
    }

    public virtual void OnApply() { }
    public virtual void OnRemove() { }
    public virtual void OnUpdate(float deltaTime) 
    {
        if (timer > 0)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                OnRemove();
            }
        }
    }

    public bool IsFinished() => timer <= 0;
}
using UnityEngine;

public class RangeBooster : BoosterBase
{
    public RangeBooster(CharacterBase character, float amount, float duration) : base(character, amount, duration) { }

    public override void OnApply()
    {
        character.SetAttackRange(character.GetAttackRange() + amount);
    }

    public override void OnRemove()
    {
        character.SetAttackRange(character.GetAttackRange() - amount);
    }
}

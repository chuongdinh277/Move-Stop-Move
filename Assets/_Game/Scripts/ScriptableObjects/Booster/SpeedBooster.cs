public class SpeedBooster : BoosterBase
{
    public SpeedBooster(CharacterBase character, float amount, float duration) : base(character, amount, duration) { }

    public override void OnApply()
    {
        character.SetMoveSpeed(character.GetMoveSpeed() + amount);
    }

    public override void OnRemove()
    {
        character.SetMoveSpeed(character.GetMoveSpeed() - amount);
    }
}
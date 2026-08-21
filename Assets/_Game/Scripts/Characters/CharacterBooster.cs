using System.Collections.Generic;

public class CharacterBooster
{
    private CharacterBase character;
    
    private Dictionary<BoosterType, BoosterBase> activeBoosters = new Dictionary<BoosterType, BoosterBase>();

    public CharacterBooster(CharacterBase character)
    {
        this.character = character;
    }

    public void ApplyBooster(BoosterType type, float amount, float duration)
    {
        if (activeBoosters.ContainsKey(type))
        {
            activeBoosters[type].OnRemove();
            activeBoosters.Remove(type);
        }

        BoosterBase newBooster = null;
        switch (type)
        {
            case BoosterType.Speed: newBooster = new SpeedBooster(character, amount, duration); break;
            case BoosterType.AttackRange: newBooster = new RangeBooster(character, amount, duration); break;
            case BoosterType.Shield: newBooster = new ShieldBooster(character, amount, duration); break;
        }

        if (newBooster != null)
        {
            newBooster.OnApply();
            activeBoosters.Add(type, newBooster);
        }
    }

    public void UpdateBoosters(float deltaTime)
    {
        List<BoosterType> toRemove = new List<BoosterType>();
        
        foreach (var kvp in activeBoosters)
        {
            kvp.Value.OnUpdate(deltaTime);
            
            if (kvp.Value.IsFinished()) 
            {
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var type in toRemove)
        {
            activeBoosters.Remove(type);
        }
    }

    public bool HasShield()
    {
        return activeBoosters.ContainsKey(BoosterType.Shield);
    }

    public void ConsumeShield()
    {
        if (activeBoosters.ContainsKey(BoosterType.Shield))
        {
            activeBoosters[BoosterType.Shield].OnRemove();
            activeBoosters.Remove(BoosterType.Shield);
        }
    }
}
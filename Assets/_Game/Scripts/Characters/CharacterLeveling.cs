using UnityEngine;

public class CharacterLeveling
{
    private CharacterBase character;
    public int currentExp { get; private set; }

    public CharacterLeveling(CharacterBase character)
    {
        this.character = character;
        this.currentExp = 0;
    }

    public void AddExp(int amount)
    {
        if (character.GetLevelData() == null) return;
        currentExp += amount;
        CheckLevelUp();
    }

    // public void AddExpAndRewards()
    // {
    //     if (character.GetLevelData() == null) return;
        
    //     CharacterLevelItemData currentLevelData = character.GetLevelData().GetLevelData(character.GetLevel());
    //     if (currentLevelData == null) return;

    //     currentExp += currentLevelData.expSpawn;
        
    //     character.AddSize(currentLevelData.size);
        
    //     character.GetAttackRange() += currentLevelData.attackRange;

    //     CheckLevelUp();
    // }

    private void CheckLevelUp()
    {
        CharacterLevelItemData config = character.GetLevelData().GetLevelData(character.GetLevel());
        if (config == null) return;

        while (currentExp >= config.expRequire && character.GetLevel() < 5)
        {
            currentExp -= config.expRequire;
            LevelUp();
            
            config = character.GetLevelData().GetLevelData(character.GetLevel());
            if (config == null) break;
        }
    }

    private void LevelUp()
    {
        character.SetLevel(character.GetLevel() + 1);
    }
}

using UnityEngine;

public class BoosterItem : GameUnit
{
    public BoosterType type;

    private void Update()
    {
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Cache.Ins.TryGetCharacter(other, out CharacterBase character))
        {
            if (!character.GetIsDead())
            {
                // character.ApplyBooster(type, amount, duration);
                BoosterItemData data = DataManager.Ins.boosterData.GetBooster(type);

                if (data != null)
                {
                    character.ApplyBooster(type, data.amount, data.duration);
                }
                
                SimplePool.Despawn(this);
            }
        }
    }
}
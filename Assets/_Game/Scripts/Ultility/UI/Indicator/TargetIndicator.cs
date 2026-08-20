using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private Player player; 
    
    [SerializeField] private SpriteRenderer spriteRenderer; 
    
    [SerializeField] private float heightOffset = -1.25f; 
    
    private CharacterBase currentTarget;

    private void Awake()
    {
        transform.SetParent(null);
        
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;  
        }
    }

    private void LateUpdate()
    {
        if (player == null || player.GetIsDead()) 
        {
            SwitchTarget(null);
            return;
        }

        CharacterBase newTarget = GetTargetFromPlayer(player);
        if (currentTarget != newTarget)
        {
            SwitchTarget(newTarget);
        }
    }

    private void SwitchTarget(CharacterBase target)
    {
        currentTarget = target;
        
        if (currentTarget != null)
        {
            transform.SetParent(currentTarget.TF, false);
            transform.localPosition = new Vector3(0, heightOffset, 0);
            transform.localRotation = Quaternion.Euler(90, 0, 0); 
            
            if (spriteRenderer != null) spriteRenderer.enabled = true;
        }
        else
        {
            transform.SetParent(null);
            if (spriteRenderer != null) spriteRenderer.enabled = false;
        }
    }

    private CharacterBase GetTargetFromPlayer(Player p)
    {
        p.GetTargetsInRange().RemoveAll(t => t == null || t.GetIsDead());
        float attackRangeSqr = (p.GetAttackRange() * p.GetSize()) * (p.GetAttackRange() * p.GetSize());
        
        foreach (CharacterBase t in p.GetTargetsInRange())
        {
            if ((t.TF.position - p.TF.position).sqrMagnitude <= attackRangeSqr)
            {
                return t; 
            }
        }
        return null;
    }
}
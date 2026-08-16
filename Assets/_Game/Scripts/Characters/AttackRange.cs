using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackRange : MonoBehaviour
{
    [SerializeField] private Character owner;

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;
        
        if (Cache.Ins.TryGetCharacter(other, out Character target))
        {
            if (target != owner && !target.IsDead)
            {
                owner.AddTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (owner == null) return;

        if (Cache.Ins.TryGetCharacter(other, out Character target))
        {
            owner.RemoveTarget(target);
        }
    }
}

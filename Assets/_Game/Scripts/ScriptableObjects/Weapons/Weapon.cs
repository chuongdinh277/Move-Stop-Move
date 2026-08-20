using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private PoolType bulletPoolType; 
    [SerializeField] private Transform spawnPoint;

    public WeaponType Type => weaponType;

    public void Throw(CharacterBase attacker, Vector3 targetPosition, bool isTargeting)
    {
        gameObject.SetActive(false); 

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        spawnPos.y = attacker.TF.position.y + 1f;

        Vector3 exactDirection = attacker.TF.forward;

        if (isTargeting)
        {
            targetPosition.y = spawnPos.y; 
            exactDirection = (targetPosition - spawnPos).normalized;
        }

        BulletBase bullet = SimplePool.Spawn<BulletBase>(bulletPoolType, spawnPos, Quaternion.identity);
        bullet.OnInit(attacker, exactDirection);
    }
}


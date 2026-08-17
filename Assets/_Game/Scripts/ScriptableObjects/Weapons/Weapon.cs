using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private PoolType bulletPoolType; 
    [SerializeField] private Transform spawnPoint;

    public WeaponType Type => weaponType;

    public void Throw(Character attacker, Vector3 direction)
    {
        gameObject.SetActive(false); 

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        BulletBase bullet = SimplePool.Spawn<BulletBase>(bulletPoolType, spawnPos, Quaternion.identity);
        
        bullet.OnInit(attacker, direction);
    }  
}

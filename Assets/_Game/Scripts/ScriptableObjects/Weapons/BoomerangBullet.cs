using UnityEngine;

public class BoomerangBullet : BulletBase
{
    [Header("Boomerang Settings")]
    [SerializeField] private float spinSpeed = 1000f; 
    [SerializeField] private float returnSpeedMultiplier = 1.5f;

    private Vector3 startPosition;
    private bool isReturning = false;

    public override void OnInit(Character attacker, Vector3 direction)
    {
        base.OnInit(attacker, direction);
        startPosition = transform.position;
        isReturning = false;
    }

    protected override void Update()
    {
        base.Update(); 
        
        // Xoay như chong chóng
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);

        if (!isReturning)
        {
            // Lượt đi: Bay thẳng
            transform.position += transform.forward * SPEED * Time.deltaTime;

            // Kiểm tra xem đã bay hết tầm chưa
            if (Vector3.Distance(startPosition, transform.position) >= attacker.AttackRange)
            {
                isReturning = true;
            }
        }
        else
        {
            // Lượt về: Bay ngược lại chủ nhân
            if (attacker != null && !attacker.IsDead)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    attacker.transform.position, 
                    SPEED * returnSpeedMultiplier * Time.deltaTime
                );
                
                // Nếu bắt được thì thu hồi
                if (Vector3.Distance(transform.position, attacker.transform.position) < 0.5f)
                {
                    OnDespawn();
                }
            }
            else
            {
                // Chủ nhân chết thì rơi rụng
                OnDespawn();
            }
        }
    }

    protected override void HitTarget(Character target)
    {
        // Vẫn gây sát thương nhưng chém xong là quay đầu luôn
        Debug.Log($"<color=orange>Boomerang chém trúng {target.gameObject.name}</color>");
        isReturning = true;
    }
}

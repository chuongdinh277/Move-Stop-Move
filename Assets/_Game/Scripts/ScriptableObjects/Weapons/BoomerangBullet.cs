using UnityEngine;

public class BoomerangBullet : BulletBase
{
    [Header("Boomerang Settings")]
    [SerializeField] private float rotateSpeed = -720f;
    [SerializeField] private Transform visualMesh;
    [SerializeField] private bool isFlat = true;
    [SerializeField] private float maxDistance = 10f;
    
    private bool isReturning = false;

    public override void OnInit(CharacterBase attacker, Vector3 direction)
    {
        base.OnInit(attacker, direction);
        isReturning = false;
        if (isFlat)
        {
            if (visualMesh != null)
            {
                visualMesh.localEulerAngles = new Vector3(90, 0, 0);
            }
            else
            {
                transform.Rotate(90, 0, 0, Space.Self);
            }
        }
    }

    protected override void Update()
    {
        if (visualMesh != null)
        {
            visualMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }

        if (attacker != null && !attacker.GetIsDead())
        {
            float distanceToAttacker = Vector3.Distance(transform.position, attacker.TF.position);

            if (!isReturning && distanceToAttacker >= maxDistance)
            {
                isReturning = true;
            }

            if (isReturning)
            {
                Vector3 returnDir = (attacker.TF.position - transform.position).normalized;
                returnDir.y = 0;
                moveDirection = returnDir;

                if (distanceToAttacker <= 1f)
                {
                    OnDespawn();
                    return; 
                }
            }
        }
        else if (isReturning)
        {
            OnDespawn();
            return;
        }

        base.Update();
    }
}

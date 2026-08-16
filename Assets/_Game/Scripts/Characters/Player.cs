using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotateSpeed = 10f;

    private Vector3 currentInputDirection;

    public override void OnInit()
    {
        base.OnInit();
    }

    private void Start()
    {
        SetUpCamera();
    }

    private void Update()
    {
        HandleInput();  
        MoveTransform();
    }

    private void SetUpCamera()
    {
        if (CameraFollow.Ins != null)
        {
            CameraFollow.Ins.target = this.transform;
        }
    }

    private void HandleInput()
    {
        if (IsDead) return;

        currentInputDirection = GetInputDirection();
        UpdateAnimation(currentInputDirection);
    }

    private void UpdateAnimation(Vector3 direction)
    {
        if (direction.sqrMagnitude >= 0.01f)
        {
            CancelAttack();
            ChangeAnim(GameConfig.ANIM_RUN);
        }
        else
        {
            HandleIdleOrAttack();
        }
    }

    private void CancelAttack()
    {
        if (isAttacking) 
        {
            CancelInvoke(nameof(SpawnWeaponBullet));
            CancelInvoke(nameof(ResetAttackState));
            ResetAttackState();
        }
    }

    private void HandleIdleOrAttack()
    {
        targetsInRange.RemoveAll(t => t == null || t.IsDead);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (targetsInRange.Count > 0)
            {
                Attack();
            }
            else 
            {
                Debug.Log("Chưa có mục tiêu trong tầm ngắm để ném");
            }
        }
        else if (!isAttacking)
        {
            ChangeAnim(GameConfig.ANIM_IDLE);
        }
    }

    private Vector3 GetInputDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void MovePhysics()
    {
        if (IsDead) return;

        if (currentInputDirection.sqrMagnitude >= 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentInputDirection);
            TF.rotation = Quaternion.Slerp(TF.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            TF.Translate(currentInputDirection * moveSpeed * Time.deltaTime, Space.World);
            
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            }
        }
        else
        {
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            }
        }
    }
}

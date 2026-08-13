using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotateSpeed = 15f;

    private Vector3 currentInputDirection;

    private void Update()
    {
        HandleInput();  
    }

    private void FixedUpdate()
    {
        MovePhysics();  
    }

    private void Start()
    {
        SetUpCamera();
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

        if(currentInputDirection.sqrMagnitude >= 0.01f)
        {
            ChangeAnim(GameConfig.ANIM_RUN);
        }
        else
        {
            ChangeAnim(GameConfig.ANIM_IDLE);
        }
    }

    private void MovePhysics()
    {
        if (IsDead) return;

        if (currentInputDirection.sqrMagnitude >= 0.01f)
        {
            RotateTowardsFixed(currentInputDirection);
            TranslateFixed(currentInputDirection);
        }
        else
        {
            Vector3 vel = rb.linearVelocity;
            vel.x = 0;
            vel.z = 0;
            rb.linearVelocity = vel;
        }
    }

    private Vector3 GetInputDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void TranslateFixed(Vector3 directiron)
    {
        Vector3 newVel = directiron * moveSpeed;
        newVel.y = rb.linearVelocity.y;
        rb.linearVelocity = newVel;
    }

    private void RotateTowardsFixed(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        rb.MoveRotation(newRotation);
    }
}

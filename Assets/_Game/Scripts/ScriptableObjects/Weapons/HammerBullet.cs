using UnityEngine;

public class HammerBullet : BulletBase
{
    [Header("Spin Settings")]
    [SerializeField] private float rotateSpeed = -720f;
    [SerializeField] private Transform visualMesh; 
    [SerializeField] private bool isFlat = true;

    public override void OnInit(CharacterBase attacker, Vector3 direction)
    {
        base.OnInit(attacker, direction);
        
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
        base.Update();
        
        if (visualMesh != null)
        {
            visualMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}


using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Ins;
    public Transform target;
    
    public Vector3 offset = new Vector3(0, 15f, -15f); 
    public float smoothSpeed = 10f;

    private void Awake()
    {
        Ins = this;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
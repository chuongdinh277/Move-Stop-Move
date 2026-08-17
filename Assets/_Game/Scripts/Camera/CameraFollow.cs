using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Ins;
    public Transform target;
    public Vector3 offset = new Vector3();

    public float smoothSpeed = 5f;
    private Transform tf;

    public Transform TF => tf == null ? tf = transform : tf;

    private Vector3 defaultOffset = new Vector3();
    private Vector3 currentOffset;
    
    private bool isZooming = false;

    private void Awake()
    {
        Ins = this;
        currentOffset = TF.transform.position;
        offset = TF.transform.position;
    }

    public void ResetCamera()
    {
        offset = defaultOffset;
        currentOffset = defaultOffset;
        isZooming = false;
        TF.rotation = Quaternion.Euler(TF.transform.rotation.x, TF.transform.rotation.y, TF.transform.rotation.z);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            FollowTarget();
            HandleZooming();
        }
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + offset;
        TF.position = Vector3.Lerp(TF.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    private void HandleZooming()
    {
        if (isZooming)
        {
            Quaternion targetRotation = Quaternion.LookRotation((target.position + Vector3.up * 1.5f) - TF.position);
            TF.rotation = Quaternion.Slerp(TF.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }
}
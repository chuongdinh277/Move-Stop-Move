using UnityEngine;

public class KnifeBullet : BulletBase
{
    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector3.forward * SPEED * Time.deltaTime);
    }
}

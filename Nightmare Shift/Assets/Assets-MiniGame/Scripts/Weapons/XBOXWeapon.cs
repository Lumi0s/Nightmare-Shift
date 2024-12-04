using UnityEngine;
using static Data;

public class XBOXWeapon : Weapon
{
    [SerializeField] Rigidbody rb;

    protected override float baseDamage => xboxWeaponDmg;
    protected override float speed => 20f;
    protected override float lifeTime => 8f;
    protected override float comboScaler => 0;

    const float bounceForce = 2f;

    protected override void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        base.OnCollisionEnter(collision);
    }

    public override void Init(GameObject source, GameObject target)
    {
        base.Init(source, target);
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}

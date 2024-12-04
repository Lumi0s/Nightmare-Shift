using UnityEngine;
using static Data;

public class BasicWeapon : Weapon
{
    [SerializeField] Rigidbody rb;

    protected override float baseDamage => basicWeaponDmg;
    protected override float speed => 10f;
    protected override float lifeTime => 5f;

    public override void Init(GameObject source, GameObject target)
    {
        base.Init(source, target);
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}

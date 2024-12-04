using UnityEngine;
using static Data;
using static UnityEngine.GraphicsBuffer;

public class Fireball : Weapon
{
    [SerializeField] Rigidbody rb;

    protected override float baseDamage => fireballDmg;
    protected override float speed => 12f;
    protected override float lifeTime => 5f;
    protected override float cooldown => 10f;

    public override void Init(GameObject source, GameObject target)
    {
        base.Init(source, target);
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}

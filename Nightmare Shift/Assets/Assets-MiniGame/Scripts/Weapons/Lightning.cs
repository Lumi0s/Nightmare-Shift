using System.Collections;
using UnityEngine;
using static Data;

public class Lightning : Weapon
{
    [SerializeField] Rigidbody rb;

    GameObject lightningEffect;

    protected override float baseDamage => lightningDmg;
    protected override float speed => 10f;
    protected override float lifeTime => 2f;
    protected override float cooldown => 15f;

    public override void Init(GameObject source, GameObject target)
    {
        base.Init(source, target);
        // i'm sorry for this, but i'm running out of time ;_;
        lightningEffect = FindInactiveObjectWithTag(lightningEffectTag);
        StartCoroutine(EnableLightningEffect(true, 1f));
    }

    IEnumerator EnableLightningEffect(bool enable, float time)
    {
        yield return new WaitForSeconds(time);

        if (lightningEffect != null)
            lightningEffect.SetActive(enable);

        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    protected override void Destroy()
    {
        StartCoroutine(EnableLightningEffect(false, 0f));
        base.Destroy();
    }
}

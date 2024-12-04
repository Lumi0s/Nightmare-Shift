using DG.Tweening;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] GameObject particleHitEffect;

    protected virtual float currentDamage { get; set; } = 0;
    protected virtual float baseDamage { get; set; } = 0;
    protected virtual float speed { get; set; } = 0;
    protected virtual float lifeTime { get; set; } = 0;
    protected virtual float cooldown { get; set; } = 0;
    protected virtual float comboScaler { get; set; } = 0.5f;
    protected virtual float currentCombo { get; set; } = 0f;
    protected virtual GameObject source { get; set; }
    protected virtual GameObject target { get; set; }

    public float Cooldown => cooldown;

    public bool initialized { get; private set; } = false;
    bool causedDamage = false;

    const float animationDuration = 0.2f;
    const float destroyTimeAfterCollision = 0.25f;
    const float hitParticlesScale = 0.25f;

    protected void Start()
    {
        StartCoroutine(DestroyAfterTime());
        currentDamage = baseDamage;
        Combo.onComboChanged += OnComboChanged;
        OnComboChanged(Combo.ComboCounter);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        CheckForCollisionWithCreature(collision.gameObject);
        SpawnHitParticles(collision.contacts[0].point);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CheckForCollisionWithCreature(other.gameObject);
        SpawnHitParticles(other.ClosestPoint(transform.position));
    }

    protected virtual void OnDestroy()
    {
        Combo.onComboChanged -= OnComboChanged;
        StopAllCoroutines();
    }

    protected void OnComboChanged(int combo)
    {
        currentDamage = baseDamage + combo * comboScaler;
        currentCombo = combo;
    }

    void SpawnHitParticles(Vector3 hitPoint)
    {
        if (particleHitEffect == null)
            return;

        var particles = Instantiate(particleHitEffect);
        particles.transform.localScale = new Vector3(hitParticlesScale, hitParticlesScale, hitParticlesScale);
        particles.transform.position = hitPoint;
    }

    void CheckForCollisionWithCreature(GameObject go)
    {
        // quick and hacky way to prevent multiple hits
        if (causedDamage)
            return;

        var creature = go.GetComponentInParent<Creature>();
        // if this is creature and it spawned this weapon, don't hit it
        if (creature != null && go != source)
        {
            creature.UpdateHealth(-currentDamage);
            causedDamage = true;
            Invoke("Destroy", destroyTimeAfterCollision);

            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                    (byte)(Color.green.r * 255f), (byte)(Color.green.g * 255f), (byte)(Color.green.b * 255f),
                    $"DMG to {creature.gameObject.name}: {currentDamage} - {gameObject.name}"));
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }

    protected virtual void Destroy()
    {
        transform.DOScale(0, animationDuration).OnComplete(() => Destroy(gameObject));
    }

    public virtual void Init(GameObject source, GameObject target)
    {
        this.source = source;

        if (source == null)
        {
            Debug.Log("AAAAAAAAAAAAAAAA");
        }
        this.target = target;
        initialized = true;
    }
}

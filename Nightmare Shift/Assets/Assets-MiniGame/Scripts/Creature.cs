using System.Collections;
using UnityEngine;
using static Data;

public class Creature : MonoBehaviour
{
    [SerializeField] protected HealthBar    healthBar;
    [SerializeField] protected ScoreIndicator scoreIndicator;
    [SerializeField] protected Animator     animator;

    [SerializeField] protected float currentHealth;

    protected float maxHealth;
    protected bool dead = false;

    private const float delayBeforeSceneLoad = 0.5f;

    virtual protected void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    virtual public void UpdateHealth(float value)
    {
        if (dead)
            return;

        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth;
        healthBar.SetHealth(healthPercentage);
        scoreIndicator.UpdateScoreIndicator(value);

        if (currentHealth <= 0)
        {
            animator.ResetTrigger(animationStates[Data.AnimationState.GettingHit]);
            dead = true;
            Die();
        }
    }

    virtual protected void Die()
    {
        animator.ResetTrigger(animationStates[Data.AnimationState.GettingHit]);
        animator.SetTrigger(animationStates[Data.AnimationState.Dying]);
    }

    virtual protected void GetHit()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.GettingHit]);
    }

    protected IEnumerator LoadEndSceneAfterDelay(bool win)
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad);
        SceneController.Instance.EndGame(win);
    }
}

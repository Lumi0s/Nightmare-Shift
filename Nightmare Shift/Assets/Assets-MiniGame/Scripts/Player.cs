using System;
using System.Collections;
using UnityEngine;
using static Data;
using static Timers;

public class Player : Creature
{
    [Serializable]
    class AttackTypes
    {
        public BasicWeapon basicWeapon;
        public Fireball fireball;
        public Lightning lightning;
    }

    [SerializeField] TextFollower   textFollower;
    [SerializeField] Timers         timer;
    [SerializeField] AudioManager   audioManager;
    [SerializeField] AttackTypes    attackTypes;
    [SerializeField] LookAtTarget   lookAtTarget;
    [SerializeField] GameObject     weaponHolder;
    [SerializeField] GameObject     healingEffectParticles;
    [SerializeField] GameObject     defendEffectParticles;

    [SerializeField] float immunityDuration = 5f;
    [SerializeField] float drinkingCooldown = 30f;

    GameObject spawnedWeapon;

    bool isImmune = false;

    const float onTextMissedPenalty = 0.5f;
    const float potionHealingAmount = 50f;

    override protected void Start()
    {
        maxHealth = playerHealth;
        base.Start();
        textFollower.onScoreChanged += OnTextFollowerScoreChanged;
        InitializeCommands();
    }

    void OnDestroy()
    {
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.RegularAttack]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackFireball]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackLightning]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.Defending]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.DrinkingPotion]);

        StopAllCoroutines();
    }

    public override void UpdateHealth(float value)
    {
        if (isImmune || dead)
        {
            return;
        }

        if (value < 0)
        {
            GetHit();
        }
        base.UpdateHealth(value);
    }

    #region CommandFunctions
    void BasicAttack()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.RegularAttack]);
        InitWeapon(attackTypes.basicWeapon);
        audioManager.PlayBasicAttackSound();
    }

    void AttackFireball()
    {
        if (timer.IsReady(TimerType.Fireball))
        {
            InitWeapon(attackTypes.fireball);
            animator.SetTrigger(animationStates[Data.AnimationState.AttackFireball]);
            audioManager.PlayFireballAttackSound();
            timer.StartTimer(attackTypes.fireball.Cooldown, TimerType.Fireball);
        }
    }

    void AttackLightning()
    {
        if (timer.IsReady(TimerType.Light))
        {
            InitWeapon(attackTypes.lightning);
            animator.SetTrigger(animationStates[Data.AnimationState.AttackLightning]);
            audioManager.PlayLightningAttackSound();
            timer.StartTimer(attackTypes.lightning.Cooldown, TimerType.Light);
        }
    }
    void Defend()
    {
        if (timer.IsReady(TimerType.Defend))
        {
            var particles = Instantiate(defendEffectParticles);
            particles.transform.position = transform.position;

            animator.SetTrigger(animationStates[Data.AnimationState.Defending]);
            ActivateImmunity();
            audioManager.PlayPlayerDefenseSound();
            timer.StartTimer(immunityDuration, TimerType.Defend);
        }
    }

    void DrinkPotion()
    {
        if (timer.IsReady(TimerType.Drink))
        {
            var particles = Instantiate(healingEffectParticles);
            particles.transform.position = transform.position;

            animator.SetTrigger(animationStates[Data.AnimationState.DrinkingPotion]);
            audioManager.PlayPlayerDrinkSound();
            Heal(potionHealingAmount);
            timer.StartTimer(drinkingCooldown, TimerType.Drink);
        }
    }
    #endregion

    protected override void GetHit()
    {
        if (dead)
            return;

        base.GetHit();
        audioManager.PlayPlayerHitSound();
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(LoadEndSceneAfterDelay(false));
    }

    void InitWeapon(Weapon weapon)
    {
        spawnedWeapon = Instantiate(weapon.gameObject, weaponHolder.transform.position, Quaternion.identity);
        spawnedWeapon.transform.LookAt(Vector3.forward);
        spawnedWeapon.transform.parent = transform.parent;
        spawnedWeapon.name = weapon.name;
        spawnedWeapon.GetComponent<Weapon>().Init(gameObject, lookAtTarget.Target);
    }

    void ActivateImmunity()
    {
        if (!isImmune)
        {
            isImmune = true;
            StartCoroutine(ImmunityCooldown());
        }
    }

    IEnumerator ImmunityCooldown()
    {
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
    }

    void OnTextFollowerScoreChanged(int score)
    {
        if (score > 0 || isImmune)
        {
            return;
        }

        UpdateHealth(-onTextMissedPenalty * Mathf.Abs(score));
    }

    void Heal(float amount)
    {
        amount = Mathf.Clamp(amount, 0, maxHealth);
        UpdateHealth(amount);
    }

    void InitializeCommands()
    {
        CommandSet.AddCommand(animationStates[Data.AnimationState.RegularAttack], new() { BasicAttack });
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackFireball], new() { AttackFireball });
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackLightning], new() { AttackLightning });
        CommandSet.AddCommand(animationStates[Data.AnimationState.Defending], new() { Defend });
        CommandSet.AddCommand(animationStates[Data.AnimationState.DrinkingPotion], new() { DrinkPotion });
    }
}

using TMPro;
using UnityEngine;

public class Timers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerBasicAttack;
    [SerializeField] TextMeshProUGUI timerFireball;
    [SerializeField] TextMeshProUGUI timerLight;
    [SerializeField] TextMeshProUGUI timerDrink;
    [SerializeField] TextMeshProUGUI timerDefend;

    private float remainingAttackTime;
    private float remainingFireballTime;
    private float remainingLightTime;
    private float remainingDrinkTime;
    private float remainingDefendTime;

    private bool isAttackRunning;
    private bool isFireballRunning;
    private bool isLightRunning;
    private bool isDrinkRunning;
    private bool isDefendRunning;

    public enum TimerType
    {
        Fireball = 0,
        Light,
        Drink,
        Defend,
        Attack
    }

    public void StartTimer(float cooldownDuration, TimerType value)
    {
        switch (value)
        {
            case TimerType.Attack:
                remainingAttackTime = cooldownDuration;
                isAttackRunning = true;
                ActivateTimerText(timerBasicAttack, "ATTACK: ");
                break;
            case TimerType.Fireball:
                remainingFireballTime = cooldownDuration;
                isFireballRunning = true;
                ActivateTimerText(timerFireball, "FIRE: ");
                break;
            case TimerType.Light:
                remainingLightTime = cooldownDuration;
                isLightRunning = true;
                ActivateTimerText(timerLight, "LIGHT: ");
                break;
            case TimerType.Drink:
                remainingDrinkTime = cooldownDuration;
                isDrinkRunning = true;
                ActivateTimerText(timerDrink, "POTION: ");
                break;
            case TimerType.Defend:
                remainingDefendTime = cooldownDuration;
                isDefendRunning = true;
                ActivateTimerText(timerDefend, "DEFEND TIME: ");
                break;
            default:
                Debug.LogWarning("Wrong timer value");
                break;
        }
    }

    void Update()
    {
        UpdateTimer(ref remainingAttackTime, ref isAttackRunning, timerBasicAttack);
        UpdateTimer(ref remainingFireballTime, ref isFireballRunning, timerFireball);
        UpdateTimer(ref remainingLightTime, ref isLightRunning, timerLight);
        UpdateTimer(ref remainingDrinkTime, ref isDrinkRunning, timerDrink);
        UpdateTimer(ref remainingDefendTime, ref isDefendRunning, timerDefend);
    }

    private void UpdateTimer(ref float remainingTime, ref bool isRunning, TextMeshProUGUI timerText)
    {
        if (isRunning)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isRunning = false;
                timerText.text = "Ready";
                timerText.gameObject.SetActive(false);
            }
            else
            {
                // Update the timer text with the remaining time
                timerText.text = timerText.text.Split(':')[0] + ": " + remainingTime.ToString("F1") + "s";
            }
        }
    }

    private void ActivateTimerText(TextMeshProUGUI timerText, string skillName)
    {
        timerText.text = skillName;
        timerText.gameObject.SetActive(true);
    }

    public bool IsReady(TimerType value)
    {
        return value switch
        {
            TimerType.Fireball => !isFireballRunning,
            TimerType.Light => !isLightRunning,
            TimerType.Drink => !isDrinkRunning,
            TimerType.Defend => !isDefendRunning,
            TimerType.Attack => !isAttackRunning,
            _ => false
        };
    }
}

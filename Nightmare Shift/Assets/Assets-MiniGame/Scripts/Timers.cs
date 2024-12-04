using TMPro;
using UnityEngine;

public class Timers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerFireball;
    [SerializeField] TextMeshProUGUI timerLight;
    [SerializeField] TextMeshProUGUI timerDrink;
    [SerializeField] TextMeshProUGUI timerDefend;

    private float remainingFireballTime;
    private float remainingLightTime;
    private float remainingDrinkTime;
    private float remainingDefendTime;

    private bool isFireballRunning;
    private bool isLightRunning;
    private bool isDrinkRunning;
    private bool isDefendRunning;

    public enum TimerType
    {
        Fireball = 0,
        Light,
        Drink,
        Defend
    }

    public void StartTimer(float cooldownDuration, TimerType value)
    {
        switch (value)
        {
            case TimerType.Fireball:
                remainingFireballTime = cooldownDuration;
                isFireballRunning = true;
                ActivateTimerText(timerFireball, "FIREBALL COOLDOWN: ");
                break;
            case TimerType.Light:
                remainingLightTime = cooldownDuration;
                isLightRunning = true;
                ActivateTimerText(timerLight, "LIGHTNING COOLDOWN: ");
                break;
            case TimerType.Drink:
                remainingDrinkTime = cooldownDuration;
                isDrinkRunning = true;
                ActivateTimerText(timerDrink, "DRINK POTION COOLDOWN: ");
                break;
            case TimerType.Defend:
                remainingDefendTime = cooldownDuration;
                isDefendRunning = true;
                ActivateTimerText(timerDefend, "DEFENSE ACTIVE FOR: ");
                break;
            default:
                Debug.LogWarning("Wrong timer value");
                break;
        }
    }

    void Update()
    {
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
            _ => false
        };
    }
}

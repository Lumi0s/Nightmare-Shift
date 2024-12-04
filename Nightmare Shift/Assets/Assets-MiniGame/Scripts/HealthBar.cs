using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image                  healthFillImage;
    [SerializeField] TMPro.TextMeshProUGUI  healthText;

    float maxHealth;

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        healthFillImage.fillAmount = 1f;
    }

    public void SetHealth(float healthPercentage)
    {
        float currentHealth = maxHealth * healthPercentage;
        float fillValue = (float)currentHealth / maxHealth;
        healthFillImage.fillAmount = fillValue;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image background;
    public Image fill;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        fill.fillAmount = currentHealth / maxHealth;
    }
}
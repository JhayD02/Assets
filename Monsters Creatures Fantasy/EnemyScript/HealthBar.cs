using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HealthBar : NetworkBehaviour
{
    public Image background;
    public Image fill;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (isLocalPlayer){
        fill.fillAmount = currentHealth / maxHealth;
        }
    }
}
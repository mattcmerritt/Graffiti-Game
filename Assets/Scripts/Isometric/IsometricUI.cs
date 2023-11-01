using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsometricUI : MonoBehaviour
{
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image DashIndicator;

    // Internal data
    private float DashCooldown, MaxDashCooldown;

    private void Start()
    {
        IsometricEncounter.Instance.OnPlayerHealthChanged += UpdateHealthIndicator;
        IsometricEncounter.Instance.OnPlayerDash += UpdateDashIndicator;
    }

    private void Update()
    {
        if (DashCooldown > 0f)
        {
            DashCooldown -= Time.deltaTime;
            DashIndicator.fillAmount = 1 - DashCooldown / MaxDashCooldown;
        }
        else
        {
            DashIndicator.fillAmount = 1f;
            // could play an animation here
        }
    }

    private void UpdateHealthIndicator(float newHealth)
    {
        float lives = newHealth;
        HealthBar.fillAmount = newHealth;
    }

    private void UpdateDashIndicator(float cooldown)
    {
        DashCooldown = cooldown;
        MaxDashCooldown = cooldown;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsometricUI : MonoBehaviour
{
    [SerializeField] private Image HealthBar;

    private void Start()
    {
        IsometricEncounter.Instance.OnPlayerHealthChanged += UpdateHealthIndicator;
    }

    private void UpdateHealthIndicator(float newHealth)
    {
        float lives = newHealth;
        HealthBar.fillAmount = newHealth;
    }
}

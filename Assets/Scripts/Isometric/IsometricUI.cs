using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> Lives;

    private void Start()
    {
        IsometricEncounter.Instance.OnPlayerHealthChanged += UpdateHealthIndicator;
    }

    private void UpdateHealthIndicator(float newHealth)
    {
        int lives = Mathf.RoundToInt(newHealth);
        for (int i = 0; i < Lives.Count; i++)
        {
            Lives[i].SetActive(i < lives);
        }
    }
}

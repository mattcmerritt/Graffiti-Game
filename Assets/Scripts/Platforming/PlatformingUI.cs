using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformingUI : MonoBehaviour
{
    [SerializeField] private Image DashIndicator;
    [SerializeField] private Image DoubleJumpIndicator;
    [SerializeField] private GameObject DashUI, JumpUI;

    // Internal data
    private float DashCooldown, MaxDashCooldown;
    private bool Paused;

    private void Start()
    {
        PlatformingLevel.Instance.OnPlayerDash += UpdateDashIndicator;
        PlatformingLevel.Instance.OnPlayerDoubleJump += () => UpdateDoubleJumpIndicator(false);
        PlatformingLevel.Instance.OnPlayerDoubleJumpRefresh += () => UpdateDoubleJumpIndicator(true);
        PlatformingLevel.Instance.OnPause += Pause;
        PlatformingLevel.Instance.OnResume += Resume;
        PlatformingLevel.Instance.OnPause += HideAbilities;
        PlatformingLevel.Instance.OnResume += ShowAbilities;
        
        // Initially show the abilities to in cases where no dialogue starts immediately
        ShowAbilities();
    }

    private void Update()
    {
        if (!Paused)
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
    }

    private void UpdateDashIndicator(float cooldown)
    {
        DashCooldown = cooldown;
        MaxDashCooldown = cooldown;
    }

    private void UpdateDoubleJumpIndicator(bool status)
    {
        if (status)
        {
            DoubleJumpIndicator.fillAmount = 1f;
            // could play an animation here
        }
        else {
            DoubleJumpIndicator.fillAmount = 0f;
        }
    }

    private void Pause()
    {
        Paused = true;
    }

    private void Resume()
    {
        Paused = false;
    }

    private void HideAbilities()
    {
        DashUI.SetActive(false);
        JumpUI.SetActive(false);
    }

    private void ShowAbilities()
    {
        if (BuddyManager.Instance.GetBuddy<DoubleJumpBuddy>())
        {
            JumpUI.SetActive(true);
        }
        if (BuddyManager.Instance.GetBuddy<DashBuddy>())
        {
            DashUI.SetActive(true);
        }
    }
}

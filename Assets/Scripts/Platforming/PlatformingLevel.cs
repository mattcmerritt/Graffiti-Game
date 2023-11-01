using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingLevel : MonoBehaviour
{
    public static PlatformingLevel Instance;
    private static PlatformingPlayer Player;
    private bool Paused;

    public event Action<float> OnPlayerDash;
    public event Action OnPlayerDoubleJump;
    public event Action OnPlayerDoubleJumpRefresh;
    public event Action OnPause;
    public event Action OnResume;

    // Configure singleton instance at earliest step
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Player = FindObjectOfType<PlatformingPlayer>();

        Player.OnDash += TrackPlayerDash;
        Player.OnDoubleJump += TrackPlayerDoubleJump;
        Player.OnDoubleJumpRefresh += TrackPlayerDoubleJumpRefresh;
    }

    private void Update()
    {
        // Debug Pausing
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void TrackPlayerDash(float cooldown)
    {
        OnPlayerDash?.Invoke(cooldown);
        Debug.Log("dash cd: " + cooldown);
    }

    private void TrackPlayerDoubleJump()
    {
        OnPlayerDoubleJump?.Invoke();
        Debug.Log("double jump");
    }

    private void TrackPlayerDoubleJumpRefresh()
    {
        OnPlayerDoubleJumpRefresh?.Invoke();
        Debug.Log("double jump reset");
    }

    private void PauseGame()
    {
        Paused = true;
        OnPause?.Invoke();
    }

    private void ResumeGame()
    {
        Paused = false;
        OnResume?.Invoke();
    }
}

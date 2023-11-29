using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingEncounter : PlatformingLevel
{
    public event Action OnWallOfDeathCollision;
    public event Action OnEncounterClear;

    private SceneTransitioner Transitioner;

    public new static PlatformingEncounter Instance;

    protected new void Awake()
    {
        base.Awake();

        Instance = this;
    }

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
    }

    public void FailEncounter()
    {
        OnWallOfDeathCollision?.Invoke();
    }

    public void ClearEncounterSuccessfully()
    {
        OnEncounterClear?.Invoke();
    }

    public void ReturnToCityFail()
    {
        StartCoroutine(Transitioner.ReturnToCity(false));
    }

    public void ReturnToCitySuccess()
    {
        StartCoroutine(Transitioner.ReturnToCity(true));
    }
}

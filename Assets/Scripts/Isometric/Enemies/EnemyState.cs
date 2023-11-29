using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    // Equivalent to a Start method, but for a state
    public abstract void ActivateState();

    // Equivalent to an Update method, will be called every frame
    public abstract void PerformStateBehavior();

    // Will perform any cleanup required to switch to next state
    public abstract void DeactivateState();
}

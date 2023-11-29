using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base agent behaviors for all intelligent/path enemies.
public class Enemy : MonoBehaviour
{
    protected EnemyState ActiveState;

    public void ChangeState(EnemyState newState)
    {
        if (ActiveState != null)
        {
            ActiveState.DeactivateState();
        }

        ActiveState = newState;
        if (ActiveState != null)
        {
            ActiveState.ActivateState();
        }
    }

    private void Update()
    {
        if (ActiveState != null)
        {
            ActiveState.PerformStateBehavior();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityChecker : MonoBehaviour
{
    public event Action<Collider> OnTriggerEnterEvent, OnTriggerStayEvent, OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }
}

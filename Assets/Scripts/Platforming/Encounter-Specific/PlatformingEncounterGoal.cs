using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingEncounterGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("The encounter has been cleared.");
    }
}

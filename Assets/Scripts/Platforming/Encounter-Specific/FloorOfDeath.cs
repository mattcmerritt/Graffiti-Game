using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOfDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlatformingPlayer>() != null)
        {
            // Debug.LogWarning("The encounter has been failed.");
            PlatformingEncounter.Instance.FailEncounter();
        }
    }
}

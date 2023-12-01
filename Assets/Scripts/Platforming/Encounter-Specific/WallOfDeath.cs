using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private string ReturnScene;

    private void FixedUpdate()
    {
        if(!PlatformingEncounter.Instance.CheckIfPaused())
        {
            transform.position += new Vector3(ScrollSpeed, 0, 0);
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlatformingPlayer>() != null)
        {
            // Debug.LogWarning("The encounter has been failed.");
            PlatformingEncounter.Instance.FailEncounter();
        }
    }
}

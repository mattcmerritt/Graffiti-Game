using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlatformingPlayer player = other.gameObject.GetComponent<PlatformingPlayer>();
        if(player != null)
        {
            if(player.CheckpointsCleared == 12 && player.EncountersCleared == 3)
            {
                StartCoroutine(FindObjectOfType<SceneTransitioner>().ReturnToMainMenu());
            }
            else
            {
                Debug.Log("There is still more to explore!");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlatformingPlayer>() != null)
        {
            // TODO: allow them to stay dead rather than instantly respawning
            other.gameObject.GetComponent<PlatformingPlayer>().RespawnPlayerAtNearest();
        }
    }
    
    private void Update()
    {
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingLevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;

    private void Start()
    {
        SceneTransitioner sceneTransitioner = FindObjectOfType<SceneTransitioner>();

        if (sceneTransitioner.GetPreviousScene() == "")
        {
            RespawnPlayerAtSpecific("Start");
        }
        else 
        {
            RespawnPlayerAtSpecific(sceneTransitioner.GetPreviousScene());
        }

        
    }

    public void RespawnPlayerAtSpecific(string goal)
    {
        // respawn the player at the goal respawn
        // if goal can't be found, place them at start and put out a warning
        // if start doesn't exist either, put out an error

        RespawnPoint[] respawnPoints = FindObjectsOfType<RespawnPoint>();
        List<GameObject> respawnObjects = new List<GameObject>();
        GameObject startRespawn = null;
        GameObject goalRespawn = null;
        foreach (RespawnPoint r in respawnPoints)
        {
            respawnObjects.Add(r.gameObject);
            if (r.GetRespawnName() == "Start")
            {
                startRespawn = r.gameObject;
            }
            if (r.GetRespawnName() == goal)
            {
                goalRespawn = r.gameObject;
            }
        }

        if (goalRespawn != null)
        {
            Instantiate(PlayerPrefab, goalRespawn.transform.position, Quaternion.identity);
        }
        else if (startRespawn != null)
        {
            Debug.LogWarning("The respawn point " + goal + " does not exist, defaulting to start point");
            Instantiate(PlayerPrefab, startRespawn.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("The respawn point " + goal + " does not exist, and a start point could not be found to spawn the player at either");
        }
    }
}

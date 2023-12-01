using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DripSource : MonoBehaviour
{
    [SerializeField] private GameObject DripPrefab;
    [SerializeField] private float SpawnTimeInterval;
    private float ElapsedTimeSinceLastSpawn;

    private void Start()
    {
        ElapsedTimeSinceLastSpawn = 0;
    }

    private void Update()
    {
        ElapsedTimeSinceLastSpawn += Time.deltaTime;
        
        if (SpawnTimeInterval < ElapsedTimeSinceLastSpawn)
        {
            Instantiate(DripPrefab, transform.position, Quaternion.identity);
            ElapsedTimeSinceLastSpawn = 0;
        }
    }
}

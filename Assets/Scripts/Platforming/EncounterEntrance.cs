using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterEntrance : MonoBehaviour
{
    [SerializeField] private string EncounterSceneName;
    [SerializeField] private SceneTransitioner Transitioner;

    private void Start()
    {
        Transitioner = FindObjectOfType<SceneTransitioner>();

        // remove the encounter trigger if it was already cleared
        if (Transitioner.GetEncounterCompleted(EncounterSceneName))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            StartCoroutine(Transitioner.LoadEncounter(EncounterSceneName));
        }
    }
}

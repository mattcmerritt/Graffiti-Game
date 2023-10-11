using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterEntrance : MonoBehaviour
{
    [SerializeField] private string EncounterSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(EncounterSceneName);
    }
}

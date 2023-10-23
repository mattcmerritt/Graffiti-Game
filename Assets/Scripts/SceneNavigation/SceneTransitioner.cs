using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private string PreviousScene, CurrentScene;
    [SerializeField] private bool CompletedLastEncounter;
    [SerializeField] private List<string> CompletedEncounters;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CompletedEncounters = new List<string>();
    }

    public void LoadEncounter(string encounterName)
    {
        string debugOutput = "<color=green>Transitioner: </color>\nBEFORE:Current: " + CurrentScene + " Previous: " + PreviousScene;
        PreviousScene = CurrentScene;
        CurrentScene = encounterName;
        CompletedLastEncounter = false;
        SceneManager.LoadScene(CurrentScene);
        debugOutput += "\n\nAFTER: Current: " + CurrentScene + " Previous: " + PreviousScene;
        Debug.Log(debugOutput);
    }

    public void ReturnToCity(bool successful)
    {
        string debugOutput = "<color=green>Transitioner: </color>\nBEFORE: Current: " + CurrentScene + " Previous: " + PreviousScene;
        CompletedLastEncounter = successful;
        string tempCurrentScene = CurrentScene;
        CurrentScene = PreviousScene;
        PreviousScene = tempCurrentScene;
        if (successful)
        {
            CompletedEncounters.Add(PreviousScene);
        }
        SceneManager.LoadScene(CurrentScene);
        debugOutput += "\n\nAFTER: Current: " + CurrentScene + " Previous: " + PreviousScene;
        Debug.Log(debugOutput);
    }

    public bool GetEncounterCompleted(string encounter)
    {
        return CompletedEncounters.Contains(encounter);
    }

    public string GetPreviousScene()
    {
        return PreviousScene;
    }
}

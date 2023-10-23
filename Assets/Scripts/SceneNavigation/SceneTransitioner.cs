using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private string PreviousScene, CurrentScene;
    [SerializeField] private bool CompletedLastEncounter;
    [SerializeField] private List<string> CompletedEncounters;

    // Transition animation details
    [SerializeField] private float TransitionDuration;
    [SerializeField] private Animator Animator;

    // State information
    [SerializeField] private bool TransitionActive;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CompletedEncounters = new List<string>();
    }

    public IEnumerator LoadEncounter(string encounterName)
    {
        if (!TransitionActive)
        {
            TransitionActive = true;
            string debugOutput = "<color=green>Transitioner: </color>\nBEFORE:Current: " + CurrentScene + " Previous: " + PreviousScene;
            PreviousScene = CurrentScene;
            CurrentScene = encounterName;
            CompletedLastEncounter = false;
            Animator.SetTrigger("Enter Stage (Good)");
            yield return new WaitForSeconds(TransitionDuration);
            SceneManager.LoadScene(CurrentScene);
            Debug.Log("<color=green>Transitioner: </color> Midway through Encounter transition.");
            Animator.SetTrigger("Leave Stage (Good)");
            // yield return new WaitForSeconds(TransitionDuration);
            Debug.Log("<color=green>Transitioner: </color> Finished Encounter transition.");
            debugOutput += "\n\nAFTER: Current: " + CurrentScene + " Previous: " + PreviousScene;
            Debug.Log(debugOutput);
            TransitionActive = false;
        } 
    }

    public IEnumerator ReturnToCity(bool successful)
    {
        if (!TransitionActive)
        {
            TransitionActive = true;
            string debugOutput = "<color=green>Transitioner: </color>\nBEFORE: Current: " + CurrentScene + " Previous: " + PreviousScene;
            CompletedLastEncounter = successful;
            string tempCurrentScene = CurrentScene;
            CurrentScene = PreviousScene;
            PreviousScene = tempCurrentScene;
            if (successful)
            {
                CompletedEncounters.Add(PreviousScene);
                Animator.SetTrigger("Enter Stage (Good)");
            }
            else
            {
                Animator.SetTrigger("Enter Stage (Bad)");
            }
            yield return new WaitForSeconds(TransitionDuration);
            SceneManager.LoadScene(CurrentScene);
            Debug.Log("<color=green>Transitioner: </color> Midway through City transition.");
            if (successful)
            {
                CompletedEncounters.Add(PreviousScene);
                Animator.SetTrigger("Leave Stage (Good)");
            }
            else
            {
                Animator.SetTrigger("Leave Stage (Bad)");
            }
            // yield return new WaitForSeconds(TransitionDuration);
            Debug.Log("<color=green>Transitioner: </color> Finished City transition.");
            debugOutput += "\n\nAFTER: Current: " + CurrentScene + " Previous: " + PreviousScene;
            Debug.Log(debugOutput);
            TransitionActive = false;
        }
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

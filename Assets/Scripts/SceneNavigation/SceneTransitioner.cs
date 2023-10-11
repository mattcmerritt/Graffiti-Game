using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private int NextScene;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Transition();
        }
    }

    public void Transition()
    {
        SceneManager.LoadScene(NextScene);
    }
}

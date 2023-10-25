using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> Lives;
    [SerializeField] private IsometricPlayer Player;
    [SerializeField] private IsometricSimpleEnemy[] Enemies;
    [SerializeField] private SceneTransitioner Transitioner;

    private void Start()
    {
        Transitioner = FindObjectOfType<SceneTransitioner>();
    }

    private void Update()
    {
        Enemies = FindObjectsOfType<IsometricSimpleEnemy>();

        int lives = Player.GetLives();
        for (int i = 0; i < Lives.Count; i++)
        {
            Lives[i].SetActive(i < lives);
        }

        if (lives <= 0)
        {
            StartCoroutine(Transitioner.ReturnToCity(false));
        }            
        else if (Enemies.Length == 0)
        {
            StartCoroutine(Transitioner.ReturnToCity(true));
        }
    }
}
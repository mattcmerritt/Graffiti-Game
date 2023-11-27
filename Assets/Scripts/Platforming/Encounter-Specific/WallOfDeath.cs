using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private string ReturnScene;

    private void FixedUpdate()
    {
        transform.position += new Vector3(ScrollSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: do something with scene transitioner
        Debug.LogWarning("The encounter has been failed.");
    }
}

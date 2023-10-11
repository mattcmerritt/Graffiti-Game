using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneChecker : MonoBehaviour
{
    [SerializeField] private List<GameObject> CurrentPlanes;

    private void Start() 
    {
        CurrentPlanes = new List<GameObject>();
    }

    public List<GameObject> GetAllIntersectingPlanes()
    {
        return CurrentPlanes;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlatformingPlane")
        {
            CurrentPlanes.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlatformingPlane" && CurrentPlanes.Contains(other.gameObject))
        {
            CurrentPlanes.Remove(other.gameObject);
        }
    }
}

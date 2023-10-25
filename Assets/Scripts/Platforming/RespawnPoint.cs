using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private string RespawnName;
    [SerializeField] private bool Activated;
    [SerializeField] private Color ActivatedColor;

    private void Start()
    {
        SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
        if (st != null && st.CheckIfRespawnActivated(RespawnName))
        {
            ActivateRespawnPoint();
        }
    }

    public string GetRespawnName()
    {
        return RespawnName;
    }

    public void ActivateRespawnPoint()
    {
        Activated = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            GetComponent<SpriteRenderer>().color = ActivatedColor;
        }
        // GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            ActivateRespawnPoint();
            SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
            if (st != null) 
            {
                st.AddActivatedRespawn(RespawnName);
            }
        }
    }

    public bool CheckIfActivated() 
    {
        return Activated;
    }
}

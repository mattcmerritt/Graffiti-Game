using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private string RespawnName;

    public string GetRespawnName()
    {
        return RespawnName;
    }
}

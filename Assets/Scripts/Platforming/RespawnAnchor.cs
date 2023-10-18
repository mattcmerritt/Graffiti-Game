using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnAnchor : MonoBehaviour
{
    [SerializeField] private string AnchorName;

    public string GetAnchorName()
    {
        return AnchorName;
    }
}

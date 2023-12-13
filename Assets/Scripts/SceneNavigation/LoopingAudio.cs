using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudio : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

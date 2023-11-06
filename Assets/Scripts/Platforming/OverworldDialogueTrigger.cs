using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueLine Line;

    public event Action<DialogueLine> OnDialogueTriggered;

    [SerializeField] private bool SingleUse;
    private bool Used;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlatformingPlayer>() != null)
        {
            OnDialogueTriggered?.Invoke(Line);
        }
    }

    public void Complete()
    {
        Used = true;

        if (SingleUse)
        {
            Collider2D trigger = GetComponent<Collider2D>();
            trigger.enabled = false;
        }
    }
}

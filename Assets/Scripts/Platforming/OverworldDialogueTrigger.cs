using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueLine Line;

    public event Action<DialogueLine> OnDialogueTriggered;

    [SerializeField] private bool SingleUse;
    [SerializeField] private string DialogueTriggerName;
    private bool Used;

    private SceneTransitioner Transitioner;

    private void Start()
    {
        Transitioner = FindObjectOfType<SceneTransitioner>();

        OnDialogueTriggered += (DialogueLine line) => Complete();

        Used = Transitioner != null && Transitioner.CheckIfDialogueCheckpointActivated(DialogueTriggerName);

        if (Used && SingleUse)
        {
            Collider2D trigger = GetComponent<Collider2D>();
            trigger.enabled = false;
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlatformingPlayer>() != null)
        {
            OnDialogueTriggered?.Invoke(Line);
        }
    }

    private void Complete()
    {
        Used = true;
        Transitioner.AddActivatedDialogueCheckpoint(DialogueTriggerName);

        if (SingleUse)
        {
            Collider2D trigger = GetComponent<Collider2D>();
            trigger.enabled = false;
        }
    }
}

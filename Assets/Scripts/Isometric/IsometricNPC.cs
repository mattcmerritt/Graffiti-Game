using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricNPC : MonoBehaviour
{
    // Tracking the conversation progress
    private bool ConversationStarted, ConversationCompleted;

    // Useful if NPC has multiple trigger boxes
    [SerializeField] private bool InteractTriggerHit;
    [SerializeField] private Collider PreviousInteractor;

    public event Action<IsometricNPC> OnConversationComplete;

    // Dialogue Information
    private DialogueUI DialogueUI;
    [SerializeField] private DialogueLine StartingLine;

    private void OnTriggerEnter(Collider other)
    {
        // If NPC is intended to have multiple collision boxes, add && !InteractTriggerHit
        // and bring back commented lines
        if (other.CompareTag("Player Hitbox") && !ConversationStarted && !ConversationCompleted)
        {
            Interact();
            // Multiple collision box additions
            // InteractTriggerHit = true;
            // PreviousInteractor = other;
        }
    }

    // Mark that the interaction trigger is over once the collision has finished
    // Prevents issues with interactions being triggered multiple times by the same collider
    // Currently unused
    // private void OnTriggerExit(Collider other)
    // {
    //     if (InteractTriggerHit)
    //     {
    //         if (PreviousInteractor == other)
    //         {
    //             InteractTriggerHit = false;
    //         }
    //     } 
    // }

    public void AttachToDialogueUI(DialogueUI dialogueUI)
    {
        DialogueUI = dialogueUI;
        DialogueUI.OnConversationOver += CompleteInteraction;
    }

    private void Interact()
    {
        Debug.Log($"<color=yellow>Interactions: </color>{gameObject.name} NPC interacted with.");
        DialogueUI.StartConversation(StartingLine);
        ConversationStarted = true;
    }

    private void CompleteInteraction()
    {
        OnConversationComplete?.Invoke(this);
        ConversationCompleted = true;
    }
}

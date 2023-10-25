using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricNPC : MonoBehaviour
{
    [SerializeField] private bool InteractTriggerHit;
    [SerializeField] private Collider PreviousInteractor;

    public event Action<IsometricNPC> OnConversationComplete;

    private void OnTriggerEnter(Collider other)
    {
        // If NPC is intended to have multiple collision boxes, add && !InteractTriggerHit
        // and bring back commented lines
        if (other.CompareTag("Player Hitbox"))
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

    private void Interact()
    {
        Debug.Log($"<color=yellow>Interactions: </color>{gameObject.name} NPC interacted with.");

        // TODO: move to a separate part of the program after the dialogue is completed
        CompleteInteraction();
    }

    private void CompleteInteraction()
    {
        OnConversationComplete?.Invoke(this);
    }
}

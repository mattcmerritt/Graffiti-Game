using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricEncounter : MonoBehaviour
{
    // Stage data
    [SerializeField] private bool IsCombatEncounter;
    private int RemainingEnemies;
    private bool FinishedConversation;
    private float PlayerHealth;

    // Elements
    private IsometricSimpleEnemy[] Enemies;
    private IsometricNPC NPC;
    private SceneTransitioner Transitioner;
    private IsometricPlayer Player;

    // Singleton instance to allow player to read critical information
    public static IsometricEncounter Instance;

    // Events that other classes can use to track changes without hunting for specific objects in scene
    public event Action<float> OnPlayerHealthChanged;

    // Configure singleton instance at earliest step
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Transitioner = FindObjectOfType<SceneTransitioner>();
        Enemies = FindObjectsOfType<IsometricSimpleEnemy>();
        NPC = FindObjectOfType<IsometricNPC>();
        Player = FindObjectOfType<IsometricPlayer>();

        RemainingEnemies = Enemies.Length;
        PlayerHealth = Player.GetHealth();

        Player.OnHealthChange += TrackPlayerHealth;

        foreach (IsometricSimpleEnemy enemy in Enemies)
        {
            enemy.OnDefeat += TrackEnemyDefeat;
        }

        if (NPC != null)
        {
            NPC.OnConversationComplete += TrackNPCComplete;
        }
    }

    private void Update()
    {
        if (IsCombatEncounter && RemainingEnemies <= 0)
        {
            StartCoroutine(Transitioner.ReturnToCity(true));
        }
        
        if (!IsCombatEncounter && FinishedConversation)
        {
            StartCoroutine(Transitioner.ReturnToCity(true));
        }

        if (PlayerHealth <= 0)
        {
            StartCoroutine(Transitioner.ReturnToCity(false));
        }
    }

    public bool CheckIsCombatEncounter()
    {
        return IsCombatEncounter;
    }

    private void TrackPlayerHealth(float newHealth)
    {
        PlayerHealth = newHealth;
        Debug.Log($"<color=red>Encounter Progress: </color>Player health has been changed to {PlayerHealth}.");
        OnPlayerHealthChanged?.Invoke(PlayerHealth);
    }

    private void TrackEnemyDefeat(IsometricSimpleEnemy enemy)
    {
        RemainingEnemies--;
        Debug.Log($"<color=red>Encounter Progress: </color>An enemy ({enemy.name}) was defeated. {RemainingEnemies} enemies remain.");
    }

    private void TrackNPCComplete(IsometricNPC npc)
    {
        FinishedConversation = true;
        Debug.Log($"<color=red>Encounter Progress: </color>Conversation with the NPC ({npc.name}) has concluded.");
    }
}

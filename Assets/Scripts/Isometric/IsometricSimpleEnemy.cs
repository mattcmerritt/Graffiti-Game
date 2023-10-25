using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricSimpleEnemy : MonoBehaviour
{
    // Components and variables
    [SerializeField] private GameObject Player;
    [SerializeField] private Rigidbody Rigidbody;

    // State information for enemy attacks
    [SerializeField] private bool Preparing, Charging, Retreating;
    [SerializeField] private float PrepDuration, ChargeDuration, RetreatDuration;
    [SerializeField] private float CurrentPrepTimer, CurrentChargeTimer, CurrentRetreatTimer;
    [SerializeField] private Vector3 ChargeDirection;
    [SerializeField] private float MoveSpeed, ChargeSpeed, RetreatSpeed;

    private void Start()
    {
        Player = FindObjectOfType<IsometricPlayer>().gameObject;
    }

    private void Update()
    {
        // If preparing a charge, sit still until the duration is over, then charge.
        if (Preparing)
        {
            CurrentPrepTimer += Time.deltaTime;
            if (CurrentPrepTimer >= PrepDuration)
            {
                Preparing = false;
                Charging = true;

                ChargeDirection = (Player.transform.position - transform.position).normalized;
                Rigidbody.velocity = ChargeDirection * ChargeSpeed;
            }
        }
        // If charging, keep track of how long the charge has been going for and start retreating at end
        else if (Charging)
        {
            CurrentChargeTimer += Time.deltaTime;
            if (CurrentChargeTimer >= ChargeDuration)
            {
                Charging = false;
                Retreating = true;

                CurrentChargeTimer = 0;
            }
        }
        // If retreating, run away from the player for a bit
        else if (Retreating)
        {
            CurrentRetreatTimer += Time.deltaTime;
            Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
            Rigidbody.velocity = directionToPlayer * RetreatSpeed * -1;
            if (CurrentRetreatTimer >= RetreatDuration)
            {
                Retreating = false;
                CurrentRetreatTimer = 0;
            }
        }
        // Otherwise, keep trying to get close to the player
        else
        {
            Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
            Rigidbody.velocity = directionToPlayer * MoveSpeed;
        }
    }

    // If close to player, begin preparing a charge.
    private void OnTriggerEnter(Collider other)
    {
        // If the player swing hits an enemy, it dies
        if (other.CompareTag("Player Hitbox"))
        {
            Destroy(gameObject);
        }
        if (other.GetComponent<IsometricPlayer>() != null)
        {
            StartPreparing();
        }
    }

    // Allow enemies to re-aggro if they miss their charge but stay in the radius of the player.
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IsometricPlayer>() != null)
        {
            if (!Charging && !Retreating && !Preparing)
            {
                StartPreparing();
            }
        }
    }

    // If player gets too far, stop preparing a charge
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IsometricPlayer>() != null)
        {
            Charging = false;
            CurrentChargeTimer = 0;
            Preparing = false;
            CurrentPrepTimer = 0;
        }
    }

    // If colliding with a player, register a hit.
    private void OnCollisionEnter(Collision collision)
    {
        IsometricPlayer player = collision.gameObject.GetComponent<IsometricPlayer>();
        if (player != null)
        {
            bool playerDashing = player.GetDashActive();
            // If the player was dashing, the enemy is defeated
            if (playerDashing)
            {
                Destroy(gameObject);
            }
            // If the player is not dashing and the enemy charges at them, it hurts the player
            else if (Charging && !playerDashing)
            {
                player.HurtPlayer();
            }
        }
    }

    private void StartPreparing()
    {
        Preparing = true;
        CurrentPrepTimer = 0;
    }
}
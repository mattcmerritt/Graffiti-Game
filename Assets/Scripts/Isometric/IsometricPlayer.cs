using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayer : MonoBehaviour
{
    // Components and variables
    [SerializeField, Range(0, 50)] private float MoveSpeed = 5f, DashForce;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private int Lives = 3;

    // Internal dash state tracking information
    [SerializeField] private bool DashEnabled, DashAvailable, DashActive;
    [SerializeField] private float DashCooldown, DashDuration;
    [SerializeField] private float MaxDashCooldown, MaxDashDuration;

    // Debug, change player color to reflect state
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Vector3 CorrectionRatio;

    private void Update()
    {
        // Movement input vectors
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 normalizedInput = Vector3.Normalize(new Vector3(horizontalInput, verticalInput, 0));
        Vector3 input = normalizedInput.x * new Vector3(1, 0, -1) * CorrectionRatio.x + normalizedInput.y * new Vector3(1, 0, 1) * CorrectionRatio.y;

        // Dash functionality
        if (DashEnabled && DashAvailable && Input.GetKey(KeyCode.Space))
        {
            // Debug.Log("Dashed!");
            SpriteRenderer.color = Color.green;
            Rigidbody.velocity = DashForce * input;
            DashActive = true; // activate the dash state
            DashAvailable = false; // start cooldown
            DashCooldown = 0f;
            DashDuration = 0f;
        }
        
        // Dash cooldown management
        if (!DashActive)
        {
            // If not currently dashing, allow player to move freely
            Rigidbody.velocity = input * MoveSpeed;

            // Additionally, if the dash in on cooldown, track this
            if (!DashAvailable)
            {
                DashCooldown += Time.deltaTime;
                if (DashCooldown >= MaxDashCooldown)
                {
                    // Debug.Log("Dash cooldown is over!");
                    SpriteRenderer.color = Color.black;
                    DashAvailable = true;
                }
            }
        }
        else
        {
            // Otherwise, track the dash duration and keep the player's speed constant
            DashDuration += Time.deltaTime;
            if (DashDuration >= MaxDashDuration)
            {
                // Debug.Log("Dash is over!");
                SpriteRenderer.color = Color.blue;
                DashActive = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (DashActive)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public bool GetDashActive()
    {
        return DashActive;
    }

    public void HurtPlayer()
    {
        SpriteRenderer.color = new Color(1, 0.5f, 0);
        Lives--;
    }

    public int GetLives()
    {
        return Lives;
    }
}

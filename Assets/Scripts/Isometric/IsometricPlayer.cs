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

    // Information related to hitstun and invincibility
    [SerializeField] private float HitDelay, MaxHitDelay;
    [SerializeField] private bool HitRecently;

    // Attack state information
    [SerializeField] private GameObject AttackVisual;
    [SerializeField] private Vector3 Direction;
    [SerializeField] private bool AttackActive;
    [SerializeField] private float AttackDuration, MaxAttackDuration;

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
        if (DashEnabled && DashAvailable && Input.GetKey(KeyCode.LeftShift))
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

        // Health management and hitstun/i-frames
        if (HitRecently)
        {
            HitDelay -= Time.deltaTime;
            if (HitDelay <= 0f)
            {
                HitDelay = 0f;
                SpriteRenderer.color = Color.black;
                HitRecently = false;
            }
        }

        Direction = new Vector3(horizontalInput, verticalInput, 0f);
        // Simple attack animation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO: Reimplement with a function
            Vector3 NE = Vector3.right + Vector3.up;
            Vector3 NW = Vector3.left + Vector3.up;
            Vector3 SE = Vector3.right + Vector3.down;
            Vector3 SW = Vector3.left + Vector3.down;
            float rotation = 0f;

            if (Direction == NW)
            {
                rotation = 0;
            }
            else if (Direction == Vector3.up)
            {
                rotation = 45;
            }
            else if (Direction == NE)
            {
                rotation = 90;
            }
            else if (Direction == Vector3.right)
            {
                rotation = 135;
            }
            else if (Direction == SE)
            {
                rotation = 180;
            }
            else if (Direction == Vector3.down)
            {
                rotation = 225;
            }
            else if (Direction == SW)
            {
                rotation = 270;
            }
            else if (Direction == Vector3.left)
            {
                rotation = 315;
            }
            
            
            AttackVisual.transform.eulerAngles = new Vector3(90f, rotation, 0f);
            AttackActive = true;
            AttackDuration = 0f;
            AttackVisual.SetActive(true);
        }

        // Attack disappears after duration
        if (AttackActive)
        {
            AttackDuration += Time.deltaTime;
            if (AttackDuration >= MaxAttackDuration)
            {
                AttackActive = false;
                AttackVisual.SetActive(false);
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
        if (!HitRecently)
        {
            HitRecently = true;
            HitDelay = MaxHitDelay;
            SpriteRenderer.color = new Color(1, 0.5f, 0);
            Lives--;
        }
    }

    public int GetLives()
    {
        return Lives;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayer : MonoBehaviour
{
    // Components and variables
    [SerializeField, Range(0, 50)] private float MoveSpeed = 5f, DashForce;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private float Health = 100, MaxHealth = 100;
    private bool IsCombatEncounter;
    private Vector3 Direction;

    // Internal dash state tracking information
    [SerializeField] private bool DashEnabled;
    private bool DashAvailable, DashActive;
    private float DashCooldown, DashDuration;
    [SerializeField] private float MaxDashCooldown, MaxDashDuration;

    // Information related to hitstun and invincibility
    private float HitDelay;
    [SerializeField] private float MaxHitDelay;
    private bool HitRecently;

    // Attack state information
    private bool AttackActive;
    [SerializeField] private GameObject AttackVisual;
    private float AttackDuration;
    [SerializeField] private float MaxAttackDuration;

    // Interaction detection boxes
    [SerializeField] private GameObject InteractCollider;
    private bool InteractionActive;

    // Debug, change player color to reflect state
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Vector3 CorrectionRatio;

    // Event data so that other classes can determine when the player health has changed
    public event Action<float> OnHealthChange;

    // Data for pausing all input and movement when paused
    private bool Paused;
    private Vector3 PreviousVelocity;

    // Events for tracking cooldowns, passes the cooldown duration
    public event Action<float> OnDash;

    // Directional sprites
    [SerializeField] private List<Sprite> DirectionalPlayerSprites;

    // Animation
    [SerializeField] private Animator Animator;
    [SerializeField] private string WalkDirection = "Right";

    // Load encounter specific information from the level data
    private void Start()
    {
        IsCombatEncounter = IsometricEncounter.Instance.CheckIsCombatEncounter();

        IsometricEncounter.Instance.OnPause += Pause;
        IsometricEncounter.Instance.OnResume += Resume;
    }

    private void Update()
    {
        if (!Paused)
        {
            // Movement input vectors
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 normalizedInput = Vector3.Normalize(new Vector3(horizontalInput, verticalInput, 0));
            Vector3 input = normalizedInput.x * new Vector3(1, 0, -1) * CorrectionRatio.x + normalizedInput.y * new Vector3(1, 0, 1) * CorrectionRatio.y;

            // Animation
            if (horizontalInput > 0)
            {
                if (verticalInput > 0)
                {
                    Animator.Play("WalkUpRight");
                    WalkDirection = "UpRight";
                }
                else if (verticalInput < 0)
                {
                    Animator.Play("WalkDownRight");
                    WalkDirection = "DownRight";
                }
                else
                {
                    Animator.Play("WalkRight");
                    WalkDirection = "Right";
                }
            }
            else if (horizontalInput < 0)
            {
                if (verticalInput > 0)
                {
                    Animator.Play("WalkUpLeft");
                    WalkDirection = "UpLeft";
                }
                else if (verticalInput < 0)
                {
                    Animator.Play("WalkDownLeft");
                    WalkDirection = "DownLeft";
                }
                else
                {
                    Animator.Play("WalkLeft");
                    WalkDirection = "Left";
                }
            }
            else if (verticalInput > 0 && horizontalInput == 0f)
            {
                Animator.Play("WalkUp");
                WalkDirection = "Up";
            }
            else if (verticalInput < 0 && horizontalInput == 0f)
            {
                Animator.Play("WalkDown");
                WalkDirection = "Down";
            }
            else
            {
                Animator.Play("Idle" + WalkDirection);
            }

            // Dash functionality
            if (DashEnabled && DashAvailable && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Z)))
            {
                // check for buddy and buddy cooldown
                DashBuddy buddy = BuddyManager.Instance.GetBuddy<DashBuddy>();
                if (buddy != null && buddy.CanDash())
                {
                    // Debug.Log("Dashed!");
                    // SpriteRenderer.color = Color.green;
                    Animator.Play("Dash" + WalkDirection);
                    Rigidbody.velocity = DashForce * input;
                    DashActive = true; // activate the dash state
                    DashAvailable = false; // start cooldown
                    DashCooldown = 0f;
                    DashDuration = 0f;
                    OnDash?.Invoke(MaxDashCooldown);
                }
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
                        // SpriteRenderer.color = Color.white;
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
                    // SpriteRenderer.color = Color.blue;
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
                    SpriteRenderer.color = Color.white;
                    HitRecently = false;
                }
            }

            Direction = new Vector3(horizontalInput, verticalInput, 0f);
            float rotation = CalculateRotation();

            // Simple attack animation
            if (Input.GetKeyDown(KeyCode.Space) && IsCombatEncounter)
            {
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

            // If the player is not in a combat stage, space becomes interact instead of attack
            if (Input.GetKey(KeyCode.Space) && !IsCombatEncounter)
            {
                InteractCollider.transform.eulerAngles = new Vector3(90f, rotation, 0f);
                InteractCollider.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Space) && !IsCombatEncounter)
            {
                InteractCollider.SetActive(false);
            }
        } 
        else
        {
            Rigidbody.velocity = Vector3.zero;
            Animator.Play("Idle" + WalkDirection);
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

    public void HurtPlayer(float healthLost)
    {
        if (!HitRecently)
        {
            HitRecently = true;
            HitDelay = MaxHitDelay;
            SpriteRenderer.color = new Color(1, 0.5f, 0);
            Health -= healthLost;

            OnHealthChange?.Invoke(GetHealthPercentage());
        }
    }

    public float GetHealthPercentage()
    {
        return Health / MaxHealth; 
    }

    private float CalculateRotation()
    {
        Vector3 NE = Vector3.right + Vector3.up;
        Vector3 NW = Vector3.left + Vector3.up;
        Vector3 SE = Vector3.right + Vector3.down;
        Vector3 SW = Vector3.left + Vector3.down;
        float rotation = 0f;

        if (Direction == NW)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[3];
            rotation = 0;
        }
        else if (Direction == Vector3.up)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[2];
            rotation = 45;
        }
        else if (Direction == NE)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[1];
            rotation = 90;
        }
        else if (Direction == Vector3.right)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[0];
            rotation = 135;
        }
        else if (Direction == SE)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[7];
            rotation = 180;
        }
        else if (Direction == Vector3.down)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[6];
            rotation = 225;
        }
        else if (Direction == SW)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[5];
            rotation = 270;
        }
        else if (Direction == Vector3.left)
        {
            SpriteRenderer.sprite = DirectionalPlayerSprites[4];
            rotation = 315;
        }
        return rotation;
    }

    private void Pause()
    {
        Paused = true;
        PreviousVelocity = Rigidbody.velocity;
    }

    private void Resume()
    {
        Paused = false;
        Rigidbody.velocity = PreviousVelocity;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformingPlayer : MonoBehaviour
{
    // player events
    public event Action<float> OnDash;
    public event Action OnDoubleJump;
    public event Action OnDoubleJumpRefresh;

    // player event trackers (should the event be active yet?)
    [SerializeField] private bool DashObtained, DoubleJumpObtained;

    // adjustable player properties (base values, like respawn time and movement speed)
    [SerializeField] private float TopHorizSpeed;
    [SerializeField] private float JumpForce, DashForce;
    [SerializeField] private float DashCooldown, DashDuration;
    [SerializeField] private Color PlayerColor, CooldownColor, DashBuddyColor, ImmobileColor;
    [SerializeField] private float OutOfBoundsTimer;

    // internal player values (usually used in runtime for cooldowns and stuff)
    private float CurrentDashCooldown, CurrentDashDuration;
    private bool DashActive;
    private float InitialGravity;
    private string DashDirection;
    private float TimeSpentOutOfBounds;
    private bool DoubleJumpUsed;
    private bool Paused;
    private Vector2 PreviousVelocity;
    private float PreviousGravity;

    // player components
    private Rigidbody2D Rb;
    private GroundChecker GroundCheck;
    private SpriteRenderer Renderer;
    private PlaneChecker PlaneCheck;
    [SerializeField] private GameObject RespawnMessage; // TODO: maybe a better way to ddo this than in editor?
    private PlatformingLevel PlatformingLevel;

    // TODO: OnEnable and OnDisable should change eventually to support being active and inactive
    private void OnEnable() 
    {
        OnDash += StartDash;
        OnDoubleJump += StartDoubleJump;
        OnDoubleJumpRefresh += RefreshDoubleJump;
    }

    private void OnDisable() 
    {
        OnDash -= StartDash;
        OnDoubleJump -= StartDoubleJump;
        OnDoubleJumpRefresh -= RefreshDoubleJump;
    }

    private void Start() 
    {
        Rb = GetComponent<Rigidbody2D>();
        GroundCheck = GetComponentInChildren<GroundChecker>();
        PlaneCheck = GetComponentInChildren<PlaneChecker>();
        Renderer = GetComponent<SpriteRenderer>();
        InitialGravity = Rb.gravityScale; // set gravity to initial value
        RespawnMessage.SetActive(false);
        PlatformingLevel = PlatformingLevel.Instance;

        PlatformingLevel.Instance.OnPause += Pause;
        PlatformingLevel.Instance.OnResume += Resume;
    }

    private void Update() 
    {
        if(!Paused)
        {
            List<GameObject> intersectedPlanes = PlaneCheck.GetAllIntersectingPlanes();
            SnapPlayerToPlatformingPanels(intersectedPlanes);

            // *** CONTROLS ***
            // TODO: move this to a separate controls manager

            if (intersectedPlanes.Count > 0)
            {
                // check for dash
                if((Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.Z))) && CurrentDashCooldown <= 0) 
                {
                    OnDash?.Invoke(DashCooldown);
                }

                // update last direction moved in to store for dash
                if(Input.GetAxisRaw("Horizontal") != 0)
                {
                    DashDirection = (Input.GetAxisRaw("Horizontal") > 0) ? "right" : "left"; // determine direction the player is moving in
                }

                // check for jump inputs (W / up arrow handled in FixedUpdate)
                if(GroundCheck == null) 
                {
                    Debug.LogError("The player has no GroundChecker to use. Please add one.");
                }
                else if(Input.GetKeyDown(KeyCode.Space) && GroundCheck.CheckIfGrounded())
                {
                    Rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                }
                // double jump with space or w
                else if(!GroundCheck.CheckIfGrounded() && DoubleJumpObtained && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !DoubleJumpUsed)
                {
                    OnDoubleJump?.Invoke();
                }
            }

            // respawn player if R is pressed
            if(Input.GetKeyDown(KeyCode.R)) 
            {
                RespawnPlayerAtNearest();
            }

            UpdatePlayerStateVariables(intersectedPlanes);

            // grab camera for following
            Camera.main.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.gameObject.transform.position.z);
        }
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!Paused) 
        {
            if(DashActive) 
            {
                if(CurrentDashDuration <= 0) 
                {
                    Rb.gravityScale = InitialGravity;
                    DashActive = false;
                }
            }
            else if(PlaneCheck.GetAllIntersectingPlanes().Count == 0) 
            {
                // controls for when not on a surface (free fall)
                // TODO: determine with team what should go in here
            }
            else 
            {
                // TODO: depending on new controls system, this might all need to be redone

                // horizontal controls when on a surface
                if(Input.GetAxisRaw("Horizontal") != 0) 
                {
                    float PlayerHorizSpeed = 0;
                    if(Input.GetAxisRaw("Horizontal") + Rb.velocity.x < 0) 
                    {
                        PlayerHorizSpeed = Mathf.Max(Input.GetAxisRaw("Horizontal") + Rb.velocity.x, -TopHorizSpeed);
                    }
                    else 
                    {
                        PlayerHorizSpeed = Mathf.Min(Input.GetAxisRaw("Horizontal") + Rb.velocity.x, TopHorizSpeed);
                    }
                    
                    // PlayerVelocity = new Vector2(PlayerHorizSpeed, Rb.velocity.y);
                    // Rb.velocity = PlayerVelocity;
                    Rb.velocity = new Vector2(PlayerHorizSpeed, Rb.velocity.y);
                }
                else 
                {
                    // stop on a dime
                    Rb.velocity = new Vector2(0, Rb.velocity.y);
                }

                // vertical controls when on a surface (space handled in Update)
                if(GroundCheck == null) 
                {
                    Debug.LogError("The player has no GroundChecker to use. Please add one.");
                }
                else if(Input.GetAxisRaw("Vertical") > 0 && GroundCheck.CheckIfGrounded())
                {
                    Rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void StartDash(float cooldown) 
    {
        DashActive = true;
        CurrentDashCooldown = DashCooldown;
        CurrentDashDuration = DashDuration;

        // change player color
        Renderer.color = DashBuddyColor;

        // controls for when dashing
            
        // prevent y value of player from falling
        // option 1 - lock y position - won't play nice with slopes and will cause player to bonk often
        // option 2 - disable gravity - might cause issues if other things interfere, enemies would need to be triggers for example - do this for now
        Rb.gravityScale = 0f;
        Rb.velocity = Vector2.zero;

        // determine dash direction
        if(DashDirection == "left")
        {
            Rb.AddForce(transform.right * -DashForce, ForceMode2D.Impulse);
        }
        else 
        {
            Rb.AddForce(transform.right * DashForce, ForceMode2D.Impulse);
        }
    }

    private void StartDoubleJump()
    {
        DoubleJumpUsed = true;
        Rb.velocity = Vector3.zero; // reset momentum so the player actually gets some jumping force
        Rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    private void RefreshDoubleJump()
    {
        DoubleJumpUsed = false;
    }

    private void SnapPlayerToPlatformingPanels(List<GameObject> intersectedPlanes)
    {
        // move player to front of the closest surface
        if(intersectedPlanes.Count > 0)
        {
            GameObject closestPlane = intersectedPlanes[0];
            // TODO: this won't work if planes are behind the camera, so don't do that
            float minDistanceFromCamera = Mathf.Abs(Camera.main.gameObject.transform.position.z - closestPlane.transform.position.z);
            foreach (GameObject plane in intersectedPlanes)
            {
                float distFromCamera = Mathf.Abs(Camera.main.gameObject.transform.position.z - plane.transform.position.z);
                if(distFromCamera < minDistanceFromCamera)
                {
                    closestPlane = plane;
                    minDistanceFromCamera = distFromCamera;
                }
            }

            // move to plane's z position without changing x or y to put player in front of plane
            transform.position = new Vector3(transform.position.x, transform.position.y, closestPlane.transform.position.z);
        }
    }

    // updates the state variables that the player uses to determine if moves are valid or what to do
    private void UpdatePlayerStateVariables(List<GameObject> intersectedPlanes)
    {
        // handle cooldowns and colors
        if(CurrentDashDuration > 0)
        {
            // dashing
            Renderer.color = DashBuddyColor;
            CurrentDashCooldown -= Time.deltaTime;
            CurrentDashDuration -= Time.deltaTime;
        }
        else if(intersectedPlanes.Count == 0)
        {
            // not on a plane - can't move
            Renderer.color = ImmobileColor; 
            CurrentDashCooldown -= Time.deltaTime;
        }
        else if(CurrentDashCooldown > 0)
        {
            // could dash, but on cooldown
            Renderer.color = CooldownColor;
            CurrentDashCooldown -= Time.deltaTime;
        }
        else
        {
            // normal, can dash
            Renderer.color = PlayerColor;
        }

        // display respawn text if player in fail state
        if (intersectedPlanes.Count == 0)
        {
            TimeSpentOutOfBounds += Time.deltaTime;
            if(TimeSpentOutOfBounds >= OutOfBoundsTimer) 
            {
                // player might be falling out of bounds
                RespawnMessage.SetActive(true);
            }
            else if(Rb.velocity == Vector2.zero)
            {
                // player has no momentum and will remain stuck
                RespawnMessage.SetActive(true);
            }
        }
        else
        {
            TimeSpentOutOfBounds = 0;
            RespawnMessage.SetActive(false);
        }

        // double jump reset on touching ground
        if(DoubleJumpUsed && GroundCheck.CheckIfGrounded())
        {
            OnDoubleJumpRefresh?.Invoke();
        }
    }

    private void RespawnPlayerAtNearest()
    {
        RespawnPoint[] respawnPoints = FindObjectsOfType<RespawnPoint>();
        List<GameObject> respawnObjects = new List<GameObject>();
        foreach (RespawnPoint r in respawnPoints)
        {
            respawnObjects.Add(r.gameObject);
        }
        if(respawnObjects.Count > 0)
        {
            GameObject closestRespawn = null;
            float minDistanceFromRespawn = Int32.MaxValue; // large so it will be overwritten 
            foreach (GameObject respawn in respawnObjects)
            {
                float distFromRespawn = Vector3.Distance(transform.position, respawn.transform.position);
                if(respawn.GetComponent<RespawnPoint>().CheckIfActivated() && distFromRespawn < minDistanceFromRespawn)
                {
                    closestRespawn = respawn;
                    minDistanceFromRespawn = distFromRespawn;
                }
            }
            
            if(closestRespawn != null) 
            {
                transform.position = closestRespawn.transform.position;
                RespawnMessage.SetActive(false);
            }
            else
            {
                Debug.LogError("ERROR: No active respawn points found to place the player.");
            }
        }
        else 
        {
            Debug.LogError("No respawn points found! Add objects with the RespawnPoint script.");
        }
        
    }

    private void Pause()
    {
        Paused = true;

        PreviousVelocity = Rb.velocity;
        PreviousGravity = Rb.gravityScale;

        Rb.velocity = Vector2.zero;
        Rb.gravityScale = 0f;
    }

    private void Resume()
    {
        Paused = false;

        Rb.velocity = PreviousVelocity;
        Rb.gravityScale = PreviousGravity;
    }
}

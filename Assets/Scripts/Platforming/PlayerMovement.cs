using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float TopHorizSpeed;
    [SerializeField] private float JumpForce, DashForce;
    [SerializeField] private float DashCooldown,CurrentDashCooldown;
    [SerializeField] private float DashDuration, CurrentDashDuration;
    [SerializeField] private bool DashActive, FreeFall;
    [SerializeField] private float InitialGravity;
    [SerializeField] private Vector2 PlayerVelocity;
    [SerializeField] private GroundChecker GroundCheck;
    [SerializeField] private string DashDirection;
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private Color PlayerColor, CooldownColor, DashBuddyColor;
    public static event Action Dash;

    private void OnEnable() 
    {
        Dash += StartDash;
    }

    private void OnDisable() 
    {
        Dash -= StartDash;
    }

    private void Start() 
    {
        Rb = GetComponent<Rigidbody2D>();
        GroundCheck = GetComponentInChildren<GroundChecker>();
        Renderer = GetComponent<SpriteRenderer>();
        Rb.gravityScale = InitialGravity; // set gravity to initial value
    }

    private void Update() 
    {
        if(Input.GetKey(KeyCode.Space) && CurrentDashCooldown <= 0) 
        {
            Dash.Invoke();
        }

        // update last direction moved in to store for dash
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            DashDirection = (Input.GetAxisRaw("Horizontal") > 0) ? "right" : "left"; // determine direction the player is moving in
        }

        if(CurrentDashDuration > 0)
        {
            Renderer.color = DashBuddyColor;
            CurrentDashCooldown -= Time.deltaTime;
            CurrentDashDuration -= Time.deltaTime;
        }
        else if(CurrentDashCooldown > 0)
        {
            Renderer.color = CooldownColor;
            CurrentDashCooldown -= Time.deltaTime;
        }
        else
        {
            Renderer.color = PlayerColor;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(DashActive) 
        {
            if(CurrentDashDuration <= 0) 
            {
                Rb.gravityScale = InitialGravity;
                DashActive = false;
            }
        }
        else if(FreeFall) 
        {
            // controls for when not on a surface (free fall)
        }
        else 
        {
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
                
                PlayerVelocity = new Vector2(PlayerHorizSpeed, Rb.velocity.y);
                Rb.velocity = PlayerVelocity;
            }
            else 
            {
                Rb.velocity = new Vector2(0, Rb.velocity.y);
                // TODO: if you want the player to slide rather than stop on a dime, add deceleration here

                // TODO: check player bounds, and position them back in the wall if they are not in it
            }
            
            // vertical controls when on a surface
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

    private void StartDash() 
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float TopHorizSpeed, JumpForce;
    [SerializeField] private bool DashActive, FreeFall;
    public static event Action Dash;
    [SerializeField] private Vector2 PlayerVelocity;
    [SerializeField] private GroundChecker GroundCheck;

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
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Dash.Invoke();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(DashActive) 
        {
            // controls for when dashing
            
            // prevent y value of player from falling
            // option 1 - lock y position - won't play nice with slopes and will cause player to bonk often
            // option 2 - disable gravity - might cause issues if other things interfere, enemies would need to be triggers for example
        }
        else if(FreeFall) 
        {
            // controls for when not on a surface (free fall)
        }
        else {
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
    }
}

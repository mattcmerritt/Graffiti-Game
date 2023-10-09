using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float TopHorizSpeed;
    [SerializeField] private bool DashActive;
    public static event Action Dash;
    [SerializeField] private Vector2 PlayerVelocity;

    private void OnEnable() {
        Dash += StartDash;
    }

    private void OnDisable() {
        Dash -= StartDash;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Dash.Invoke();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(DashActive) {

        }
        else {
            if(Input.GetAxisRaw("Horizontal") != 0) {
                float PlayerHorizSpeed = 0;
                if(Input.GetAxisRaw("Horizontal") + Rb.velocity.x < 0) {
                    PlayerHorizSpeed = Mathf.Max(Input.GetAxisRaw("Horizontal") + Rb.velocity.x, -TopHorizSpeed);
                }
                else {
                    PlayerHorizSpeed = Mathf.Min(Input.GetAxisRaw("Horizontal") + Rb.velocity.x, TopHorizSpeed);
                }
                
                PlayerVelocity = new Vector2(PlayerHorizSpeed, Rb.velocity.y);
                Rb.velocity = PlayerVelocity;
            }
            else {
                Rb.velocity = new Vector2(0, Rb.velocity.y);
                // TODO: if you want the player to slide rather than stop on a dime, add deceleration here
            }
            
        }
    }

    private void StartDash() {
        DashActive = true;
    }
}

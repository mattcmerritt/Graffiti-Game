using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRaycastGroundChecker : GroundChecker
{
    [SerializeField] private float CheckDistance = 0.05f;
    private int GroundLayer = 6;
    private int GroundLayerMask;

    private void Awake() 
    {
        GroundLayerMask = 1 << GroundLayer;
    }

    public override bool CheckIfGrounded() 
    {
        bool InVerticalMotion = Mathf.Abs(GetComponentInParent<Rigidbody2D>().velocity.y) >= 0.01f; // check if stopped
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, CheckDistance, GroundLayerMask); // check if ground below
        return hit.collider != null && !InVerticalMotion;
    }

    public override bool CheckIfGroundStable()
    {
        // The point of this is to check if the player should be moving, to make them not slide down slopes

        // TODO: for now, this just always returns true
        // in the future, this should check if the ground is a moving platform or solid ground
        // if the ground is "stable" (it won't fall, move, disappear, or be broken), return true
        // else return false
        
        return true;
    }
}

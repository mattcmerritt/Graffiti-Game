using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGroundChecker : GroundChecker
{
    [SerializeField] private bool IsGrounded = false;

    public override bool CheckIfGrounded() 
    {
        return IsGrounded;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
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

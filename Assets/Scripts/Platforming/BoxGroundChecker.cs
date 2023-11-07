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
}

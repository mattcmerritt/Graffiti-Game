using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRaycastGroundChecker : GroundChecker
{
    [SerializeField] private List<GroundChecker> GroundCheckers;

    public override bool CheckIfGrounded() 
    {
        foreach(GroundChecker gc in GroundCheckers)
        {
            if(gc.CheckIfGrounded())
            {
                return true;
            }
        }
        return false;
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

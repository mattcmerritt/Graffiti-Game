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
}

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, CheckDistance, GroundLayerMask);
        return hit.collider != null;
    }
}

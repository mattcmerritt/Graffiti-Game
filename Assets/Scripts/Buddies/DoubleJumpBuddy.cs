using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpBuddy : Buddy
{
    // Constants to set milestones
    [SerializeField] private int DoubleJumpMilestone;
    [SerializeField] private int ChargedJumpMilestone;
    [SerializeField] private int TripleJumpMilestone;

    public bool CanDoubleJump()
    {
        return CurrentMilestone <= DoubleJumpMilestone;
    }

    public bool CanChargedJump()
    {
        return CurrentMilestone <= ChargedJumpMilestone;
    }

    public bool CanTripleJump()
    {
        return CurrentMilestone <= TripleJumpMilestone;
    }
}

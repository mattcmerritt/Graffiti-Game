using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBuddy : Buddy
{
    // Constants to set milestones
    [SerializeField] private int DashMilestone;

    public bool CanDash()
    {
        return CurrentMilestone <= DashMilestone;
    }
}

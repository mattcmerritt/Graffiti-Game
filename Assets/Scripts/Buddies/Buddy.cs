using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buddy : MonoBehaviour
{
    [SerializeField] private bool Active;
    [SerializeField] private int Progress;
    [SerializeField] private List<int> Milestones;
    [SerializeField] protected int CurrentMilestone;

    public void UpdateProgress(int value)
    {
        Progress += value;

        // updating progression with milestones
        for (int i = CurrentMilestone; i < Milestones.Count; i++)
        {
            if (Milestones[i] <= Progress)
            {
                CurrentMilestone++;
            }
            else
            {
                break;
            }
        }
    }

    public bool CheckBuddyActive()
    {
        return Active;
    }
}

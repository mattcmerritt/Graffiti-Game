using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyManager : MonoBehaviour
{
    [SerializeField] private List<Buddy> ActiveBuddies;

    public static BuddyManager Instance;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public B GetBuddy<B>() where B : Buddy
    {
        B buddy = (B) (ActiveBuddies.Find((Buddy b) => b is B));
        return buddy;
    }

    // TODO: find a way to use this meaningfully
    public void AddBuddy<B>() where B : Buddy
    {
        Buddy newBuddy = gameObject.AddComponent<B>();
        ActiveBuddies.Add(newBuddy);
    }

    // TODO: find a way to implement this without knowing the buddy reference
    private void RemoveBuddy<B>() where B : Buddy
    {
        Buddy buddy = ActiveBuddies.Find((Buddy b) => b is B);
        ActiveBuddies.Remove(buddy);
    }
}

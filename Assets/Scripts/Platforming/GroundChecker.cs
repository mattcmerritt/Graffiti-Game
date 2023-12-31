using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundChecker : MonoBehaviour
{
    public abstract bool CheckIfGrounded();

    public abstract bool CheckIfGroundStable();
}
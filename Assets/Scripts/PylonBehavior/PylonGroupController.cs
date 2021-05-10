using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonGroupController : MonoBehaviour
{
    [SerializeField] private GameObject doorToUnlock;
    [SerializeField] private Pylon[] pylonsInGroup;

    public void OnPylonActivated()
    {
        // Check if each pylon is activated
        foreach(Pylon pylon in pylonsInGroup)
        {
            if (!pylon.IsDeactivated)
            {
                return;
            }
        }

        // Unlock the door by calling it's trigger function
        doorToUnlock.GetComponent<ISwitchTriggerable>().OnTriggerActivate();
    }
}

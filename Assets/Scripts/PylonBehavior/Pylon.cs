using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : MonoBehaviour, ISwitchTriggerable
{
    [SerializeField] private PylonGroupController groupController;
    [SerializeField] private Light pylonPointLight;

    private bool isActive = true;

    public bool IsDeactivated
    {
        get
        {
            return !isActive;
        }
    }

    public void OnTriggerActivate()
    {
        isActive = false;
        groupController.OnPylonActivated();
        pylonPointLight.color = Color.yellow;
    }
}

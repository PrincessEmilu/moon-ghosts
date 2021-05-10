using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    private bool switchEnabled = true;

    [SerializeField] private float ActivationDistance = 4.0f;
    [SerializeField] private KeyCode activationKey;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private Light lightToDeactivate;

    // This assumes that the player is the object that must be near the switch
    [SerializeField] private GameObject playerObject;

    public void Update()
    {
        if (switchEnabled)
        {
            if (Input.GetKeyDown(activationKey))
            {
                if (Vector3.Distance(transform.position, playerObject.transform.position) < ActivationDistance)
                {
                    OnSwitchActivated();
                }
            }
        }
    }
    private void OnSwitchActivated()
    {
        Debug.Log("Pylon Switch Activated!");

        switchEnabled = false;
        objectToActivate.GetComponent<ISwitchTriggerable>().OnTriggerActivate();
        lightToDeactivate.color = Color.yellow;
    }
}

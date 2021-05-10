using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockableBarrier : MonoBehaviour, ISwitchTriggerable
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private KeyCode activationKey;
    [SerializeField] private const float ActivationDistance = 15.0f;

    bool barrierActive = true;

    public void OnTriggerActivate()
    {
        Debug.Log("barrier has been removed");
        barrierActive = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Update()
    {
        if (!barrierActive)
        {
            Debug.Log("updating after activation");
            if (Input.GetKeyDown(activationKey))
            {
                if (Vector3.Distance(transform.position, playerObject.transform.position) < ActivationDistance)
                {
                    // TODO: Game end logic
                    Debug.Log("You entered the door");
                    SceneManager.LoadScene("VictoryScene");
                }
            }
        }
    }
}

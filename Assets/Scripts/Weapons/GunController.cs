using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject currentGun;

    private GunBehavior gunScript;

    private void Start()
    {
        gunScript = currentGun.GetComponent<GunBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        // Fully-automatic guns are fired when the mouse is held down
        if (gunScript.IsFullAuto)
        {
            if (Input.GetMouseButton(0))
            {
                gunScript.ShootGun();
            }
        }
        // Semi-auto and bolt-action guns are shot on button press only
        else if (Input.GetMouseButtonDown(0))
        {
            gunScript.ShootGun();
        }
        // If not shooting, the player may instead want to reload the gun
        else if (Input.GetKeyDown(KeyCode.R))
        {
            gunScript.Reload();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GunController is responsible for taking player input and telling the gun what to do.
/// The GunController only cares whether or not the fun is full-auto. Otherwise it does not care what kind of gun it is controlling.
/// </summary>
public class GunController : MonoBehaviour
{
    /// <summary>
    /// The guns that the player currently has equiped.
    /// </summary>
    [SerializeField] private GameObject[] gunLoadout;

    public GameObject CurrentGun
    {
        set
        {
            currentGun = value;
            gunBehavior = currentGun.GetComponent<GunBehavior>();
        }
        get
        {
            return currentGun;
        }
    }

    private GameObject currentGun;

    /// <summary>
    /// Access to the gunbehavior attached to the current gun.
    /// </summary>
    private GunBehavior gunBehavior;

    /// <summary>
    /// On Start, gets the GunBehavior of the current gun for later usage.
    /// </summary>
    private void Start()
    {
        CurrentGun = gunLoadout[0];

        if (gunLoadout.Length <= 1)
            return;

        for (int i = 1; i < gunLoadout.Length; i++)
        {
            gunLoadout[i].SetActive(false);
        }
    }

    /// <summary>
    /// Checks for player input each Update.
    /// The player can shoot a full-auto gun by holding the mouse,
    /// A semi or bolt-action gun by clicking,
    /// they can reload by pressing R,
    /// and they can switch weapons by rolling the mouse wheel
    /// </summary>
    void Update()
    {
        // Fully-automatic guns are fired when the mouse is held down
        if (gunBehavior.IsFullAuto)
        {
            if (Input.GetMouseButton(0))
            {
                gunBehavior.ShootGun();
            }
        }
        // Semi-auto and bolt-action guns are shot on button press only
        else if (Input.GetMouseButtonDown(0))
        {
            gunBehavior.ShootGun();
        }
        // If not shooting, the player may instead want to reload the gun
        else if (Input.GetKeyDown(KeyCode.R))
        {
            gunBehavior.Reload();
        }
        // Switch Weapons
        else if (Input.mouseScrollDelta.magnitude != 0)
        {
            Debug.Log("Swap gun");
        }
    }
}

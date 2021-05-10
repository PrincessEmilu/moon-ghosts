using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    /// <summary>
    /// Does this gun fire while holding down the trigger?
    /// </summary>
    [SerializeField] private bool isFullAuto;

    /// <summary>
    /// At what rate to the bullets shoot from this gone?
    /// </summary>
    [SerializeField] private float fireRate;

    /// <summary>
    /// How long does it take to reload this gun?
    /// </summary>
    [SerializeField] private float secondsToReload;

    /// <summary>
    /// The maximum number of bullets per clip
    /// </summary>
    [SerializeField] private int clipCapacity;

    /// <summary>
    /// The vector3 the represents where the muzzle of the gun is, ie where bullets should spawn from
    /// </summary>
    [SerializeField] private Vector3 muzzlePoint;

    /// <summary>
    /// The bullet prefab that this gun shoots
    /// </summary>
    [SerializeField] private GameObject bulletPrefab;
    
    /// <summary>
    /// Is the player in the middle of a reload?
    /// </summary>
    private bool isReloading;

    /// <summary>
    /// Is the gun on "cooldown" before the next bullet can be short?
    /// </summary>
    private bool isFireable;

    private Clip gunClip;

    /// <summary>
    /// Timers track of the time since reload and the time since the last shot, respectively
    /// </summary>
    private Timer reloadTimer;
    private Timer fireTimer;

    public bool IsFullAuto { get { return isFullAuto; } }

    [SerializeField] Transform playerCamTransform = null;

    [SerializeField] private SoundManager soundManager;

    [FMODUnity.EventRef]
    public string shootEvent;

    public System.DateTime switchTime; 

    /// <summary>
    /// Initilaize the gun on Awake
    /// </summary>
    private void Awake()
    {
        isReloading = false;
        isFireable = true;

        reloadTimer = new Timer(secondsToReload);
        fireTimer = new Timer(fireRate);

        gunClip = new Clip(clipCapacity);

        soundManager = FindObjectOfType<SoundManager>();

        switchTime = System.DateTime.Now;
    }

    /// <summary>
    /// Update uses the timers to keep track of the gun's state for shooting and reloading
    /// </summary>
    void Update()
    {
        if (isReloading)
        {
            reloadTimer.Tick();
            if (reloadTimer.CheckTime())
            {
                transform.Rotate(new Vector3(-45, 0, 0));
                gunClip.RefillClip();
                isReloading = false;
                reloadTimer.Reset();
            }
        }

        if (!isFireable)
        {
            fireTimer.Tick();
            if (fireTimer.CheckTime())
            {
                isFireable = true;
                fireTimer.Reset();
            }
        }
    }

    /// <summary>
    /// Spawns a new bullet if not reloading, out of ammo, or on cooldown
    /// </summary>
    public void ShootGun()
    {
        if (!isReloading && isFireable && !gunClip.IsEmpty)
        {
            Shoot();

            QualtricsDataContainer.AddWeaponShotFired(name);

            //soundManager.PlayShoot(shootEvent); 
            Debug.Log("sound disabled");
        }
    }

    /// <summary>
    /// Starts the reload timer. When the timer is finished, the reload will happen.
    /// </summary>
    public void Reload()
    {
        if (!isReloading)
        {
            transform.Rotate(new Vector3(45, 0, 0));
            isReloading = true;

            //soundManager.PlayReload();
            Debug.Log("Sound is disabled");
        }
    }

    /// <summary>
    /// Handles spawning and applying velocity/targetting to the bullet
    /// </summary>
    private void Shoot()
    {
        Debug.Log("Pew!");
        isFireable = false;
        gunClip.UseBullet();

        // TODO: Spawn a bullet, play animations, etc etc.
        Instantiate(bulletPrefab, gameObject.transform.position + transform.TransformDirection(muzzlePoint), playerCamTransform.rotation);
    }

    public void OnGunSwitch()
    {
        if (!gameObject.activeInHierarchy)
        {
            switchTime = System.DateTime.Now; 
        }
        else
        {
            float secondsUsed = (float)(System.DateTime.Now - switchTime).TotalSeconds;

            QualtricsDataContainer.AddWeaponUseTime(name, secondsUsed);
        }

        soundManager.PlaySwitchWeapon();

        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    [SerializeField] private bool isFullAuto;
    [SerializeField] private float fireRate;
    [SerializeField] private float secondsToReload;
    [SerializeField] private int clipCapacity;
    
    private bool isReloading;
    private bool isFireable;

    private Clip gunClip;

    private Timer reloadTimer;
    private Timer fireTimer;

    public bool IsFullAuto { get { return isFullAuto; } }

    private void Awake()
    {
        isReloading = false;
        isFireable = true;

        reloadTimer = new Timer(secondsToReload);
        fireTimer = new Timer(fireRate);

        gunClip = new Clip(clipCapacity);
    }
    void Update()
    {
        if (isReloading)
        {
            reloadTimer.Tick();
            if (reloadTimer.CheckTime())
            {
                Debug.Log("Reload finished");
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

    public void ShootGun()
    {
        if (!isReloading && isFireable && !gunClip.IsEmpty)
        {
            Shoot();
        }
        else
        {
            Debug.Log("*click*");
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading, cover me!");
        if (!isReloading)
        {
            isReloading = true;
        }
    }

    public void OnSwitch()
    {
        // TODO: Disable the gun, somehow
    }

    private void Shoot()
    {
        Debug.Log("Pew!");
        isFireable = false;
        gunClip.UseBullet();

        // TODO: Spawn a bullet, play animations, etc etc.
    }
}

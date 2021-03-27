using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clip is responsible to keeping track of if their are bullets avaialble to shoot.
/// Out of a total supply of bullets, a certain amount can be stored in the clip.
/// When reloading, take as many bullets as possible and put them into the clip.
/// </summary>
public class Clip
{
    private int bulletCapacity;
    private int currentBulletSupply;
    private int currentBulletsInClip;
    public bool IsEmpty { get { return currentBulletsInClip == 0; } }

    /// <summary>
    /// Initializes the Clip
    /// </summary>
    /// <param name="capacity">The maxium number of bullets that can fit in one clip</param>
    public Clip(int capacity)
    {
        bulletCapacity = capacity;
        currentBulletsInClip = bulletCapacity;

        // TODO: This should be set either by the gun or the player's "loadout"
        currentBulletSupply = 50;
    }

    /// <summary>
    /// Uses up one bullet from the clip
    /// </summary>
    public void UseBullet()
    {
        currentBulletsInClip--;
    }

    /// <summary>
    /// Attempts to refill the clip up to capacity. Will partially refill it if not enough ammo left.
    /// </summary>
    public void RefillClip()
    {
        int bulletToAdd = bulletCapacity - currentBulletsInClip;

        if (currentBulletSupply >= bulletToAdd)
        {
            currentBulletSupply -= bulletToAdd;
            currentBulletsInClip += bulletToAdd;
        }
        else if (currentBulletSupply > 0)
        {
            currentBulletsInClip += currentBulletSupply;
            currentBulletSupply = 0;
        }
    }
}

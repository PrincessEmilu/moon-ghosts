using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip
{
    private int bulletCapacity;
    private int currentBulletSupply;
    private int currentBulletsInClip;
    public bool IsEmpty { get { return currentBulletsInClip == 0; } }

    public Clip(int capacity)
    {
        bulletCapacity = capacity;
        currentBulletsInClip = bulletCapacity;

        // TODO: This should be set either by the gun or the player's "loadout"
        currentBulletSupply = 50;
    }

    public void UseBullet()
    {
        currentBulletsInClip--;
    }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float cdTimer;
    public float fireCD;
    public bool canFire;

    public GameObject projectile;

    public Queue<GameObject> bounds;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        cdTimer = 0.0f;
        fireCD = 2.0f;
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        // logic to make sure the fire has a 2s cooldown
        if(canFire == false)
        {
            cdTimer += Time.deltaTime;
        }
        if(cdTimer >= fireCD)
        {
            canFire = true;
        }

        // if I see the player, fire
        if(DistBetween(player) < 5 && canFire == true)
        {
            cdTimer = 0.0f;
            Fire(player.transform.position);
            canFire = false;
        }
    }

    /// <summary>
    /// public void Fire();
    /// Params: Vector3 target, the vector3 we are shooting at
    /// Returns: None
    /// Fires a projectile at the vector3
    /// </summary>
    public void Fire(Vector3 target)
    {
        Vector3 tempVec = target - transform.position;
        GameObject tempProj = GameObject.Instantiate(projectile, transform.position + Vector3.Normalize(tempVec), Quaternion.identity);
        tempProj.GetComponent<Projectile>().targetVec = target;
    }
}

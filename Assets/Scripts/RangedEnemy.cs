using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float cdTimer;
    public float fireCD;
    public bool canFire;

    public GameObject projectile;
    public List<GameObject> bounds;

    public int currentBound;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        cdTimer = 0.0f;
        fireCD = 2.0f;
        canFire = true;
        currentBound = 1;
        speed = 0.25f;
        maxSpeed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // logic to make sure the fire has a 2s cooldown
        if (canFire == false)
        {
            cdTimer += Time.deltaTime;
        }
        if (cdTimer >= fireCD)
        {
            canFire = true;
        }

        // if I see the player, fire
        if (DistBetween(player) < 5)
        {
            rb.velocity = rb.velocity * 0.8f;
            if (canFire == true)
            {
                cdTimer = 0.0f;
                Fire(player.transform.position);
                canFire = false;
            }
        }
        // if the player is not in range, chase the appropriate bound
        else
        {
            if (currentBound == 0)
            {
                Chase(bounds[1]);
            }
            else if (currentBound == 1)
            {
                Chase(bounds[0]);
            }
        }

        // clamping the magnitude of the speed
        if (Vector3.Magnitude(rb.velocity) > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
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
        tempProj.GetComponent<Projectile>().player = player;
    }

    // collision resolution with trigger objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bounds")
        {
            if (currentBound == 0)
            {
                currentBound = 1;
            }
            else if (currentBound == 1)
            {
                currentBound = 0;
            }
        }
    }
}

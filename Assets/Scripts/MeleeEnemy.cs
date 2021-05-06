using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public enum MeleeState
    {
        wander, 
        chase
    }

    public float sightRange;
    public Vector3 prevVec;
    public float wanderDiff;
    public float maxSpeed;
    public float speed;
    public MeleeState currentState;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sightRange = 5.0f;
        prevVec = new Vector3(0.0f, 0.0f, 0.0f);
        wanderDiff = 10f * Mathf.Deg2Rad;
        speed = 3.0f;
        maxSpeed = 6.0f;
        currentState = MeleeState.wander;
        health = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // if I am close enough to the player, chase it. otherwise, wander
        if(DistBetween(player) <= sightRange)
        {
            Chase(player);
            currentState = MeleeState.chase;
        }
        else
        {
            Wander();
            currentState = MeleeState.wander;
        }

        // clamping the magnitude of the speed
        if (Vector3.Magnitude(rb.velocity) > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public float DistBetween(GameObject target)
    {
        return Vector3.Magnitude(transform.position - target.transform.position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void Chase(GameObject target)
    {
        // get vector to enemy, normalize it, and multiply it by the speed
        Vector3 directionVec = Vector3.Normalize(target.transform.position - transform.position);
        transform.LookAt(target.transform.position);

        Vector3 vecToEenemy = directionVec * speed;

        // add the force to the RB
        rb.AddForce(vecToEenemy);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Wander() 
    {
        // gets vector based of fprevious vector, adding or subtracting a little to get "randomized" movement
        Vector3 wanderVecNorm = new Vector3(prevVec.x + Random.Range(-wanderDiff, wanderDiff), 0.0f, prevVec.z + Random.Range(-wanderDiff, wanderDiff));

        // normalize it and m,ultiply it by speed
        wanderVecNorm = Vector3.Normalize(wanderVecNorm);
        Vector3 wanderVec = wanderVecNorm * speed;

        // add in some rotation so it looks good
        transform.LookAt(transform.position + wanderVecNorm);
        Debug.DrawRay(transform.position, wanderVecNorm);

        // add a force to the new vector
        rb.AddForce(wanderVec);

        // set prevVec to the vector we just worked with
        prevVec = wanderVec;
    }

    // OBSATCLE AVOIDANCE
}

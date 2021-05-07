using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public enum MeleeState
    {
        wander, 
        chase, 
        avoid
    }

    public float sightRange;
    public Vector3 prevVec;
    public float wanderDiff;
    public float maxSpeed;
    public float speed;
    public MeleeState currentState;
    public List<GameObject> obstacles;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sightRange = 5.0f;
        prevVec = new Vector3(0.0f, 0.0f, 0.0f);
        wanderDiff = 10f * Mathf.Deg2Rad;
        speed = 1.5f;
        maxSpeed = 3.5f;
        currentState = MeleeState.wander;
        health = 10.0f;

        GameObject[] tempObs = GameObject.FindGameObjectsWithTag("Obstacle");
        for(int i = 0; i < tempObs.Length; i ++)
        {
            obstacles.Add(tempObs[i]);
        }

        GameObject[] tempEn = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < tempEn.Length; i++)
        {
            obstacles.Add(tempEn[i]);
        }
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

        // loops trhough each obstacle and avoids them
        for (int i = 0; i < obstacles.Count; i++)
        {
            ObstcaleAvoidance(obstacles[i]);
        }

        // clamping the magnitude of the speed
        if (Vector3.Magnitude(rb.velocity) > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        transform.LookAt(transform.position + rb.velocity);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }

    /// <summary>
    /// public float DistBetween(GameObject Target)
    /// Params: GameObject Target, the object I am measureing the distance between with myself
    /// Returns: Float, the distance between myself and the target object
    /// Returns the distance between this GameObject and the target GameObject
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public float DistBetween(GameObject target)
    {
        return Vector3.Magnitude(transform.position - target.transform.position);
    }

    /// <summary>
    /// public void Chase(GameObject target)
    /// Params: Game Object Target. The object you are chasing
    /// Returns: None
    /// Points me in the direction of the object I am chasing and sends me there
    /// </summary>
    /// <param name="target"></param>
    public void Chase(GameObject target)
    {
        // get vector to enemy, normalize it, and multiply it by the speed
        Vector3 directionVec = Vector3.Normalize(target.transform.position - transform.position);
        Vector3 vecToEenemy = directionVec * speed;

        // add the force to the RB
        rb.AddForce(vecToEenemy, ForceMode.Force);
    }

    /// <summary>
    /// public void Wander()
    /// Params: None
    /// Returns: None
    /// Choses a random direction based off my previous direction and sends me that way
    /// </summary>
    public void Wander() 
    {
        // gets vector based of fprevious vector, adding or subtracting a little to get "randomized" movement
        Vector3 wanderVecNorm = new Vector3(prevVec.x + Random.Range(-wanderDiff, wanderDiff), 0.0f, prevVec.z + Random.Range(-wanderDiff, wanderDiff));

        // normalize it and m,ultiply it by speed
        wanderVecNorm = Vector3.Normalize(wanderVecNorm);
        Vector3 wanderVec = wanderVecNorm * speed;

        // add a force to the new vector
        rb.AddForce(wanderVec, ForceMode.Force);
        Debug.DrawRay(transform.position, wanderVecNorm);

        // set prevVec to the vector we just worked with
        prevVec = wanderVec;
    }

    // OBSATCLE AVOIDANCE
    /// <summary>
    /// public void ObstacleAvoidance()
    /// Params: GameObject obstacle, the obstacle we are seeing if we need to avoid
    /// Returns: None
    /// Runs through the obstacle avoidance algorithm to make sure I don't hit anything
    /// </summary>
    public void ObstcaleAvoidance(GameObject obstacle)
    {
        Vector3 betweenVec = gameObject.transform.position - obstacle.transform.position;
        float myR = gameObject.GetComponent<CapsuleCollider>().radius + 0.5f;
        float theirR = 0.0f;

        if (obstacle.tag == "Enemy")
        {
            theirR = obstacle.GetComponent<CapsuleCollider>().radius;
        }
        else if (obstacle.tag == "Obstacle")
        {
            theirR = obstacle.GetComponent<Obstacle>().radius;
        }


        // if the obstacle is too far away, return from the method
        if(DistBetween(obstacle) > 5)
        {
            return;
        }

        // if the dot of my forward and thier pos is negative, they are behind me, so return from the method
        if(Vector3.Dot(betweenVec, transform.forward) < 0)
        {
            return;
        }

        // check if the dot(betweenVec, right) < myR + theirR, which means they will collide, 
        if(Vector3.Dot(betweenVec, transform.right) < myR + theirR)
        {
            int wallLayer = LayerMask.GetMask("Wall");
            Vector3 center = new Vector3(30.0f, 1.0f, 26.0f);
            if(obstacle.layer == wallLayer)
            {
                rb.AddForce(center - transform.position * 10.0f, ForceMode.Force);
                return;
            }

            currentState = MeleeState.avoid;
            // dot against my right and see if they are on the left or the right
            // positive is to the right
            if (Vector3.Dot(betweenVec, transform.right) > 0)
            {
                Debug.Log("Avoiding to the right");
                rb.AddForce(Vector3.left * speed, ForceMode.Force);
            }
            // everything else is to the left
            else
            {
                rb.AddForce(Vector3.right * speed, ForceMode.Force);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;
    public float health;
    public Vector3 prevVec;
    public float wanderDiff;
    public float maxSpeed;
    public float speed;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        health = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float incomingDmg)
    {
        health -= incomingDmg;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
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
        if (DistBetween(obstacle) > 5)
        {
            return;
        }

        // if the dot of my forward and thier pos is negative, they are behind me, so return from the method
        if (Vector3.Dot(betweenVec, transform.forward) < 0)
        {
            return;
        }

        // check if the dot(betweenVec, right) < myR + theirR, which means they will collide, 
        if (Vector3.Dot(betweenVec, transform.right) < myR + theirR)
        {
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

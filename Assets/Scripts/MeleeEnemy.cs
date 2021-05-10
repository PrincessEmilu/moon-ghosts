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
    public MeleeState currentState;
    public List<GameObject> obstacles;
    public Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sightRange = 5.0f;
        prevVec = new Vector3(0.0f, 0.0f, 0.0f);
        wanderDiff = 5.0f * Mathf.Deg2Rad;
        speed = 1.5f;
        maxSpeed = 3.5f;
        currentState = MeleeState.wander;
        center = new Vector3(25.0f, 1.0f, 29.0f);

        GameObject[] tempObs = GameObject.FindGameObjectsWithTag("Obstacle");
        for(int i = 0; i < tempObs.Length; i ++)
        {
            obstacles.Add(tempObs[i]);
        }

        GameObject[] tempEn = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < tempEn.Length; i++)
        {
            if(tempEn[i] != gameObject)
            {
                obstacles.Add(tempEn[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        // loops trhough each obstacle and avoids them
        for (int i = 0; i < obstacles.Count; i++)
        {
            ObstcaleAvoidance(obstacles[i]);
        }

        // if I am close enough to the player, chase it. otherwise, wander
        if (DistBetween(player) <= sightRange)
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

        // transform.LookAt(transform.position + rb.velocity);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            Vector3 tempVec = center - transform.position;
            prevVec = tempVec;
            rb.AddForce(tempVec * speed * 5);
        }
    }
}

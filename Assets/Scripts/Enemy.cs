using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;
    public float health;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
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
}

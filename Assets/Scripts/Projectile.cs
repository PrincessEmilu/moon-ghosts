using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 targetVec;
    public Rigidbody rb;
    public float speed;
    public float maxSpeed;
    public GameObject player;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 0.50f;
        maxSpeed = 1.5f;
        transform.LookAt(targetVec);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * speed);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        timer += Time.deltaTime;

        if(timer >= 3.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            player.GetComponent<FPSController>().TakeDamage(1.0f);
            Destroy(gameObject);
        }
    }
}

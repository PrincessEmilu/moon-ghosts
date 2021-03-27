using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] float baseDamage;
    [SerializeField] float maximumLifetime;

    private Timer cullTimer;

    // Start is called before the first frame update
    void Start()
    {
        cullTimer = new Timer(maximumLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        cullTimer.Tick();
        if (cullTimer.CheckTime())
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: On collision with something that can be damaged, damage that something
    }
}

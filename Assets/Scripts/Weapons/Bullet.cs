using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the bullet prefab. For now, it will delete itself after its maximum lifetime has passed.
/// At some point the bullet prefab will want more information, such as how much damage it deals.
/// It will also handle dealing damage to damageable objects (such as the player)
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] float baseDamage;
    [SerializeField] float maximumLifetime;
    [SerializeField] float projectileSpeed = 100f;

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
        //moving the bullet forward.
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: On collision with something that can be damaged, damage that something
        Debug.Log("Hit soemthing");
       
        IDamageable<float> damageable = collision.transform.GetComponent<IDamageable<float>>();

        if (damageable == null)
        {
            return;
        }

        damageable.TakeDamage(baseDamage);
    }
}

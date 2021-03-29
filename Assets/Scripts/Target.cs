using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour,IDamageable<float>
{
    public float health;
    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            //todo other stuff here like updating score or something idk.
            Destroy(gameObject);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int trapDamage = 30;
    public Vector2 knockback = Vector2.zero;
    Damageable damageable;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            damageable = other.GetComponent<Damageable>();
            Vector2 deliveredKnockback =
                transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(trapDamage, deliveredKnockback);

            if (gotHit)
                Debug.Log(other.name + " kena trap " + trapDamage);
        }
    }
}

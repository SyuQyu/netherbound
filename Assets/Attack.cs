using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Cek apakah bisa kena hit
        Damageable damageable = col.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback =
                transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            // Hit targetnya
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
                Debug.Log(col.name + " kena hit " + attackDamage);
        }
    }
}
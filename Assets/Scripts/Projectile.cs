using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 3f;
    public float lifetime = 3f;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    private Vector3 direction;

    public void Setup(Vector3 shootDirection, Vector3 parentScale)
    {
        direction = new Vector3(shootDirection.x * parentScale.x, shootDirection.y, shootDirection.z).normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = direction.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

            Destroy(gameObject);
            if (gotHit)
                Debug.Log(collision.name + " kena hit fire " + attackDamage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private bool canShoot = true;
    public float cooldownFire = 2f;
    
    public void Shoot()
    {
        if (!canShoot)
            return;

        GameObject fire = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = new Vector3(transform.localScale.x, 0);
        fire.GetComponent<Projectile>().Setup(direction, transform.parent.localScale);

        StartCoroutine(ShootCooldown());
    }
    
    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownFire);
        canShoot = true;
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        // Cek apakah bisa kena hit
        Damageable damageable = col.GetComponent<Damageable>();

        if (damageable != null)
        {
            Shoot();
        }
    }
}

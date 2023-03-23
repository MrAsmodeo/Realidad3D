using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float shotForce = 1500;
    public float shotRate = 0.5f;
    private float shotRateTime = 0f;
    public int damage = 10;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > shotRateTime)
            {
                GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);
                shotRateTime = Time.time + shotRate;
                Destroy(newBullet, 2);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with is an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);

            // Destroy the bullet
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}


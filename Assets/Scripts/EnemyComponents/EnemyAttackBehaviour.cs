using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    public float fireRate;
    public float fireRange;
    private float fireTimer;

    public Transform PlayerTransform { get; set; }

    //TODO: Optimise and refactor to not use update method, cache player postion with one findgameobject call and store for akk enemies to access
    void Start()
    {
        fireTimer = 0f;
        fireRate = Random.Range(1.0f, 2.0f);
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (Vector3.Distance(transform.position, PlayerTransform.position) <= fireRange && fireTimer >= fireRate)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        // Retrieve a projectile from the object pool
        GameObject projectile = ObjectPool.instance.GetFromPool();

        // Set the projectile's target and position
        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileBehaviour>().Target = PlayerTransform;

        // Activate the projectile
        projectile.SetActive(true);
        projectile.GetComponent<ProjectileBehaviour>().ProjectileShoot();

        fireTimer = 0f;
    }
}

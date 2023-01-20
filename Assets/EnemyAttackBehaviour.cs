using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    public float fireRate;
    public float fireRange = 10f;

    private float fireTimer = 0f;
    private Transform playerTransform;

    //TODO: Optimise and refactor to not use update method, cache player postion with one findgameobject call and store for akk enemies to access
    void Start()
    {
        fireRate = Random.Range(1.0f, 2.0f);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Vector3.Distance(transform.position, playerTransform.position) <= fireRange && fireTimer >= fireRate)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void FireProjectile()
    {
        // Retrieve a projectile from the object pool
        GameObject projectile = ObjectPool.instance.GetFromPool();

        // Set the projectile's target and position
        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileBehaviour>().target = playerTransform;

        // Activate the projectile
        projectile.SetActive(true);
        projectile.GetComponent<ProjectileBehaviour>().ProjectileShoot();

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public Transform target { get; internal set; }

    public void ProjectileShoot()
    {
        StartCoroutine(ProjectileFire());
    }
    private IEnumerator ProjectileFire()
    {
        float t = 0f;
        var pos = target.position;
        while (t <= 5f)
        {
            transform.position = Vector3.Lerp(transform.position, pos, t * 0.5f);
            t += Time.deltaTime;
            yield return null;
        }

        // Return the object to the pool when it's no longer needed
        ObjectPool.instance.ReturnToPool(gameObject);
    }
}


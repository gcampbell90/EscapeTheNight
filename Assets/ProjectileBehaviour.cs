using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
        while (t <= 2f)
        {
            transform.position = Vector3.Lerp(transform.position, pos, t);
            t += Time.deltaTime;
            yield return null;
        }

        // Return the object to the pool when it's no longer needed
        ObjectPool.instance.ReturnToPool(gameObject);
    }
}


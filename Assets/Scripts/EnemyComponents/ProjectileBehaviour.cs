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
        var start = transform.position;
        var targetPos = target.position;
        while (t <=1f)
        {
            Debug.Log("Moving Projectile");
            transform.position = Vector3.Lerp(start, targetPos, t);
            transform.LookAt(targetPos);
            t += Time.deltaTime;
            yield return null;
        }

        // Return the object to the pool when it's no longer needed
        gameObject.SetActive(false);
    }
}


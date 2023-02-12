using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] GameObject explosionGO;
    public Transform Target { get; internal set; }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
        if(other.name == "Player")
        {
            GameController.onSpeedChange?.Invoke(GameController.Instance.StandardSpeed_MPH/2);
        }
    }

    private void Explode()
    {
        var explosion = Instantiate(explosionGO);
        explosion.transform.position = transform.position;
        Destroy(explosion,1f);
    }
    public void ProjectileShoot()
    {
        Material material = transform.GetChild(0).GetComponent<Renderer>().material;
        material.SetFloat("_BoostPower", 5f);
        StartCoroutine(ProjectileFire());
    }
    private IEnumerator ProjectileFire()
    {
        float t = 0f;
        var start = transform.position;
        var targetPos = Target.position;
        while (t < 1.01f)
        {
            //Debug.Log("Moving Projectile");
            transform.position = Vector3.Lerp(start, targetPos, t);
            transform.LookAt(targetPos);
            t += Time.deltaTime;
            yield return null;
        }
        Explode();
        // Return the object to the pool when it's no longer needed
        gameObject.SetActive(false);
    }
}


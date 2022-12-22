using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private int health = 10;
    private bool isAnimated = false;
    public Vector3 TargetPos { get; set; }

    private void Start()
    {
        StartCoroutine(MoveToTarget());
    }

    public void Animate()
    {
        isAnimated = true;
        StartCoroutine(FloatAround());
    }
    private IEnumerator MoveToTarget()
    {
        float timer = 0;
        float duration = 2f;
        Vector3 SpawnPos = new Vector3(0, 70, 50);
        while(timer < 1)
        {
            transform.position = Vector3.Slerp(SpawnPos, TargetPos, timer);
            timer += Time.deltaTime/ duration;
            yield return null;
        }
        if (!isAnimated) yield break; 
    }

    private IEnumerator FloatAround()
    {
        float _t = 0f;
        float dur = 1f;
        Vector3 _offset;

        while (true)
        {
            _offset = new Vector3(Random.Range(-12f, 12f), Random.Range(5, 11f), Random.Range(10f, 20f));

            while (_t < dur)
            {
                transform.position = Vector3.Lerp(transform.position, _offset, _t);
                transform.LookAt(Vector3.zero);
                _t += Time.deltaTime / dur;
                yield return null;
            }
            _t = 0f;

            yield return null;
        }
    }

    void HitByRay()
    {
        Debug.Log($"{gameObject.name} was hit by a Ray");
        TakeDamage();

    }

    private void TakeDamage()
    {
        health -= 1;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Debug.Log("Enemy Destroyed");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private int health = 10;

    public Vector3 TargetPos { get; set; }

    private void Start()
    {
        StartCoroutine(MoveToTarget());
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
        StartCoroutine(FloatAround());
    }

    private IEnumerator FloatAround()
    {
        float _t = 0f;
        float dur = 0.25f; 
        Vector3 _offset = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-5f, 5f), Random.Range(2f, 10f));
        var _origin = transform.position;
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, _origin + _offset, _t);
            if(_t > 1f)
            {
                _t = 0f;
                _offset = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-5f, 5f), Random.Range(2f, 10f));
                continue;
            }
            _t += Time.deltaTime / dur;

            yield return null;
        }
    }

    void HitByRay()
    {
        Debug.Log("I was hit by a Ray");
        TakeDamage();
    }

    private void TakeDamage()
    {
        health -= 4;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Debug.Log("Enemy Killed");
    }
}

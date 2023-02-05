using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    private int health = 10;
    private bool isAnimated = false;
    public Vector3 TargetPos { get; set; }
    public Transform PlayerTransform { get; set; }

    [SerializeField] ParticleSystem deathExplosion;
    [SerializeField] AudioSource explosionSound;

    private bool isDestroyed = false;

    private void Start()
    {
        StartCoroutine(MoveToTarget());
        StartCoroutine(LookAtTarget());
    }

    public void Animate()
    {
        isAnimated = true;
        StartCoroutine(FloatAround());
    }
    private IEnumerator LookAtTarget()
    {
        while (true)
        {
            transform.LookAt(PlayerTransform);
            yield return null;
        }
    }
    private IEnumerator MoveToTarget()
    {
        float timer = 0;
        float duration = 2f;
        Vector3 SpawnPos = new Vector3(0, Random.Range(20f, 30f), Random.Range(30f, 50f));
        while (timer < 1)
        {
            transform.position = Vector3.Slerp(SpawnPos, TargetPos, timer);
            timer += Time.deltaTime / duration;
            yield return null;
        }
        Animate();
    }
    private IEnumerator FloatAround()
    {
        float m_timer = 0f;
        float m_duration = 1f;
        Vector3 m_offset;

        while (true)
        {
            m_offset = new Vector3(Random.Range(-12f, 12f), Random.Range(5, 11f), Random.Range(20f, 40f));
            var m_pos = transform.position;
            while (m_timer < m_duration)
            {
                transform.position = Vector3.Lerp(m_pos, m_offset, m_timer);
                m_timer += Time.deltaTime / m_duration;
                yield return null;
            }
            m_timer = 0f;
            yield return null;
        }
    }

    void HitByRay()
    {
        if (isDestroyed != true)
        {
            Debug.Log($"{gameObject.name} was hit by a Ray");
            TakeDamage();
        }
        else
        {
            return;
        }
    }

    private void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            StartCoroutine(DroneDeath());
        }
    }
    IEnumerator DroneDeath()
    {
        explosionSound.Play();
        deathExplosion.Play();
        yield return new WaitForSeconds(0.15f);
        isDestroyed = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy Hit OnCollision");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EnemySpawnManager.onDroneDestroyed?.Invoke();
        Debug.Log("Enemy Destroyed");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private int spawnCount;
    [SerializeField] GameObject enemy;
    [SerializeField] bool isMoving;
    private Transform playerTransform;

    private int spawnedEnemyCount = 0;

    public delegate void OnDroneDestroyed();
    public static OnDroneDestroyed onDroneDestroyed;

    public delegate void OnDroneCall();
    public static OnDroneCall onDroneCall;

    private void OnEnable()
    {
        onDroneCall += ConfirmDronesPresent;
        onDroneDestroyed += UpdateDroneCount;
    }
    private void OnDisable()
    {
        onDroneCall -= ConfirmDronesPresent;
        onDroneDestroyed -= UpdateDroneCount;
    }
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void ConfirmDronesPresent()
    {
        if (spawnedEnemyCount == 0)
        {
            SpawnEnemies();
        }
        else
        {
            return;
        }
    }
    private void UpdateDroneCount()
    {
        spawnedEnemyCount--;

        if (spawnedEnemyCount == 0)
        {
            UIController.onDroneAlert?.Invoke(false);
            Debug.Log("onDroneAlert false");
        }
    }

    private void SpawnEnemies()
    {
        //Event called to let UI know drones are about to attack
        UIController.onDroneAlert?.Invoke(true);

        spawnedEnemyCount = spawnCount;
        Vector3 m_position = new Vector3();

        float m_offset = spawnCount / 2;
        float m_spacing = 5;

        for (int x = 0; x < spawnCount; x++)
        {
            m_position.x = (x - m_offset) * m_spacing;
            m_position.y = Random.Range(7, 10);
            m_position.z = Random.Range(35,55);

            Vector3 SpawnPos = new Vector3(Random.Range(-30f, 30f), Random.Range(10f, 20f), Random.Range(-100f, -150f));

            var m_enemy = Instantiate(enemy, SpawnPos, Quaternion.identity);
            m_enemy.GetComponent<EnemyBehaviour>().TargetPos = m_position;
            m_enemy.GetComponent<EnemyBehaviour>().PlayerTransform = playerTransform;
            m_enemy.GetComponent<EnemyAttackBehaviour>().PlayerTransform = playerTransform;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private int spawnCount;
    [SerializeField] GameObject enemy;
    [SerializeField] bool isMoving;
    [SerializeField] private Transform _player;

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

    private void ConfirmDronesPresent()
    {
        if(spawnedEnemyCount == 0)
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
    }

    // Start is called before the first frame update
    void SpawnEnemies()
    {
        spawnedEnemyCount = spawnCount;
        int _y;
        Vector3 _pos = new Vector3();

        float _offset = spawnCount / 2;
        float _spacing = 5;

        for (int x = 0; x < spawnCount; x++)
        {
            _y = 7;
            _pos.x = (x - _offset) * _spacing;
            _pos.y = _y;
            _pos.z = 20;

            var _enemy = Instantiate(enemy);
            _enemy.GetComponent<EnemyBehaviour>().TargetPos = _pos;
            _enemy.GetComponent<EnemyBehaviour>().PlayerTransform = _player;
            if (isMoving) _enemy.GetComponent<EnemyBehaviour>().Animate(); continue;
        }
    }
}

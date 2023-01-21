using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]private int enemyCount;
    [SerializeField] GameObject enemy;
    [SerializeField] bool isMoving;
    [SerializeField] private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        //int x, y;
        int _y;
        Vector3 _pos = new Vector3();
        
        float _offset = enemyCount / 2;
        float _spacing = 5;

        for (int x = 0; x < enemyCount; x++)
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

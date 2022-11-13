using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private int enemyCount;
    [SerializeField] GameObject enemy;

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
            _y = 5;
            _pos.x = (x - _offset) * _spacing;
            _pos.y = _y;
            _pos.z = 10;
            var _enemy = Instantiate(enemy);
            _enemy.GetComponent<Enemy>().TargetPos = _pos;
        }
    }

}

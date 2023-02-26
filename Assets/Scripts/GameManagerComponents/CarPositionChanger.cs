using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using AmazingAssets.CurvedWorld.Example;
using Random = UnityEngine.Random;

public class CarPositionChanger : MonoBehaviour
{
    public CarChunkSpawner spawner;

    int[] preDefinedPosX = new int[] { -5, 0, 5 };

    private void Awake()
    {
        ChangeLanePosition();
    }

    private void Update()
    {
        transform.Translate(spawner.moveDirection * spawner.movingSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        switch (spawner.axis)
        {
            case CarChunkSpawner.AXIS.XPositive:
                if (transform.position.x > spawner.destroyZone)
                    spawner.DestroyChunk(this);
                break;

            case CarChunkSpawner.AXIS.XNegative:
                if (transform.position.x < -spawner.destroyZone)
                    spawner.DestroyChunk(this);
                break;

            case CarChunkSpawner.AXIS.ZPositive:
                if (transform.position.z > spawner.destroyZone)
                    spawner.DestroyChunk(this);
                break;

            case CarChunkSpawner.AXIS.ZNegative:
                if (transform.position.z < -spawner.destroyZone)
                {
                    spawner.DestroyChunk(this);
                    ChangeLanePosition();
                }
                 
                break;
        }
    }
    private void ChangeLanePosition()
    {
        //Debug.Log("Changing car lanes");
        foreach (Transform child in transform)
        {
            int randomIndex = Random.Range(0, preDefinedPosX.Length);
            int selectedNumber = preDefinedPosX[randomIndex];
            child.transform.position = new Vector3(selectedNumber, child.transform.position.y, child.transform.position.z);
        }
    }


}

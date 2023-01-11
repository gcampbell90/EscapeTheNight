using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(BoostController))]

public class PlayerController : MonoBehaviour
{


    ////environment chunks
    //public ChunkSpawner[] chunkSpawners;
    //public ChunkSpawner[] ChunkSpawners { get { return chunkSpawners; } set { ChunkSpawners = chunkSpawners; } }
    
    ////Scene spawner speed
    //public float StandardSpeed { get; set; }
    //public float BoostSpeed { get; set; }
    //public float CurrentSpeed { get; set; }

    private BoostController boostController;
    public BoostController BoostController { get { return boostController; } set { boostController = BoostController; } }


     private void Awake()
    {
        boostController = GetComponent<BoostController>();
        //velocityTracker = GetComponent<VelocityTracker>();
    }

    private void Start()
    {
        
        //Debug.Log($"Normal m/s: {VelocityTracker.GetMeterPerSec(GameController.Instance.StandardSpeed_MPH)} Boost m:s: {VelocityTracker.GetMeterPerSec(GameController.Instance.BoostSpeed_MPH)} CurrentSpeed: {CurrentSpeed}");
    }



    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Player Hit something");
    }

}

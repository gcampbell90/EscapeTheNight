using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehaviour))]
[RequireComponent(typeof(BoostBehaviour))]

public class PlayerCollisionBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Player Hit something");

        GameController.onSpeedChange?.Invoke(10);
    }

}

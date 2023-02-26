﻿using UnityEngine;


namespace AmazingAssets.CurvedWorld.Example
{
    public class RunnerChunk : MonoBehaviour
    {
        public ChunkSpawner spawner;

        public void Update()
        {
            transform.Translate(spawner.moveDirection * spawner.movingSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {

            switch (spawner.axis)
            {
                case ChunkSpawner.AXIS.XPositive:
                    if (transform.position.x > spawner.destroyZone)
                        spawner.DestroyChunk(this);
                    break;

                case ChunkSpawner.AXIS.XNegative:
                    if (transform.position.x < -spawner.destroyZone)
                        spawner.DestroyChunk(this);
                    break;

                case ChunkSpawner.AXIS.ZPositive:
                    if (transform.position.z > spawner.destroyZone)
                        spawner.DestroyChunk(this);
                    break;

                case ChunkSpawner.AXIS.ZNegative:
                    if (transform.position.z < -spawner.destroyZone)
                        spawner.DestroyChunk(this);
                    break;
            }
        }
    }
}
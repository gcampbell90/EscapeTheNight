﻿using UnityEngine;


namespace AmazingAssets.CurvedWorld.Example
{
    public class ChunkSpawner : MonoBehaviour
    {
        public enum AXIS { XPositive, XNegative, ZPositive, ZNegative }

        public GameObject[] chunks;
        public int initialSpawnCount = 5;
        public float destroyZone = 300;

        [Space(10)]
        public AXIS axis;

        [HideInInspector]
        public Vector3 moveDirection = new Vector3(-1, 0, 0);
        public float movingSpeed = 1;


        public float chunkSize = 60;        
        GameObject lastChunk;

        //public Texture2D lightmapColor;

        void Awake()
        {
            initialSpawnCount = initialSpawnCount > chunks.Length ? initialSpawnCount : chunks.Length;

            
            int chunkIndex = 0;
            for (int i = 0; i < initialSpawnCount; i++)
            {
                GameObject chunk = (GameObject)Instantiate(chunks[chunkIndex]);
                chunk.SetActive(true);

                chunk.GetComponent<RunnerChunk>().spawner = this;
               

                switch (axis)
                {
                    case AXIS.XPositive:
                        chunk.transform.localPosition = new Vector3(-i * chunkSize, 0, transform.position.z);
                        moveDirection = new Vector3(1, 0, 0);
                        break;

                    case AXIS.XNegative:
                        chunk.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                        moveDirection = new Vector3(-1, 0, 0);
                        break;

                    case AXIS.ZPositive:
                        chunk.transform.position = new Vector3(i * chunkSize, 0, transform.position.z);
                        moveDirection = new Vector3(0, 0, 1);
                        break;

                    case AXIS.ZNegative:
                        chunk.transform.localPosition = new Vector3(transform.position.x, 0, i * chunkSize);
                        moveDirection = new Vector3(0, 0, -1);
                        break;
                }
                

                lastChunk = chunk;

                if (++chunkIndex >= chunks.Length)
                    chunkIndex = 0;
            }           
        }
        
        public void DestroyChunk(RunnerChunk thisChunk)
        {
            Vector3 newPos = lastChunk.transform.position;
            switch (axis)
            {
                case AXIS.XPositive:
                    newPos.x -= chunkSize;
                    break;
                case AXIS.XNegative:
                    newPos.x += chunkSize;
                    break;
                case AXIS.ZPositive:
                    newPos.z -= chunkSize;
                    break;
                case AXIS.ZNegative:
                    newPos.z += chunkSize;
                    break;
            }
           
            lastChunk = thisChunk.gameObject;
            lastChunk.transform.position = newPos;

        }
    }
}

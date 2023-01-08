using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Static instance of the object pool
    public static ObjectPool instance;

    // Prefab of the object that we want to pool
    public GameObject pooledObject;

    // List to store the pooled objects
    private List<GameObject> pooledObjects;

    private void Awake()
    {
        // Set the static instance to this object
        instance = this;
    }

    private void Start()
    {
        // Initialize the list of pooled objects
        pooledObjects = new List<GameObject>();
    }

    // Method to retrieve an object from the pool
    public GameObject GetFromPool()
    {
        // Check if there is an available object in the pool
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // If the object is not active, return it
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        // If no available objects were found, instantiate a new one
        GameObject obj = Instantiate(pooledObject);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    // Method to return an object to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}


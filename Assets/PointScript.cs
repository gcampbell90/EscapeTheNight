using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public int value { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Value: " + value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

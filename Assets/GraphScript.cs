using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphScript : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject xAxisPrefab;
    public GameObject yAxisPrefab;
    public GameObject xTickPrefab;
    public GameObject yTickPrefab;
    public GameObject valueTextPrefab;
    public float tickSize = 0.1f;
    public float xMin = 0f;
    public float xMax = 10f;
    public float yMin = 0f;
    public float yMax = 10f;
    public float[] sampleData = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };

    void Start()
    {
        // Draw x-axis
        GameObject xAxis = Instantiate(xAxisPrefab, Vector3.zero, Quaternion.identity);
        xAxis.transform.parent = transform;
        xAxis.transform.localScale = new Vector3((xMax - xMin), tickSize, tickSize);

        // Draw y-axis
        GameObject yAxis = Instantiate(yAxisPrefab, Vector3.zero, Quaternion.identity);
        yAxis.transform.parent = transform;
        yAxis.transform.localScale = new Vector3(tickSize, (yMax - yMin), tickSize);

        // Draw x-axis tick marks
        for (float x = xMin + 1; x < xMax; x++)
        {
            GameObject xTick = Instantiate(xTickPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            xTick.transform.parent = transform;
            xTick.transform.localScale = new Vector3(tickSize, tickSize, tickSize);
        }

        // Draw y-axis tick marks
        for (float y = yMin + 1; y < yMax; y++)
        {
            GameObject yTick = Instantiate(yTickPrefab, new Vector3(0, y, 0), Quaternion.identity);
            yTick.transform.parent = transform;
            yTick.transform.localScale = new Vector3(tickSize, tickSize, tickSize);
        }

        // Draw sample data points
        for (int i = 0; i < sampleData.Length; i++)
        {
            float x = (float)i / (sampleData.Length - 1) * (xMax - xMin) + xMin;
            float y = sampleData[i];
            GameObject point = Instantiate(pointPrefab, new Vector3(x, y, 0), Quaternion.identity);
            GameObject text = Instantiate(valueTextPrefab, point.transform.position, Quaternion.identity, point.transform);
            point.transform.parent = transform;
            int value = (int)sampleData[i];
            text.GetComponent<TextMeshProUGUI>().text = value.ToString();

            point.GetComponent<PointScript>().value = value;
        }
    }
}


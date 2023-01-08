using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    // The number of rows and columns in the grid
    public int rows = 10;
    public int columns = 10;

    // The size of each cell in the grid
    public float cellSize = 1f;

    // The prefab for the cells
    public GameObject cellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Loop through the rows and columns and instantiate the cell prefabs
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Calculate the position of the cell
                float xPos = j * cellSize;
                float yPos = i * cellSize;

                // Instantiate the cell prefab and set its position
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.transform.position = new Vector3(xPos, yPos, 0);
            }
        }
    }
}

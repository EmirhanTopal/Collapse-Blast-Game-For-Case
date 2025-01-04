using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class GridManager : MonoBehaviour
{
    private static int _rows = 10;
    private static int _columns = 10;
    [SerializeField] private List<GameObject> gridBackground = new List<GameObject>();
    //private int[,] _customGrid = new int[_rows, _columns];
    private List<GameObject> gridBox = new List<GameObject>();
    HashSet<BoxManager> colorBoxHash = new HashSet<BoxManager>();
    private void Start()
    {
        DrawGrid();
    }
    
    private void DrawGrid()
    {
        int gridRow = 0;
        while (gridRow < _rows)
        {
            int gridColumn = 0;
            while (gridColumn < _columns)
            {
                Vector2 v2 = new Vector2(gridRow, gridColumn);
                
                GameObject gridGo = Instantiate(gridBackground[UnityEngine.Random.Range(0, gridBackground.Count)],v2, quaternion.identity);
                gridBox.Add(gridGo);
                gridColumn++;
            }
            gridRow++;
        }
    }

    public void ClickListenerHelp(BoxManager boxManager, int row, int column, int colorNumber)
    {
        colorBoxHash.Clear();
        colorBoxHash = ClickListenerBoxManager(boxManager, row, column, colorNumber, colorBoxHash);
        foreach (var cbh in colorBoxHash)
        {
            if (colorBoxHash.Count > 1)
            {
                Debug.Log(cbh.Row + "," + cbh.Column);
                gridBox.Remove(cbh.gameObject);
                Destroy(cbh.gameObject);
            }
        }
        colorBoxHash.Clear();
    }
    
    private HashSet<BoxManager> ClickListenerBoxManager(BoxManager boxManager, int row, int column, int colorNumber, HashSet<BoxManager> hashSet)
    {
        if (!hashSet.Contains(boxManager))
            hashSet.Add(boxManager);
        foreach (var grid in gridBox)
        {
            var neighbor = grid.GetComponent<BoxManager>();
            if (neighbor == null || hashSet.Contains(neighbor))
                continue;
            if (Mathf.Approximately(grid.transform.position.x, column) && Mathf.Approximately(grid.transform.position.y, row - 1) && colorNumber == neighbor.ColorNumber)
            {
                hashSet.Add(neighbor);
                Debug.Log("altında var");
                Debug.Log($"{boxManager.Row}, {boxManager.Column}");
                ClickListenerBoxManager(grid.GetComponent<BoxManager>(), row - 1, column, colorNumber, hashSet);
            }
            if (Mathf.Approximately(grid.transform.position.x, column) && Mathf.Approximately(grid.transform.position.y, row + 1) && colorNumber == neighbor.ColorNumber)
            {
                hashSet.Add(neighbor);
                Debug.Log("üstünde var");
                Debug.Log($"{boxManager.Row}, {boxManager.Column}");
                ClickListenerBoxManager(grid.GetComponent<BoxManager>(), row + 1, column, colorNumber, hashSet);
            }
            if (Mathf.Approximately(grid.transform.position.x, column + 1) && Mathf.Approximately(grid.transform.position.y, row) && colorNumber == neighbor.ColorNumber)
            {
                hashSet.Add(neighbor);
                Debug.Log("sağında var");
                Debug.Log($"{boxManager.Row}, {boxManager.Column}");
                ClickListenerBoxManager(grid.GetComponent<BoxManager>(), row, column + 1, colorNumber, hashSet);
            }
            if (Mathf.Approximately(grid.transform.position.x, column - 1) && Mathf.Approximately(grid.transform.position.y, row) && colorNumber == neighbor.ColorNumber)
            { 
                hashSet.Add(neighbor);
                Debug.Log("solunda var");
                Debug.Log($"{boxManager.Row}, {boxManager.Column}");
                ClickListenerBoxManager(grid.GetComponent<BoxManager>(), row, column - 1, colorNumber, hashSet);
            }
        }

        return hashSet;
    }
    
}

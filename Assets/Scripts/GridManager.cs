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
    [SerializeField] private List<GameObject> gridBackground = new List<GameObject>();
    private List<GameObject> _gridBox = new List<GameObject>();
    private HashSet<BoxManager> _colorBoxHash = new HashSet<BoxManager>();
    private HashSet<BoxManager> _visitedBoxHash = new HashSet<BoxManager>();
    
    private static int _rows = 6;
    private static int _columns = 6;
    private void Start()
    {
        InitialGrid();
        IntBoxGroupHelp();
    }
    
    private void InitialGrid()
    {
        int gridRow = 0;
        while (gridRow < _rows)
        {
            int gridColumn = 0;
            while (gridColumn < _columns)
            {
                Vector2 v2 = new Vector2(gridRow, gridColumn);
                
                GameObject gridGo = Instantiate(gridBackground[UnityEngine.Random.Range(0, gridBackground.Count)],v2, quaternion.identity);
                _gridBox.Add(gridGo);
                gridColumn++;
            }
            gridRow++;
        }
    }

    public void ClickListenerHelp(BoxManager boxManager, int row, int column, int colorNumber)
    {
        _colorBoxHash = ClickListenerBoxManager(boxManager, row, column, colorNumber, _colorBoxHash);
        foreach (var cbh in _colorBoxHash)
        {
            if (_colorBoxHash.Count > 1)
            {
                _gridBox.Remove(cbh.gameObject);
                Destroy(cbh.gameObject);
            }
        }
        _colorBoxHash.Clear();
        IntBoxGroupHelp();
    }
    
    private HashSet<BoxManager> ClickListenerBoxManager(BoxManager boxManager, int row, int column, int colorNumber, HashSet<BoxManager> hashSet)
    {
        if (!hashSet.Contains(boxManager))
            hashSet.Add(boxManager);
        foreach (var grid in _gridBox)
        {
            var neighbor = grid.GetComponent<BoxManager>();
            if (neighbor == null || hashSet.Contains(neighbor))
                continue;
            if (Mathf.Approximately(grid.transform.position.x, column) && Mathf.Approximately(grid.transform.position.y, row - 1) && colorNumber == neighbor.ColorNumber)
            {
                hashSet.Add(neighbor);
                ClickListenerBoxManager(neighbor, row - 1, column, colorNumber, hashSet);
            }
            if (Mathf.Approximately(grid.transform.position.x, column) && Mathf.Approximately(grid.transform.position.y, row + 1) && colorNumber == neighbor.ColorNumber)
            {
                hashSet.Add(neighbor);
                ClickListenerBoxManager(neighbor, row + 1, column, colorNumber, hashSet);
            }
            if (Mathf.Approximately(grid.transform.position.x, column + 1) && Mathf.Approximately(grid.transform.position.y, row) && colorNumber == neighbor.ColorNumber)
            {
                hashSet.Add(neighbor);
                ClickListenerBoxManager(neighbor, row, column + 1, colorNumber, hashSet);
            }
            if (Mathf.Approximately(grid.transform.position.x, column - 1) && Mathf.Approximately(grid.transform.position.y, row) && colorNumber == neighbor.ColorNumber)
            { 
                hashSet.Add(neighbor);
                ClickListenerBoxManager(neighbor, row, column - 1, colorNumber, hashSet);
            }
        }
        return hashSet;
    }

    private void IntBoxGroupHelp()
    {
        _visitedBoxHash.Clear();
        
        HashSet<BoxManager> tempHash = new HashSet<BoxManager>();
        foreach (var gridHelp in _gridBox)
        {
            var officialGridHelp = gridHelp.GetComponent<BoxManager>();
            tempHash = IntBoxGroup(officialGridHelp, officialGridHelp.Row, officialGridHelp.Column, officialGridHelp.ColorNumber, tempHash, _visitedBoxHash); //c
            Debug.Log(tempHash.Count);
            tempHash.Clear();
        }

    }
    private HashSet<BoxManager> IntBoxGroup(BoxManager boxManager, int row, int column, int colorNumber, HashSet<BoxManager> hashSet2, HashSet<BoxManager> visitedHash)
    {
        if (visitedHash.Contains(boxManager))
            return hashSet2;
        if (!hashSet2.Contains(boxManager))
            hashSet2.Add(boxManager);
        foreach (var grid in _gridBox)
        {
            var neighbor = grid.GetComponent<BoxManager>();
            if (Mathf.Approximately(grid.transform.position.x, column) && Mathf.Approximately(grid.transform.position.y, row - 1) && colorNumber == neighbor.ColorNumber)
            {
                hashSet2.Add(neighbor);
                visitedHash.Add(boxManager);
                IntBoxGroup(neighbor, row - 1, column, colorNumber, hashSet2, visitedHash);
            }
            if (Mathf.Approximately(grid.transform.position.x, column) && Mathf.Approximately(grid.transform.position.y, row + 1) && colorNumber == neighbor.ColorNumber)
            {
                hashSet2.Add(neighbor);
                visitedHash.Add(boxManager);
                IntBoxGroup(neighbor, row + 1, column, colorNumber, hashSet2, visitedHash);
            }
            if (Mathf.Approximately(grid.transform.position.x, column + 1) && Mathf.Approximately(grid.transform.position.y, row) && colorNumber == neighbor.ColorNumber)
            {
                hashSet2.Add(neighbor);
                visitedHash.Add(boxManager);
                IntBoxGroup(neighbor, row, column + 1, colorNumber, hashSet2, visitedHash);
            }
            if (Mathf.Approximately(grid.transform.position.x, column - 1) && Mathf.Approximately(grid.transform.position.y, row) && colorNumber == neighbor.ColorNumber)
            { 
                hashSet2.Add(neighbor);
                visitedHash.Add(boxManager);
                IntBoxGroup(neighbor, row, column - 1, colorNumber, hashSet2, visitedHash);
            }
        }
        return hashSet2;
    }
    
}

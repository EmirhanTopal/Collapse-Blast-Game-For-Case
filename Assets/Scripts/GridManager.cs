using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridBackground = new List<GameObject>();
    [SerializeField] private BoxFeatures features;
    private List<GameObject> _gridBox = new List<GameObject>();
    private List<BoxManager> _colorBoxHash = new List<BoxManager>();
    private HashSet<BoxManager> _visitedBoxHash = new HashSet<BoxManager>();
    
    private static int _gridRows = 10;
    private static int _gridColumns = 10;

    public List<GameObject> TempGridBox
    {
        get { return _gridBox; }
    }
    public int GridRows
    {
        get { return _gridRows; }
    }
    public int GridColumns
    {
        get { return _gridColumns; }
    }
    
    private static int _totalSize = _gridRows * _gridColumns;
    private List<GameObject> _myBox = new List<GameObject>();
    
    [SerializeField] private int changeA;
    [SerializeField] private int changeB;
    [SerializeField] private int changeC;

    public int ChangeA
    {
        get { return changeA; }
    }
    public int ChangeB
    {
        get { return changeB; }
    }
    public int ChangeC
    {
        get { return changeC; }
    }
    
    private void Start()
    {
        InitialGrid();
        IntBoxGroupHelp();
    }

    private void Update()
    {
        IntBoxGroupHelp();
    }

    private void InitialGrid()
    {
        int gridRow = 0;
        while (gridRow < _gridRows)
        {
            int gridColumn = 0;
            while (gridColumn < _gridColumns)
            {
                Vector2 v2 = new Vector2(gridRow, gridColumn);
                GameObject gridGo = Instantiate(gridBackground[Random.Range(0, gridBackground.Count)],v2, quaternion.identity);
                _gridBox.Add(gridGo);
                gridColumn++;
            }
            gridRow++;
        }
    }

    private List<GameObject> DestroyGroup(BoxManager boxManager, int row, int column, int colorNumber)
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
        return _myBox;
    }
    
    public void ClickListenerHelp(BoxManager boxManager, int row, int column, int colorNumber)
    {
        DestroyGroup(boxManager,row,column,colorNumber);
        _colorBoxHash.Clear();
        NewGridBoxMain();
        IntBoxGroupHelp();
    }
    
    private List<BoxManager> ClickListenerBoxManager(BoxManager boxManager, int row, int column, int colorNumber, List<BoxManager> hashSet)
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
        List<GameObject> copyGridBox = new List<GameObject>(_gridBox);
        foreach (var copyGridHelp in copyGridBox)
        {
            var officialGridHelp = copyGridHelp.GetComponent<BoxManager>();
            tempHash = IntBoxGroup(officialGridHelp, officialGridHelp.Row, officialGridHelp.Column, officialGridHelp.ColorNumber, tempHash, _visitedBoxHash);
            ChangeShape(tempHash);
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

    private void ChangeShape(HashSet<BoxManager> tempHash)
    {
        int hashCount = tempHash.Count;
        
        foreach (var tempBox in tempHash)
        {
            int tempColorNumber = tempBox.ColorNumber;
            if (hashCount >= ChangeA && hashCount < ChangeB)
            {
                tempBox.gameObject.GetComponent<SpriteRenderer>().sprite = features.Ateam[tempColorNumber - 1];
            }
            else if (hashCount >= ChangeB && hashCount < ChangeC)
            {
                tempBox.gameObject.GetComponent<SpriteRenderer>().sprite = features.Bteam[tempColorNumber - 1];
            }
            else if (hashCount >= ChangeC)
            {
                tempBox.gameObject.GetComponent<SpriteRenderer>().sprite = features.Cteam[tempColorNumber - 1];
            }
            else
            {
                tempBox.gameObject.GetComponent<SpriteRenderer>().sprite = features.Dteam[tempColorNumber - 1];
            }
        }
    }
    
    private void NewGridBoxMain()
    {
        _myBox.Clear();
        for (int row = 0; row < _gridRows; row++)
        {
            for (int column = 0; column < _gridColumns; column++)
            {
                bool flag = false;
                
                foreach (var grid in _gridBox) 
                { 
                    int gridPosX = Convert.ToInt32(grid.transform.position.x); 
                    int gridPosY = Convert.ToInt32(grid.transform.position.y);
                    if (gridPosX == column && gridPosY == row) 
                    { 
                        flag = true;
                    }
                }
                if (!flag)
                {
                    Vector2 newV2 = new Vector2(column, _gridRows + row + 2);
                    GameObject gridGo = Instantiate(gridBackground[Random.Range(0, gridBackground.Count)], newV2, quaternion.identity);
                    _gridBox.Add(gridGo);
                }
            }
        }
    }
}

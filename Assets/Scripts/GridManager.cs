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
    
    private static int totalSize = _gridRows * _gridColumns;
    private List<GameObject> myBox = new List<GameObject>();
    GameObject[] _addBox = new GameObject[totalSize];
    int[,] _addBoxPos = new int[totalSize, totalSize];
    
    [SerializeField] private int changeA;
    [SerializeField] private int changeB;
    [SerializeField] private int changeC;

    private int _changingSpeed = 100;
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
        //MoveObjects();
        IntBoxGroupHelp();// optimizasyon için bakılacak.
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
        return myBox;
    }
    
    public void ClickListenerHelp(BoxManager boxManager, int row, int column, int colorNumber)
    {
        DestroyGroup(boxManager,row,column,colorNumber);
        _colorBoxHash.Clear();
        //StartCoroutine(FallBoxMain());
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
    
    private IEnumerator FallBoxMain()
    {
        bool flag = true;
        while (flag)
        {
            flag = false; // Döngüyü sonlandırmayı dene
            List<GameObject> gridTemp = new List<GameObject>(_gridBox);
            foreach (var grid in gridTemp)
            {
                int gridPosX = Convert.ToInt32(grid.transform.position.x);
                int gridPosY = Convert.ToInt32(grid.transform.position.y - 1);
                bool flag2 = true;
                foreach (var grid2 in _gridBox)
                {
                    if (Mathf.Approximately(grid.transform.position.y - 1, grid2.transform.position.y) && Mathf.Approximately(grid.transform.position.x, grid2.transform.position.x))
                    {
                        flag2 = false;
                    }
                }
                if (flag2 && gridPosY > 0)
                {
                    flag = true;
                    Vector3 targetV3 = new Vector3(grid.transform.position.x, grid.transform.position.y - 1, grid.transform.position.z);
                    yield return StartCoroutine(FallBoxIE(grid, targetV3, 1));
                }
            }
        }
    }


    IEnumerator FallBoxIE(GameObject gridGo, Vector3 gridTargetV3, int speed)
    {
        while (gridGo && gridGo.transform.position != gridTargetV3)
        {
            gridGo.transform.position = Vector2.Lerp(gridGo.transform.position,gridTargetV3,speed);
            yield return null;
        }

        if (gridGo)
        {
            gridGo.transform.position = gridTargetV3;
        }
    }
    
    private void NewGridBoxMain()
    {
        myBox.Clear();
        int index = 0;
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
                    // _addBox[index] = gridGo;
                    // _addBoxPos[index, 0] = row;
                    // _addBoxPos[index, 1] = column;
                    _gridBox.Add(gridGo);
                    index++;
                }
            }
        }

        // bool flagControl = true;
        // while (flagControl)
        // {
        //     flagControl = false;
        //     int indexWhile = 0;
        //     while (indexWhile < _addBox.Length)
        //     {
        //         if (_addBox[indexWhile] != null)
        //         {
        //             int boxPosX = Convert.ToInt32(_addBox[indexWhile].transform.position.x);
        //             int boxPosY = Convert.ToInt32(_addBox[indexWhile].transform.position.y);
        //             if (boxPosY > _addBoxPos[indexWhile, 0] && boxPosX == _addBoxPos[indexWhile, 1])
        //             {
        //                 flagControl = true;
        //                 Vector2 newVector2 = new Vector2(_addBox[indexWhile].transform.position.x, _addBox[indexWhile].transform.position.y - 1);
        //                 _addBox[indexWhile].transform.position = newVector2;
        //             }
        //         }
        //         indexWhile++;
        //     }
        //     //buraya 1 saniye bekleme süresi eklenecek
        //     //obje bazlı deneme yapılacak - lets think
        // }
    }
}

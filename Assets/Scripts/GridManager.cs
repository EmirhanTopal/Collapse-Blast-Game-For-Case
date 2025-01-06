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
    private HashSet<BoxManager> _colorBoxHash = new HashSet<BoxManager>();
    private HashSet<BoxManager> _visitedBoxHash = new HashSet<BoxManager>();
    
    private static int _gridRows = 4;
    private static int _gridColumns = 4;
    
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
        NewGridBox();
        IntBoxGroupHelp(); // optimizasyon için bakılacak.
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

    private void DestroyGroup(BoxManager boxManager, int row, int column, int colorNumber)
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
    }
    
    public void ClickListenerHelp(BoxManager boxManager, int row, int column, int colorNumber)
    {
        DestroyGroup(boxManager,row,column,colorNumber);
        _colorBoxHash.Clear();
        FallBox();
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
            Vector2 newAVector2 = new Vector2(tempBox.transform.position.x, tempBox.transform.position.y);
            int tempColorNumber = tempBox.ColorNumber;
            if (hashCount >= ChangeA && hashCount < ChangeB)
            {
                GameObject newA = Instantiate(features.Ateam[tempColorNumber - 1], newAVector2, quaternion.identity);
                _gridBox.Add(newA);
                _gridBox.Remove(tempBox.gameObject);
                Destroy(tempBox.gameObject);
            }
            else if (hashCount >= ChangeB && hashCount < ChangeC)
            {
                GameObject newB = Instantiate(features.Bteam[tempColorNumber - 1], newAVector2, quaternion.identity);
                _gridBox.Add(newB);
                _gridBox.Remove(tempBox.gameObject);
                Destroy(tempBox.gameObject);
            }
            else if (hashCount >= ChangeC)
            {
                GameObject newC = Instantiate(features.Cteam[tempColorNumber - 1], newAVector2, quaternion.identity);
                _gridBox.Add(newC);
                _gridBox.Remove(tempBox.gameObject);
                Destroy(tempBox.gameObject);
            }
            else
            {
                GameObject newD = Instantiate(features.Dteam[tempColorNumber - 1], newAVector2, quaternion.identity);
                _gridBox.Add(newD);
                _gridBox.Remove(tempBox.gameObject);
                Destroy(tempBox.gameObject);
            }
        }
    }

    private void FallBox()
    {
        bool flag = true;
        while (flag)
        {
            foreach (var grid in _gridBox)
            {
                int gridPosX = Convert.ToInt32(grid.transform.position.x);
                int gridPosY = Convert.ToInt32(grid.transform.position.y);
                bool flag2 = true;
                foreach (var grid2 in _gridBox)
                {
                    int grid2PosX = Convert.ToInt32(grid2.transform.position.x);
                    int grid2PosY = Convert.ToInt32(grid2.transform.position.y);
                    if (gridPosY - 1 == grid2PosY && gridPosX == grid2PosX)
                    {
                        flag2 = false;
                        break;
                    }
                }
                if (flag2 && gridPosY > 0)
                {
                    flag = true;
                    Vector2 targetV2 = new Vector2(grid.transform.position.x, grid.transform.position.y - 1);
                    StartCoroutine(FallBoxIE(grid, targetV2, 100));
                    break;
                }
                else
                    flag = false;
            }
        }
    }

    IEnumerator FallBoxIE(GameObject gridGo, Vector2 gridTargetV2, int speed)
    {
        while (gridGo && Vector2.Distance(gridGo.transform.position, gridTargetV2) > 0.01f)
        {
            gridGo.transform.position = Vector2.Lerp(gridGo.transform.position,gridTargetV2,Time.deltaTime * speed);
            yield return null;
        }

        if (gridGo)
        {
            gridGo.transform.position = gridTargetV2;
        }
    }
    
    private void NewGridBox()
    {
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
                        break;
                    }
                }
                if (!flag)
                {
                    Vector2 newV2 = new Vector2(column, row);
                    GameObject gridGo = Instantiate(gridBackground[Random.Range(0, gridBackground.Count)], newV2, quaternion.identity);
                    _gridBox.Add(gridGo);
                    break;
                }
            }
            
        }
    }

    private IEnumerator NewBoxIE()
    {
        
    }    
}

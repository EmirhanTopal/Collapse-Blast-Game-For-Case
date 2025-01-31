using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridBackground = new List<GameObject>();
    [SerializeField] private BoxFeatures features;
    [SerializeField] private GameObject panel;
    
    private List<int> _groupCounts = new List<int>();
    private List<GameObject> _gridBox = new List<GameObject>();
    private List<BoxManager> _colorBoxHash = new List<BoxManager>();
    private HashSet<BoxManager> _visitedBoxHash = new HashSet<BoxManager>();
    private HashSet<BoxManager> _tempHash = new HashSet<BoxManager>();
    
    private int _changeA;
    private int _changeB;
    private int _changeC;
    private static int _gridRows;
    private static int _gridColumns;
    private static int _colorNumbers;
    private int _test = 0;
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

    public int ChangeA
    {
        get { return _changeA; }
    }
    public int ChangeB
    {
        get { return _changeB; }
    }
    public int ChangeC
    {
        get { return _changeC; }
    }

    private float tempTime = 0.2f;
    private float durationTime;
    
    private void Start()
    {
        _gridRows = UIScript.StringRow;
        _gridColumns = UIScript.StringColumn;
        _colorNumbers = UIScript.StringColor;
        _changeA = UIScript.StringA;
        _changeB = UIScript.StringB;
        _changeC = UIScript.StringC;
        InitialGrid();
        _test = IntBoxGroupHelp();
        CheckGameOver(_test);
    }
    
    public void ClickListenerHelp(BoxManager boxManager, int row, int column, int colorNumber)
    {
        DestroyGroup(boxManager,row,column,colorNumber);
        _colorBoxHash.Clear();
        NewGridBoxMain();
        StartCoroutine(WaitAndUpdate());
    }
    
    private bool CheckForPossibleMatches()
    {
        foreach (var grid in _gridBox)
        {
            var box = grid.GetComponent<BoxManager>();
            if (box == null)
                continue;
            
            foreach (var neighbor in GetNeighbors(box))
            {
                if (neighbor.ColorNumber == box.ColorNumber)
                    return true;
            }
        }
        return false;
    }
    
    private List<BoxManager> GetNeighbors(BoxManager box)
    {
        List<BoxManager> neighbors = new List<BoxManager>();
        foreach (var grid in _gridBox)
        {
            var neighbor = grid.GetComponent<BoxManager>();
            if (neighbor == null || neighbor == box)
                continue;
            
            if (Mathf.Approximately(Mathf.Abs(neighbor.transform.position.x - box.transform.position.x) + Mathf.Abs(neighbor.transform.position.y - box.transform.position.y), 1))
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    public void ShuffleGridSmart()
    {
        bool matchFound = false;
        
        for (int i = 0; i < _gridBox.Count; i++)
        {
            for (int j = i + 1; j < _gridBox.Count; j++)
            {
                SwapBoxes(i, j);

                if (CheckForPossibleMatches())
                {
                    matchFound = true;
                    break;
                }

                SwapBoxes(i, j);
            }

            if (matchFound)
                break;
        }
        if (!matchFound)
        {
            Debug.Log("No matches found, performing random shuffle.");
            ShuffleGrid();
        }
    }

    private void SwapBoxes(int indexA, int indexB)
    {
        var tempPos = _gridBox[indexA].transform.position;
        _gridBox[indexA].transform.position = _gridBox[indexB].transform.position;
        _gridBox[indexB].transform.position = tempPos;
    }

    public void ShuffleGrid()
    {
        List<GameObject> boxes = new List<GameObject>(_gridBox);
        
        for (int i = 0; i < boxes.Count; i++)
        {
            int randomIndex = Random.Range(0, boxes.Count);
            (boxes[i], boxes[randomIndex]) = (boxes[randomIndex], boxes[i]);
        }
        
        int index = 0;
        for (int row = 0; row < _gridRows; row++)
        {
            for (int col = 0; col < _gridColumns; col++)
            {
                boxes[index].transform.position = new Vector3(col, row, 0);
                index++;
            }
        }
    }

    private IEnumerator WaitAndUpdate()
    {
        yield return new WaitForSeconds(1f);
        _test = IntBoxGroupHelp();
        CheckGameOver(_test);
    }
    
    private void CheckGameOver(int testParam)
    {
        if (testParam <= 0)
        {
            ShuffleGridSmart();
        }
        _groupCounts.Clear();
    }
    private void InitialGrid()
    {
        int gridRow = 0;
        while (gridRow < _gridRows)
        {
            int gridColumn = 0;
            while (gridColumn < _gridColumns)
            {
                Vector2 v2 = new Vector2(gridColumn, gridRow);
                GameObject gridGo = Instantiate(gridBackground[Random.Range(0, _colorNumbers)],v2, quaternion.identity);
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

    private int IntBoxGroupHelp()
    {
        _visitedBoxHash.Clear();
        _tempHash.Clear();
        _groupCounts.Clear();
        List<GameObject> copyGridBox = new List<GameObject>(_gridBox);
        foreach (GameObject copyGridHelp in copyGridBox)
        {
            var officialGridHelp = copyGridHelp.GetComponent<BoxManager>();
            _tempHash = IntBoxGroup(officialGridHelp, officialGridHelp.Row, officialGridHelp.Column, officialGridHelp.ColorNumber, _tempHash, _visitedBoxHash);
            if (_tempHash.Count > 1)
            {
                _groupCounts.Add(_tempHash.Count);
            }
            ChangeShape(_tempHash);
            _tempHash.Clear();
        }
        _tempHash.Clear();
        return _groupCounts.Count;
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
            var sprite = tempBox.gameObject.GetComponent<SpriteRenderer>();
            int tempColorNumber = tempBox.ColorNumber;
            if (hashCount >= ChangeA && hashCount < ChangeB)
            {
                sprite.sprite = features.Ateam[tempColorNumber - 1];
            }
            else if (hashCount >= ChangeB && hashCount < ChangeC)
            {
                sprite.sprite = features.Bteam[tempColorNumber - 1];
            }
            else if (hashCount >= ChangeC)
            {
                sprite.sprite = features.Cteam[tempColorNumber - 1];
            }
            else
            {
                sprite.sprite = features.Dteam[tempColorNumber - 1];
            }
        }
    }
    
    private void NewGridBoxMain()
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
                    Vector2 newV2 = new Vector2(column, _gridRows + row + 2);
                    GameObject gridGo = Instantiate(gridBackground[Random.Range(0, _colorNumbers)], newV2, quaternion.identity);
                    _gridBox.Add(gridGo);
                }
            }
        }
    }
}

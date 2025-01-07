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
    
    
    private static int totalSize = _gridRows * _gridColumns;
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
        FallBoxMain();
        NewGridBoxMain();
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
    
    private void FallBoxMain()
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
                    }
                }
                if (flag2 && gridPosY > 0)
                {
                    flag = true;
                    Vector2 targetV2 = new Vector2(grid.transform.position.x, grid.transform.position.y - 1);
                    StartCoroutine(FallBoxReachTargetIE(grid, targetV2, 10));
                }
                else
                    flag = false;
            }
        }
    }

    IEnumerator FallBoxReachTargetIE(GameObject gridGo, Vector2 gridTargetV2, int speed)
    {
        
        while (gridGo && Vector2.Distance(gridGo.transform.position, gridTargetV2) > 0.01f)
        {
            gridGo.transform.position = Vector2.Lerp(gridGo.transform.position,gridTargetV2, speed);
            yield return null;
        }

        if (gridGo)
        {
            gridGo.transform.position = gridTargetV2;
        }
    }
    
    private void NewGridBoxMain()
    {
        int totalSize = _gridRows * _gridColumns;
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
                    //gamobjectlerin listesini al
                    //o anki gameobjectin row ve column değerlerini al
                    _addBox[index] = gridGo;
                    _addBoxPos[index, 0] = row;
                    _addBoxPos[index, 1] = column;
                    index++;
                    _gridBox.Add(gridGo);
                }
            }
        }

        bool flagControl = true;
        while (flagControl)
        {
            flagControl = false;
            int indexWhile = 0;
            while (indexWhile < _addBox.Length)
            {
                if (_addBox[indexWhile] != null)
                {
                    int boxPosX = Convert.ToInt32(_addBox[indexWhile].transform.position.x);
                    int boxPosY = Convert.ToInt32(_addBox[indexWhile].transform.position.y);
                    if (boxPosY > _addBoxPos[indexWhile, 0] && boxPosX == _addBoxPos[indexWhile, 1])
                    {
                        flagControl = true;
                        Vector2 newVector2 = new Vector2(_addBox[indexWhile].transform.position.x, _addBox[indexWhile].transform.position.y - 1);
                        _addBox[indexWhile].transform.position = newVector2;
                    }
                }
                indexWhile++;
            }
            //buraya 1 saniye bekleme süresi eklenecek
        }
    }

    private IEnumerator UpdateGridBoxPositions(List<GameObject> addBox)
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1f);  // Her iterasyon arasında 1 saniye bekle
        
            HelpNewGridBox(addBox); // Pozisyon güncellemeyi burada çağır
        }
    }

    void HelpNewGridBox(List<GameObject> myAddBox)
    {
        foreach (var aGameObject in myAddBox)
        {
            if (aGameObject != null)
            {
                Debug.Log("sa"); // buraya girmiyor - invoke dene
                Vector3 gridMyGo = new Vector3(aGameObject.transform.position.x, aGameObject.transform.position.y - 1, aGameObject.transform.position.z);
                aGameObject.transform.position = gridMyGo;
            }
        }
    }
}

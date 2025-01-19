using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class BoxManager : MonoBehaviour
{
    [SerializeField] private int colorNumber;
    
    private int _row;
    private int _column;
    private bool _isTarget = false;
    private Vector3 _targetV3;
    private Vector3 _startV3;
    private Vector3 _smallTargetV3;
    public int Row
    {
        get { return _row; }
    }
    public int Column
    {
        get { return _column; }
    }
    public int ColorNumber
    {
        get { return colorNumber; }
    }
    private GridManager _gridManager;
    private List<GameObject> myGridBox = new List<GameObject>();
    private Vector3 mytargetV3;

    private void Awake()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        myGridBox = _gridManager.TempGridBox;
        if(transform.position.y > _gridManager.GridRows - 1 && transform.position.x < _gridManager.GridColumns)
            _targetV3 = new Vector3(transform.position.x, transform.position.y - _gridManager.GridRows - 2, transform.position.z);
        _startV3 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        UpdatePosition();
    }


    private void Update()
    {
        UpdatePosition();
        
        
    }
    private void FixedUpdate()
    {
        // bool flag = false;
        // foreach (var gridBox in myGridBox)
        // {
        //     int gridBoxY = Convert.ToInt32(gridBox.transform.position.y);
        //     if (_startV3.x < _gridManager.GridColumns && _startV3.y < _gridManager.GridRows && (_startV3.x >= 0 || _startV3.y >= 0))
        //     {
        //         if (Convert.ToInt32(_startV3.y - 1) == gridBoxY)
        //         {
        //             flag = true;
        //         }
        //     }
        // }
        // if (!flag)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, _smallTargetV3, 10 * Time.deltaTime);
        //     if (Mathf.Approximately(_smallTargetV3.y, transform.position.y) && Mathf.Approximately(_smallTargetV3.x, transform.position.x))
        //     {
        //         transform.position = _smallTargetV3;
        //     }
        // }
        
        mytargetV3 = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        if (_startV3.y > 0 && mytargetV3.y >= 0)
        {
            Debug.Log(mytargetV3);
            bool fallControl = true;
            foreach (var grid in myGridBox)
            {
                int gridX = Convert.ToInt32(grid.transform.position.x);
                int gridY = Convert.ToInt32(grid.transform.position.y);
                if (Convert.ToInt32(transform.position.x) == gridX && Convert.ToInt32(transform.position.y - 1) == gridY)
                {
                    fallControl = false;
                }
            }
            if (fallControl)
            {
                transform.position = Vector3.MoveTowards(transform.position, mytargetV3, 50 * Time.deltaTime);
                if (Mathf.Approximately(transform.position.x, mytargetV3.x) && Mathf.Approximately(transform.position.y, mytargetV3.y))
                {
                    transform.position = mytargetV3;
                }
            }
            
        }
        // if (_startV3.x < _gridManager.GridColumns && _startV3.y > _gridManager.GridRows && (_startV3.x >= 0 || _startV3.y >= 0))
        // {
        //     if (!_isTarget)
        //     {
        //         transform.position = Vector3.MoveTowards(transform.position, _targetV3, 10 * Time.deltaTime);
        //         if (_targetV3.y == transform.position.y && _targetV3.x == transform.position.x)
        //         {
        //             _isTarget = true;
        //         }
        //     }
        //     else
        //     {
        //         transform.position = _targetV3;
        //     }
        // }
    }

    private void UpdatePosition()
    {
        _column = Convert.ToInt32(transform.position.x);
        _row = Convert.ToInt32(transform.position.y);
    }
    private void OnMouseDown()
    {
        _gridManager.ClickListenerHelp(this, _row, _column, colorNumber);
    }
}

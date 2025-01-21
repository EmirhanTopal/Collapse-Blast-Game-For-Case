using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxManager : MonoBehaviour
{
    [SerializeField] private int colorNumber;
    
    private int _row;
    private int _column;
    private Vector3 _startV3;
    private Vector3 _mytargetV3;
    private GridManager _gridManager;
    private List<GameObject> _myGridBox = new List<GameObject>();
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

    private void Awake()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        _myGridBox = _gridManager.TempGridBox;
        _startV3 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        UpdatePosition();
    }


    private void Update()
    {
        UpdatePosition();
        
    }
    private void FixedUpdate()
    {
        _mytargetV3 = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        if (_startV3.y > 0 && _mytargetV3.y >= 0)
        {
            bool fallControl = true;
            foreach (var grid in _myGridBox)
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
                transform.position = Vector3.MoveTowards(transform.position, _mytargetV3, 50 * Time.fixedDeltaTime);
                if (Mathf.Approximately(transform.position.x, _mytargetV3.x) && Mathf.Approximately(transform.position.y, _mytargetV3.y))
                {
                    transform.position = _mytargetV3;
                }
            }
        }
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

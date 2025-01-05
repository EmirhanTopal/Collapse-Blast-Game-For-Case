using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxManager : MonoBehaviour
{
    private int _row;
    private int _column;
    [SerializeField] private int colorNumber;
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

    private void Awake()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        _column = Convert.ToInt32(transform.position.x);
        _row = Convert.ToInt32(transform.position.y);
    }

    private void OnMouseDown()
    {
        _gridManager.ClickListenerHelp(this, _row, _column, colorNumber);
    }
}

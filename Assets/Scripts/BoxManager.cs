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
        if(transform.position.y > _gridManager.GridRows - 1 && transform.position.x < _gridManager.GridColumns)
            _targetV3 = new Vector3(transform.position.x, transform.position.y - _gridManager.GridRows - 2, transform.position.z);
        _startV3 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Debug.Log("target: " + _targetV3);
        UpdatePosition();
    }


    private void Update()
    {
        UpdatePosition();
    }
    private void FixedUpdate()
    {
        if (_targetV3 != Vector3.zero)
        {
            if (!_isTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetV3, 10 * Time.deltaTime);
                if (Mathf.Approximately(_targetV3.y, transform.position.y) && Mathf.Approximately(_targetV3.x, transform.position.x))
                {
                    _isTarget = true;
                }
            }
            else
            {
                transform.position = _targetV3;
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

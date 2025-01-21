using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField row;
    [SerializeField] private TMP_InputField column;
    [SerializeField] private TMP_InputField color;
    [SerializeField] private TMP_InputField a;
    [SerializeField] private TMP_InputField b;
    [SerializeField] private TMP_InputField c;
    
    public static int StringRow;
    public static int StringColumn;
    public static int StringColor;
    public static int StringA;
    public static int StringB;
    public static int StringC;

    
    public void InputGrid()
    {
        bool boolRow = false;
        bool boolColumn = false;
        bool boolColor = false;
        bool boolABC = false;
        try
        {
            StringRow = Convert.ToInt32(row.text);
            if (StringRow <= 10 && StringRow >= 2)
            {
                boolRow = true;
            }
        }
        catch (FormatException)
        {
            Debug.Log("please enter a number between 2 and 10");
            //error canvas open and write it down (please change Row and enter a number between 2 and 10)
        }
        catch (OverflowException)
        {
            Debug.Log("please enter a number between 2 and 10");
            //error canvas open and write it down (please change Row and enter a number between 2 and 10)
        }
        
        try
        {
            StringColumn = Convert.ToInt32(column.text);
            if (StringColumn <= 10 && StringColumn >= 1)
            {
                boolColumn = true;
            }
        }
        catch (FormatException)
        {
            Debug.Log("please enter a number between 1 and 10");
            //error canvas open and write it down (please change Column and enter a number between 1 and 10)
        }
        catch (OverflowException)
        {
            Debug.Log("please enter a number between 1 and 10");
            //error canvas open and write it down (please change Column and enter a number between 1 and 10)
        }
        
        try
        {
            StringColor = Convert.ToInt32(color.text);
            if (StringColor <= 6 && StringColumn >= 1)
            {
                boolColor = true;
            }
        }
        catch (FormatException)
        {
            Debug.Log("please enter a number between 1 and 6");
            //error canvas open and write it down (please change Color and enter a number between 1 and 6)
        }
        catch (OverflowException)
        {
            Debug.Log("please enter a number between 1 and 6");
            //error canvas open and write it down (please change Color and enter a number between 1 and 6)
        }
        
        try
        {
            StringA = Convert.ToInt32(a.text);
        }
        catch (FormatException)
        {
            Debug.Log("please change A and enter a number under B and C");
            //error canvas open and write it down (please change A and enter a number under B and C)
        }
        catch (OverflowException)
        {
            Debug.Log("please change A and enter a number under B and C");
            //error canvas open and write it down (please change A and enter a number under B and C)
        }
        
        try
        {
            StringB = Convert.ToInt32(b.text);
        }
        catch (FormatException)
        {
            Debug.Log("please change B and enter a number under C");
            //error canvas open and write it down (please change B and enter a number under C)
        }
        catch (OverflowException)
        {
            Debug.Log("please change B and enter a number under C");
            //error canvas open and write it down (please change B and enter a number under C)
        }
        
        try
        {
            StringC = Convert.ToInt32(c.text);
        }
        catch (FormatException)
        {
            Debug.Log("please change B and enter a number below A and B");
            //error canvas open and write it down (please change B and enter a number below A and B)
        }
        catch (OverflowException)
        {
            Debug.Log("please change B and enter a number below A and B");
            //error canvas open and write it down (please change B and enter a number below A and B)
        }

        if (StringA >= StringB)
        {
            Debug.Log("please change A and enter a number under B and C");
        }
        else if (StringA >= StringC)
        {
            Debug.Log("please change A and enter a number under B and C");
        }
        else if (StringB >= StringC)
        {
            Debug.Log("please change B and enter a number under C");
        }
        else
        {
            boolABC = true;
        }
        if (boolRow && boolColumn && boolColor && boolABC)
        {
            SceneManager.LoadScene("MainScene");
            
        }
    }
}

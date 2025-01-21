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
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    public static int StringRow;
    public static int StringColumn;
    public static int StringColor;
    public static int StringA;
    public static int StringB;
    public static int StringC;
    
    public void InputGrid()
    {
        int totalBox;
        try
        {
            StringRow = Convert.ToInt32(row.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            _textMeshProUGUI.text = "please enter a integer format and enter a number between 2 and 10";
        }
        catch (OverflowException)
        {
            Debug.Log("please enter a number between 2 and 10");
            //error canvas open and write it down (please change Row and enter a number between 2 and 10)
        }
        
        try
        {
            StringColumn = Convert.ToInt32(column.text);
        }
        catch (FormatException)
        {
            Debug.Log("please enter a integer format and enter a number between 1 and 10");
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
        }
        catch (FormatException)
        {
            Debug.Log("please enter a integer format and enter a number between 1 and 6");
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

        totalBox = StringRow * StringColumn;
        if (StringRow > 10 || StringRow < 2)
        {
            Debug.Log("Please enter a Row number under 10 and below 2");
        }
        else if (StringColumn > 10 || StringColumn < 1)
        {
            Debug.Log("Please enter a Column number under 10 and below 1");
        }
        else if (StringColor > 6 || StringColumn < 1)
        {
            Debug.Log("Please enter a Color number under 6 and below 1");
        }
        else if (StringA > totalBox || StringA < 2)
        {
            Debug.Log($"Please enter a 'A' number under {totalBox} and below 2");
        }
        else if (StringB > totalBox || StringB < 3)
        {
            Debug.Log($"Please enter a 'B' number under {totalBox} and below 3");
        }
        else if (StringC > totalBox || StringC < 4)
        {
            Debug.Log($"Please enter a 'C' number under {totalBox} and below 4");
        }
        else if (StringA >= StringB)
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
            SceneManager.LoadScene("MainScene");
        }
    }
}

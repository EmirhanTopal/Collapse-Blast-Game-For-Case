using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField row;
    [SerializeField] private TMP_InputField column;
    [SerializeField] private TMP_InputField color;
    [SerializeField] private TMP_InputField a;
    [SerializeField] private TMP_InputField b;
    [SerializeField] private TMP_InputField c;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    public static int StringRow;
    public static int StringColumn;
    public static int StringColor;
    public static int StringA;
    public static int StringB;
    public static int StringC;
    
    private int _totalBox;

    public void PanelOnOff()
    {
        panel.SetActive(false);
    }
    public void InputGrid()
    {
        bool hasError = false;
        try
        {
            StringRow = Convert.ToInt32(row.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a integer format and enter a number between 2 and 10";
            hasError = true;
        }
        catch (OverflowException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a integer format and enter a number between 2 and 10";
            hasError = true;
        }

        if (hasError) return;
        
        try
        {
            StringColumn = Convert.ToInt32(column.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a integer format and enter a number between 1 and 10";
            hasError = true;
        }
        catch (OverflowException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a integer format and enter a number between 1 and 10";
            hasError = true;
        }
        if (hasError) return;
        
        try
        {
            StringColor = Convert.ToInt32(color.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a integer format and enter a number between 1 and 6";
            hasError = true;
        }
        catch (OverflowException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a integer format and enter a number between 1 and 6";
            hasError = true;
        }
        if (hasError) return;
        
        try
        {
            StringA = Convert.ToInt32(a.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change A and enter a number under B and C";
            hasError = true;
        }
        catch (OverflowException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change A and enter a number under B and C";
            hasError = true;
        }
        if (hasError) return;
        try
        {
            StringB = Convert.ToInt32(b.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change B and enter a number under C";
            hasError = true;
        }
        catch (OverflowException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change B and enter a number under C";
            hasError = true;
        }
        if (hasError) return;
        
        try
        {
            StringC = Convert.ToInt32(c.text);
        }
        catch (FormatException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change B and enter a number below A and B";
            hasError = true;
        }
        catch (OverflowException)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change B and enter a number below A and B";
            hasError = true;
        }
        if (hasError) return;
        
        _totalBox = StringRow * StringColumn;
        if (StringRow > 10 || StringRow < 2)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a Row number under 10 and below 2";
        }
        else if (StringColumn > 10 || StringColumn < 1)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a Column number under 10 and below 1";
        }
        else if (StringColor > 6 || StringColumn < 1)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please enter a Color number under 6 and below 1";
        }
        else if (StringA > _totalBox || StringA <= 0)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = $"Please enter a 'A' number under {_totalBox} and below 0";
        }
        else if (StringB > _totalBox || StringB <= 0)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = $"Please enter a 'B' number under {_totalBox} and below 0";
        }
        else if (StringC > _totalBox || StringC <= 0)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = $"Please enter a 'C' number under {_totalBox} and below 0";
        }
        else if (StringA >= StringB)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change A and enter a number under B and C";
        }
        else if (StringA >= StringC)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change A and enter a number under B and C";
        }
        else if (StringB >= StringC)
        {
            panel.SetActive(true);
            textMeshProUGUI.text = "Please change B and enter a number under C";
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

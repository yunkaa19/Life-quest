using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionWindow : MonoBehaviour
{
    public GameObject mainPanel; 
    public Canvas canvas;

    public void HidePanel()
    {
        canvas.sortingOrder = 1;
        mainPanel.SetActive(false);
    }
    public void ShowPanel()
    {
        canvas.sortingOrder = 2;
        mainPanel.SetActive(true);
    }
}

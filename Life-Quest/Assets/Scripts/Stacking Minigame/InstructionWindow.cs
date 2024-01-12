using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionWindow : MonoBehaviour
{
    public GameObject mainPanel; 

    public void HidePanel()
    {
        mainPanel.SetActive(false);
    }
}

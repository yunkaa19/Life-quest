using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManagerHearing : MonoBehaviour
{
    public GameObject popUpCanvas;

    public void OpenPopUp()
    {
        popUpCanvas.SetActive(true);
        Debug.Log("click");
    }

    public void ClosePopUp()
    {
        popUpCanvas.SetActive(false);
        Debug.Log("click2");
    }

}

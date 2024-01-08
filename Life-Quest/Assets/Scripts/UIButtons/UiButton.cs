using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public void ReturnGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

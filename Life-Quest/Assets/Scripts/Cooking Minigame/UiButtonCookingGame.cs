using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonCookingGame : MonoBehaviour
{
    public void ReturnGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

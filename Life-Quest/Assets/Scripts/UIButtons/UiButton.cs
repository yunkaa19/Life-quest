using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    private Button button;
    private Color originalColor;
    public Color pressedColor = Color.red;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ReturnGame);
    }
    public void ReturnGame()
    {
        button.image.color = pressedColor;
        SceneManager.LoadSceneAsync(1);
    }
}

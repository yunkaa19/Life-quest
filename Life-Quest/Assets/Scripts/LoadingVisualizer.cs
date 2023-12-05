using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingVisualizer : MonoBehaviour
{
    public Image symbolImage;
    public Text progressText;

    public float loadingSpeed = 0.5f;
    private float loadingProgress = 0f;

    public Color startColor = new(205f, 205f, 205f);
    public Color endColor = new(85f, 195f, 233f);

    private string targetScene;

    void Start()
    {
        targetScene = PlayerPrefs.GetString("TargetScene", "Main Menu"); // Default to MainMenu if not set
        StartCoroutine(LoadTargetScene());
    }

    IEnumerator LoadTargetScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            symbolImage.color = Color.Lerp(startColor, endColor, progress);
            int percentage = Mathf.RoundToInt(progress * 100f);
            progressText.text = percentage + "%";

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

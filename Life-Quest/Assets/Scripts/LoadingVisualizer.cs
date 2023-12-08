using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingVisualizer : MonoBehaviour
{
    public Image symbolImage;
    public Text progressText;

    public float loadingSpeed = 0.1f;
    private float loadingProgress = 0f;

    public Color startColor = new(205f, 205f, 205f);
    public Color endColor = new(85f, 195f, 233f);

    private string targetScene;

    void Start()
    {
        targetScene = PlayerPrefs.GetString("TargetScene", "Main Menu");
        StartCoroutine(LoadTargetScene());
    }

    IEnumerator LoadTargetScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false;

        float targetProgress = 1.0f; 
        float delayAfterComplete = 1.0f;

        while (loadingProgress < targetProgress)
        {
            float progress = Mathf.Clamp01(loadingProgress / targetProgress);
            symbolImage.color = Color.Lerp(startColor, endColor, progress);

            int percentage = Mathf.RoundToInt(loadingProgress * 100f);
            progressText.text = percentage + "%";

            loadingProgress += Time.deltaTime * loadingSpeed;

            yield return null;
        }


        yield return new WaitForSeconds(delayAfterComplete);
        asyncLoad.allowSceneActivation = true;
    }
}

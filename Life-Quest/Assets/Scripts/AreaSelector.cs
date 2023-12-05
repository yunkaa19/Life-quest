using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaSelector : MonoBehaviour
{
    private Color defaultColor = new Color(1f, 1f, 1f, 0.7f);
    private Color selectedColor = new Color(1f, 1f, 1f, 1.0f);
    private Dictionary<string, string> sceneMapping = new Dictionary<string, string>();
    private float sceneLoadDelay = 3f;
    private string selectedSpriteName;
    public string loadingSceneName = "LoadingScene";

    private void Start()
    {
        SceneMapper();
        ResetAllSpriteColors();
        AddTouchListener();
    }

    private void AddTouchListener()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data, button.gameObject); });
            trigger.triggers.Add(entry);
        }
    }

    private void SceneMapper()
    {
        sceneMapping.Add("GREENPillar", "Main Menu");
        sceneMapping.Add("YELLOWPillar", "YellowFeeling");
        sceneMapping.Add("REDPillar", "Main Menu");
        sceneMapping.Add("BLUEPillar", "Main Menu");
        sceneMapping.Add("PINKPillar", "Main Menu");
    }

    private void OnPointerDown(PointerEventData data, GameObject clickedSprite)
    {

        selectedSpriteName = clickedSprite.name;
        Debug.Log("Selected Sprite: " + selectedSpriteName);


        Image spriteImage = clickedSprite.GetComponent<Image>();
        if (spriteImage != null)
        {
            ResetAllSpriteColors();
            spriteImage.color = selectedColor;
        }

        if (sceneMapping.ContainsKey(selectedSpriteName))
        {
            LoadLoadingScene(sceneMapping[selectedSpriteName]);
        }
        else
        {
            Debug.LogError("Invalid pillar name!");
        }
    }

    private void LoadLoadingScene(string targetScene)
    {
        PlayerPrefs.SetString("TargetScene", targetScene);
        SceneManager.LoadScene(loadingSceneName);
    }

    private void LoadSceneWithDelay()
    {
        SceneManager.LoadScene(sceneMapping[selectedSpriteName]);
    }

    private void ResetAllSpriteColors()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            Image spriteImage = button.GetComponent<Image>();
            if (spriteImage != null)
            {
                spriteImage.color = defaultColor;
            }
        }
    }
}

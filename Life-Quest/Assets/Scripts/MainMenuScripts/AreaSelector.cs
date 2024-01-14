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
        sceneMapping.Add("GREENPillar", "GreenSmelling");
        sceneMapping.Add("YELLOWPillar", "YellowFeeling");
        sceneMapping.Add("REDPillar", "RedHearing");
        sceneMapping.Add("BLUEPillar", "BlueSeeing");
        sceneMapping.Add("PINKPillar", "PinkTasting");
        sceneMapping.Add("GREYPillar", "NeutralMini");
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
        int minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);

        if(minigamesPlayed >= 4) { PlayerPrefs.SetInt("MinigamesPlayed", 0); }
        Debug.Log("minigames played" + minigamesPlayed.ToString());
        if (minigamesPlayed == 1)
        {
            PlayerPrefs.SetInt("PlayBellyRub", 1);
        }
        else
        {
            PlayerPrefs.SetInt("PlayBellyRub", 0); 
        }

        SceneManager.LoadScene(loadingSceneName);
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

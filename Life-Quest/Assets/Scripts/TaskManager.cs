using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public TaskData taskData;
    public GameObject taskContainerPrefab;
    public Transform contentTransform; 
    public Sprite[] senseIcons;
    public GameObject popUpCanvas;

    public GameObject popUpTextInput;
    public Text popUpTaskDescription;
    public TMP_InputField popUpInputField;

    private List<TaskData.Task> allTasks;
    private List<TaskData.Task> dailyTasks;

    public string youtubeLink = "https://www.youtube.com/watch?v=-pZy43c9Njg&t=142s";
    
    private DateTime lastTaskResetTime;
    private TimeSpan resetInterval = TimeSpan.FromDays(1);

    void Start()
    {
        allTasks = taskData.tasks.ToList();
        LoadLastTaskResetTime();

        Debug.Log(IsTaskResetRequired());
        // Check if the daily tasks need to be reset
        if (IsTaskResetRequired())
        {
            UpdateDailyTasks();
            SaveLastTaskResetTime();
            ClosePopUp();
            ClosePopUpTextField();
        }

        DisplayDailyTasks();
        ClosePopUp();
        ClosePopUpTextField();
    }

    void LoadLastTaskResetTime()
    {
        string lastResetTimeKey = "LastTaskResetTime";
        long ticks = PlayerPrefs.GetInt(lastResetTimeKey, 0);

        lastTaskResetTime = new DateTime(ticks);
    }

    void SaveLastTaskResetTime()
    {
        string lastResetTimeKey = "LastTaskResetTime";
        PlayerPrefs.SetInt(lastResetTimeKey, (int)lastTaskResetTime.Ticks);
        PlayerPrefs.Save();
    }

    bool IsTaskResetRequired()
    {
        DateTime now = DateTime.Now;
        return now - lastTaskResetTime >= resetInterval;
    }

    void UpdateDailyTasks()
    {
        ShuffleTasks();
    }

    void ShuffleTasks()
    {
        System.Random rng = new System.Random();
        allTasks = allTasks.OrderBy(x => rng.Next()).ToList();

        // Select 3 non-duplicate tasks for the current day
        dailyTasks = allTasks.Take(3).ToList();
    }

    void DisplayDailyTasks()
    {
        Debug.Log($"Number of daily tasks: {dailyTasks.Count}");
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < dailyTasks.Count; i++)
        {
            // Instantiate Card
            GameObject taskContainer = Instantiate(taskContainerPrefab, contentTransform);

            // Set task text
            Text taskText = taskContainer.GetComponentInChildren<Text>();
            taskText.text = $"{dailyTasks[i].sense}: {dailyTasks[i].description}";

            Image iconImage = taskContainer.transform.Find("SenseIcon").GetComponent<Image>();
            iconImage.sprite = GetSenseIcon(dailyTasks[i].sense);

            TaskData.Task currentTask = dailyTasks[i];

            // Attach button click event
            Button submitButton = taskContainer.GetComponentInChildren<Button>();
            submitButton.onClick.AddListener(() => OnSubmitButtonClick(currentTask));
            Debug.Log($"Added task container {i + 1}/{dailyTasks.Count}");
        }
    }

    Sprite GetSenseIcon(string sense)
    {
        // Find the sense icon based on the sense name
        Sprite icon = senseIcons.FirstOrDefault(icon => icon.name.ToLower() == sense.ToLower());
        if (icon == null)
        {
            Debug.LogError($"Icon not found for sense: {sense}");
        }
        return icon;
    }

    void OnSubmitButtonClick(TaskData.Task task)
    {
        OpenPopUpTextField();
        string taskDescription = task.description;
        switch (task.sense.ToLower())
        {
            case "sight":
                popUpTaskDescription.text = $"Describe what you saw when you '{taskDescription}'";
                break;
            case "hearing":
                popUpTaskDescription.text = $"Describe what you heard when you '{taskDescription}'";
                break;
            case "smell":
                popUpTaskDescription.text = $"Describe what you smelled when you '{taskDescription}'";
                break;
            case "taste":
                popUpTaskDescription.text = $"Describe what you tasted when you '{taskDescription}'";
                break;
            case "touch":
                popUpTaskDescription.text = $"Describe what you felt (touched) when you '{taskDescription}'";
                break;
            default:
                popUpTaskDescription.text = "Describe the sensation you're feeling:";
                break;
        }

        // Attach an event listener to the pop-up submit button
        Button submitButton = popUpTextInput.transform.Find("SubmitButton").GetComponent<Button>();
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(() => OnTextInputSubmitted(task));
        Debug.Log($"Button clicked for task: {task.sense}");
    }

    void OnTextInputSubmitted(TaskData.Task task)
    {
        TMP_InputField inputField = popUpTextInput.GetComponentInChildren<TMP_InputField>();
        string submittedText = inputField.text;
        string sense = task.sense;

        SaveSubmittedText(sense, submittedText);
        dailyTasks.Remove(task);
        DisplayDailyTasks();
        ClosePopUpTextField();
    }

    void SaveSubmittedText(string sense, string text)
    {
        string key = $"SubmittedText_{sense}";
        Debug.Log("Submitted:" + key);
        PlayerPrefs.SetString(key, text);
        PlayerPrefs.Save();
    }


    public void OpenPopUp()
    {
        popUpCanvas.SetActive(true);
    }

    public void ClosePopUp()
    {
        popUpCanvas.SetActive(false);
    }

    public void OpenPopUpTextField()
    {
        popUpTextInput.SetActive(true);
    }

    public void ClosePopUpTextField()
    {
        TMP_InputField inputField = popUpTextInput.GetComponentInChildren<TMP_InputField>();
        if (inputField != null)
        {
            inputField.text = "";
        }
        popUpTextInput.SetActive(false);
    }

    public void OpenLink()
    {
        Application.OpenURL(youtubeLink);
    }
}

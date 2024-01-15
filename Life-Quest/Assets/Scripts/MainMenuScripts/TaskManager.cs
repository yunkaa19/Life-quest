using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        LoadData();
        DisplayDailyTasks();
        ClosePopUps();
    }

    void LoadData()
    {
        LoadLastTaskResetTime();
        LoadDailyTasks();
        if (IsTaskResetRequired())
        {
            SaveLastTaskResetTime();
            UpdateDailyTasks();
            ClosePopUps();
        }

    }

    void LoadLastTaskResetTime()
    {
        lastTaskResetTime = DataPersistenceTasks.LoadDateTime("LastTaskResetTime");
        Debug.Log($"Loaded LastTaskResetTime: {lastTaskResetTime}");
    }

    void SaveLastTaskResetTime()
    {
        lastTaskResetTime = DateTime.Now; // Update reset time to the current time
        DataPersistenceTasks.SaveDateTime("LastTaskResetTime", lastTaskResetTime);
        Debug.Log($"Saved LastTaskResetTime: {lastTaskResetTime}");
    }

    bool IsTaskResetRequired()
    {
        DateTime now = DateTime.Now;
        return now - lastTaskResetTime >= resetInterval;
    }

    void UpdateDailyTasks()
    {
        ShuffleTasks();
        SaveDailyTasks();
    }

    void SaveDailyTasks()
    {
        DataPersistenceTasks.SaveList("DailyTasks", dailyTasks);
    }

    void LoadDailyTasks()
    {
        dailyTasks = DataPersistenceTasks.LoadList<TaskData.Task>("DailyTasks") ?? new List<TaskData.Task>();
    }

    void ShuffleTasks()
    {
        DateTime currentDate = DateTime.Now;

        List<string> submittedSenses = allTasks
            .Where(task => !string.IsNullOrEmpty(DataPersistenceTasks.LoadString($"SubmittedText_{task.sense}_{currentDate.Date}")))
            .Where(task => DataPersistenceTasks.LoadDateTime($"SubmissionDate_{task.sense}_{currentDate.Date}").Date == currentDate.Date)
            .Select(task => task.sense)
            .ToList();

        int tasksToLoad = Mathf.Max(3 - submittedSenses.Count, 0);

        List<TaskData.Task> remainingTasks = allTasks.Where(task => !submittedSenses.Contains(task.sense)).ToList();
        System.Random rng = new System.Random();
        remainingTasks = remainingTasks.OrderBy(x => rng.Next()).ToList();

        dailyTasks = remainingTasks.Take(tasksToLoad).ToList();
    }

    void DisplayDailyTasks()
    {
        if (dailyTasks == null)
        {
            UpdateDailyTasks();
        }

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
        SaveDailyTasks();
        DisplayDailyTasks();
        ClosePopUpTextField();
    }

    void SaveSubmittedText(string sense, string text)
    {
        string key = $"SubmittedText_{sense}";
        Debug.Log("Submitted:" + key);
        DataPersistenceTasks.SaveString(key, text);

        string dateKey = $"SubmissionDate_{sense}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
        DataPersistenceTasks.SaveDateTime(dateKey, DateTime.Now);
    }

    void ClosePopUps()
    {
        ClosePopUp();
        ClosePopUpTextField();
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

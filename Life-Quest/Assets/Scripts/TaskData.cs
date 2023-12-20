using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskData", menuName = "Task System/Task Data")]
public class TaskData : ScriptableObject
{
    [System.Serializable]
    public class Task
    {
        public string description;
        public string sense;
    }

    public Task[] tasks = new Task[10]
    {
        new Task { description = "How many birds do you see if you look outside", sense = "Sight" },
        new Task { description = "Walk to your fridge and choose an item to look at", sense = "Sight" },

        new Task { description = "Task 1 for Hearing", sense = "Hearing" },
        new Task { description = "Task 2 for Hearing", sense = "Hearing" },

        new Task { description = "Task 1 for Touch", sense = "Touch" },
        new Task { description = "Task 2 for Touch", sense = "Touch" },

        new Task { description = "Task 1 for Taste", sense = "Taste" },
        new Task { description = "Task 2 for Taste", sense = "Taste" },

        new Task { description = "Task 1 for Smell", sense = "Smell" },
        new Task { description = "Task 2 for Smell", sense = "Smell" },
    };
}

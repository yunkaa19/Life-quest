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

        new Task { description = "listen to your surroundings: what is the most interesting sound you hear?", sense = "Hearing" },
        new Task { description = "Pick a song that makes you feel good, listen to it", sense = "Hearing" },

        new Task { description = "Touch an object you regularly hold, what makes it feel familiar?", sense = "Touch" },
        new Task { description = "Task 2 for Touch", sense = "Touch" },

        new Task { description = "Task 1 for Taste", sense = "Taste" },
        new Task { description = "Task 2 for Taste", sense = "Taste" },

        new Task { description = "Go outside and smell a flower", sense = "Smell" },
        new Task { description = "Find a scent that reminds you of something and describe it", sense = "Smell" },
    };
}

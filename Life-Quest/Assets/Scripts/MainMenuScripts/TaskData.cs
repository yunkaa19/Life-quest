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
        new Task { description = "Touch your face with your eyes closed, what makes it feel special?", sense = "Touch" },

        new Task { description = "Take a bite of your favorite fruit", sense = "Taste" },
        new Task { description = "Take a bite of something you haven't tasted before", sense = "Taste" },

        new Task { description = "Go outside and smell a flower", sense = "Smell" },
        new Task { description = "Find a scent that reminds you of someone you love and describe it", sense = "Smell" },
    };
}

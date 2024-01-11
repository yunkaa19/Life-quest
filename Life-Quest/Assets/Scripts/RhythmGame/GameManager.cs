using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool musicStarted = false;
    public BeatScroller theBS;
    public static GameManager instance;

    public float delayBeforeStart = 10f;
    private float musicStartTime;
    private bool beatScrollerStarted = false;

    public GameObject touchToStartImage;
    public GameObject infoImage;
    public int activeNotes;
    public int deactivatedNotes;

    private bool minigameCompleted = false;

    void Start()
    {
        instance = this;
        theMusic.Stop();
        touchToStartImage.SetActive(true);
        infoImage.SetActive(false);
        activeNotes = GameObject.FindGameObjectsWithTag("Note").Length;
        LogActiveNotesCount();
        deactivatedNotes = 0;
    }

    void Update()
    {
        if (!musicStarted)
        {
            CheckForMusicStart();
        }
        else if (musicStarted && !beatScrollerStarted)
        {
            StartBeatScrollerAfterDelay();
        }

        CheckForCompletion();
    }

    void CheckForMusicStart()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            StartMusic();
        }
    }

    void StartMusic()
    {
        musicStarted = true;
        PlayDelayedMusic();
        musicStartTime = Time.time;
        touchToStartImage.SetActive(false);
        infoImage.SetActive(true);
    }

    void PlayDelayedMusic()
    {
        theMusic.PlayDelayed(0);
    }

    void StartBeatScrollerAfterDelay()
    {
        if (Time.time - musicStartTime > delayBeforeStart)
        {
            StartBeatScroller();
            infoImage.SetActive(false);
        }
    }

    void StartBeatScroller()
    {
        theBS.hasStarted = true;
        beatScrollerStarted = true;
    }

    void CheckForCompletion()
    {
        if (activeNotes == deactivatedNotes)
        {
            StartCoroutine(DelayBeforeSceneLoad());
        }
    }

    private IEnumerator DelayBeforeSceneLoad()
    {
        yield return new WaitForSeconds(3f);
        LoadCompletionScene();
    }

    void LoadCompletionScene()
    {
        if (!minigameCompleted)
        {
            SceneManager.LoadScene("Scenes/CompletionScene");
            minigameCompleted = true;
        }
    }

    public void NoteDeactivated()
    {
        deactivatedNotes++;
    }

    void LogActiveNotesCount()
    {
        Debug.Log("Active notes: " + activeNotes);
    }

    void LogDeactivatedNotesCount()
    {
        Debug.Log("Deactivated notes: " + deactivatedNotes);
    }

    void LogMinigameCompletion()
    {
        Debug.Log("Minigame completed!");
        theMusic.Stop();
    }
}

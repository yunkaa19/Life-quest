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
    public GameObject tapInfo;
    public GameObject effectParticle;
    public int activeNotes;
    public int deactivatedNotes;

    private bool minigameCompleted = false;

    void Start()
    {
        instance = this;
        theMusic.Stop();
        touchToStartImage.SetActive(true);
        infoImage.SetActive(false);
        tapInfo.SetActive(false);
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
        tapInfo.SetActive(true);

        StartCoroutine(HideTapInfoAfterDelay());
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

    IEnumerator HideTapInfoAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        tapInfo.SetActive(false);
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
            int minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
            minigamesPlayed++; 
            PlayerPrefs.SetInt("MinigamesPlayed", minigamesPlayed);
            SceneManager.LoadScene("CompletionScreen");
            minigameCompleted = true;
        }
    }

    public void NoteDeactivated(Transform noteTransform, bool playParticleSystem)
    {
        deactivatedNotes++;

        if (playParticleSystem)
        {
            Instantiate(effectParticle, noteTransform.position, Quaternion.identity);
        }
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

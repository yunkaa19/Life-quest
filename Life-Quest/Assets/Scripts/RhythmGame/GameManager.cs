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
    public GameObject popUpCanvas;
    public int activeNotes;
    public int deactivatedNotes;

    private bool minigameCompleted = false;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;
        instance = this;
        //theMusic.Stop();
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
        if (popUpCanvas.activeSelf)
        {
            StartCoroutine(WaitForPopUpCloseAndStartMusic());
        }
        else
        {
            StartMusicInternal();
        }
    }

    IEnumerator WaitForPopUpCloseAndStartMusic()
    {
        if (!popUpCanvas.activeSelf)
        {
            OpenPopUp();
        }

        while (popUpCanvas.activeSelf)
        {
            yield return null;
        }

        StartMusicInternal();
    }

    void StartMusicInternal()
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
        //theMusic.PlayDelayed(0);
        audioManager.RhythmMinigame.start();
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
            audioManager.RhythmMinigame.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
        audioManager.RhythmMinigame.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void OpenPopUp()
    {
        popUpCanvas.SetActive(true);
    }

    public void ClosePopUp()
    {
        popUpCanvas.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int amountOfNotes; 

    void Start()
    {
        instance = this;
        theMusic.Stop(); 
        touchToStartImage.SetActive(true);
        infoImage.SetActive(false);
        amountOfNotes = GameObject.FindGameObjectsWithTag("Note").Length; 
        Debug.Log(amountOfNotes);
    }

    void Update()
    {
        if (!musicStarted)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                musicStarted = true;
                PlayDelayedMusic();
                musicStartTime = Time.time; 
                touchToStartImage.SetActive(false);
                infoImage.SetActive(true);
            }
        }
        else if (musicStarted && !beatScrollerStarted && Time.time - musicStartTime > delayBeforeStart)
        {
            StartBeatScroller();
            infoImage.SetActive(false);
        }
    }

    void PlayDelayedMusic()
    {
        theMusic.PlayDelayed(0);
    }

    void StartBeatScroller()
    {
        theBS.hasStarted = true;
        beatScrollerStarted = true;
    }
}

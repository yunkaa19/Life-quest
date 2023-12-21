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

    void Start()
    {
        instance = this;
        theMusic.Stop(); // Ensure the music is initially stopped
    }

    void Update()
    {
        if (!musicStarted)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                musicStarted = true;
                PlayDelayedMusic();
                musicStartTime = Time.time; // Record music start time
            }
        }
        else if (musicStarted && !beatScrollerStarted && Time.time - musicStartTime > delayBeforeStart)
        {
            StartBeatScroller();
        }
    }

    void PlayDelayedMusic()
    {
        theMusic.PlayDelayed(0); // Start playing the music immediately
    }

    void StartBeatScroller()
    {
        theBS.hasStarted = true;
        beatScrollerStarted = true;
    }
}

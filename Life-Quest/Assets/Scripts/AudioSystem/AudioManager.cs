using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public EventInstance MainMenuMusic;
    public EventInstance FeelingMiniMusic;
    public EventInstance SeeingMiniMusic;
    public EventInstance SmellingMiniMusic;
    public EventInstance TastingMiniMusic;
    public EventInstance NeutralMiniMusic;
    public EventInstance PositiveFeedbackMusic;
    public EventInstance RubYourBelly;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            MainMenuMusic = RuntimeManager.CreateInstance("event:/MainMenu");
            SmellingMiniMusic = RuntimeManager.CreateInstance("event:/SmellingMini");
            TastingMiniMusic = RuntimeManager.CreateInstance("event:/TastingMini");
            SeeingMiniMusic = RuntimeManager.CreateInstance("event:/SeeingMini");
            FeelingMiniMusic = RuntimeManager.CreateInstance("event:/FeelingMini");
            NeutralMiniMusic = RuntimeManager.CreateInstance("event:/Neutral");
            PositiveFeedbackMusic = RuntimeManager.CreateInstance("event:/PostiveNote");
            PositiveFeedbackMusic = RuntimeManager.CreateInstance("event:/RubYourBelly");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

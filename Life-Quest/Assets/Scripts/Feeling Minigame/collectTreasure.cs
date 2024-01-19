using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RDG;

public class collectTreasure : MonoBehaviour
{
    ParticleSystem lightParticle;
    AudioSource pickupSFX;
    public spawnTreasure spawnTreasure;
    public int treasuresCollected = 0;
    int minigamesPlayed;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.FeelingMiniMusic.start();
        minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
        lightParticle = GetComponent<ParticleSystem>();
        pickupSFX = GetComponent<AudioSource>();
    }

    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Treasure")
        {
            Handheld.Vibrate();
            GameObject[] ripples = GameObject.FindGameObjectsWithTag("Ripple");
            foreach (GameObject rippleObject in ripples)
            {
                Destroy(rippleObject);
            }

            lightParticle.Play();
            audioManager.FeelingPickupSound.start();

            treasuresCollected++;
            spawnTreasure.alreadySpawned = 0;
            if (treasuresCollected == 3)
            {
                audioManager.FeelingMiniMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                minigamesPlayed++;
                PlayerPrefs.SetInt("MinigamesPlayed", minigamesPlayed);
                SceneManager.LoadScene("CompletionScreen");
            }
        }
    }

}

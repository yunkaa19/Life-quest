using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collectTreasure : MonoBehaviour
{
    ParticleSystem lightParticle;
    AudioSource pickupSFX;
    public spawnTreasure spawnTreasure;
    public int treasuresCollected = 0;
    int minigamesPlayed;
    

    void Start()
    {
        minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
        lightParticle = GetComponent<ParticleSystem>();
        pickupSFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(treasuresCollected == 3)
        {
            minigamesPlayed++;
            PlayerPrefs.SetInt("MinigamesPlayed", minigamesPlayed);
            SceneManager.LoadScene("CompletionScreen");
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Treasure")
        {
            Handheld.Vibrate();
            GameObject[] ripples = GameObject.FindGameObjectsWithTag("Ripple");        
            foreach(GameObject rippleObject in ripples)
            {
                Destroy(rippleObject);
            }

            lightParticle.Play();
            pickupSFX.Play();

            treasuresCollected ++;
            spawnTreasure.alreadySpawned = 0;
        }
    }

}

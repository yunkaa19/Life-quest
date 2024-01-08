using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collectTreasure : MonoBehaviour
{
    public spawnTreasure spawnTreasure;
    public int treasuresCollected = 0;
    int minigamesPlayed;
    

    void Start()
    {
        minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
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
        Debug.Log("hit a " + collision.gameObject.tag);
        Handheld.Vibrate();
        if(collision.gameObject.tag == "Treasure")
        {
            Debug.Log("You found it");
            GameObject[] ripples = GameObject.FindGameObjectsWithTag("Ripple");
            foreach(GameObject rippleObject in ripples)
            {
                Destroy(rippleObject);
            }



            treasuresCollected ++;
            spawnTreasure.alreadySpawned = 0;
        }
    }

}

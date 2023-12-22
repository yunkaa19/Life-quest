using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectTreasure : MonoBehaviour
{
    public spawnTreasure spawnTreasure;
    public int treasuresCollected = 0;
    


    void Update()
    {
        if(treasuresCollected == 3)
        {
            //do a win
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit a " + collision.gameObject.tag);
        Handheld.Vibrate();
        if(collision.gameObject.tag == "Treasure")
        {
            Debug.Log("You found it");
            treasuresCollected ++;
            spawnTreasure.alreadySpawned = 0;
        }
    }

}

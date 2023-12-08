using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectTreasure : MonoBehaviour
{

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit a " + collision.gameObject.tag);
        if(collision.gameObject.tag == "Treasure")
        {
            Destroy(collision.gameObject);
            Handheld.Vibrate();
            Debug.Log("You found it");
        }
    }
}

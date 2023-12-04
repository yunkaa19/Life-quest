using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTreasure : MonoBehaviour
{
    public float horizontalPosition;
    public float verticalPosition;
    
    void Start()
    {

        DeterminePosition();
        
    }

    void DeterminePosition()
    {
        horizontalPosition = Random.Range(-6.5f, 7.3f);
        verticalPosition = Random.Range(-19.2f, 16.4f);
        if(horizontalPosition > -3f && horizontalPosition < 3.5f && verticalPosition > -6f && verticalPosition < 5.5f)
        {
            DeterminePosition();
            return;
        }

        else
        {
            transform.position = new Vector3(horizontalPosition,verticalPosition,transform.position.z);
        }
    }

}

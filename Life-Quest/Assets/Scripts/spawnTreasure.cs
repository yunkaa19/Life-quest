using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTreasure : MonoBehaviour
{
    public float treasureDistance;
    public float horizontalPosition;
    public float verticalPosition;
    public float verticalPositionStoring;
    public float verticalAbsolute;
    
    void Start()
    {

        DeterminePosition();
        
    }

    void DeterminePosition()
    {
        treasureDistance = Random.Range(20f,30f);
        horizontalPosition = Random.Range(-treasureDistance, treasureDistance);
        verticalPositionStoring = treasureDistance*treasureDistance - horizontalPosition*horizontalPosition;
        verticalPosition = Mathf.Sqrt(verticalPositionStoring);
        verticalAbsolute = Random.Range(-1f,1f);

        if(verticalAbsolute <= 0)
        {
            transform.position = new Vector3(horizontalPosition,-verticalPosition,transform.position.z);
        }
        if(verticalAbsolute > 0)
        {
            transform.position = new Vector3(horizontalPosition,verticalPosition,transform.position.z);
        }

        
        

        
    }

}

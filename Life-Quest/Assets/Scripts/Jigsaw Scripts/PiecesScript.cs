using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PiecesScript : MonoBehaviour
{
    private Vector2 RightPosition;
    public bool InRightPosition;
    public bool Selected;
    
    void Start()
    {
        RightPosition = transform.position;
        transform.position = new Vector2(Random.Range(-1.8f,1.55f), Random.Range(-0.55f,-3.9f));
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, RightPosition) <0.5f)
        {
            if(!Selected)
            {  
                if(InRightPosition == false)
                {
                    transform.position = RightPosition;
                    InRightPosition = true;
                    GetComponent<SortingGroup>().sortingOrder = 0;
                    DragAndDrop.amountOfRightPieces ++;
                }
            }
        }
    }
}

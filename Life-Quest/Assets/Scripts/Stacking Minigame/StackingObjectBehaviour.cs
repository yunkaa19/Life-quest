using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingObjectBehaviour : MonoBehaviour
{
    private Vector3 offset;
    private float zCoordinate;
    public Camera mainCamera;
    public StackingManager stackingManager;

    void Start()
    {
        
    }

    void OnMouseDown()
    {
        zCoordinate = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }
    
    void OnMouseUp()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.75f;
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(GetMouseWorldPos().x + offset.x, GetMouseWorldPos().y + offset.y, transform.position.z);
        
        GetComponent<Rigidbody2D>().MovePosition(newPosition);
        
        GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BalancingObject"))
        {
            stackingManager.AddCollisionPair(gameObject, collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BalancingObject"))
        {
            stackingManager.RemoveCollisionPair(gameObject, collision.gameObject);
        }
    }
}

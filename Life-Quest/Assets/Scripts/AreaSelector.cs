using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                //Debug.Log("Touched UI element");
                return;
            }

            // Convert touch position to world space
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // Cast a ray from the touch position
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            // Check if the ray hits a collider
            if (hit.collider != null)
            {
                Debug.Log("Selected: " + hit.collider.gameObject.name);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHitbox : MonoBehaviour
{
    [SerializeField] private SlicableFruit slicableFruit;
    [SerializeField] private GameObject sliced;
    private bool isTouching = false;
    private bool isSliced = false; // To check if the fruit is already sliced

    void Update()
    {
        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Consider the first touch for simplicity

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Check if the touch began over the GameObject
                    isTouching = IsTouchInsideTarget(touch.position);
                    break;

                case TouchPhase.Moved:
                    // Check if the touch moved into the GameObject
                    if (!isTouching && IsTouchInsideTarget(touch.position))
                    {
                        isTouching = true; // Set the flag to indicate the touch is inside
                    }
                    // Check if the touch moved out of the GameObject
                    else if (isTouching && !IsTouchInsideTarget(touch.position))
                    {
                        HandleTouchExit();
                        isTouching = false; // Reset the flag
                    }
                    break;

                case TouchPhase.Ended:
                    // Check if the touch ended and it was over the GameObject
                    if (isTouching)
                    {
                        HandleTouchExit();
                        isTouching = false; // Reset the flag
                    }
                    break;
            }


        // if (isSliced)
        // {
        //     // Drop the fruit downwards
        //     transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);
        // }

        }
    }

    bool IsTouchInsideTarget(Vector2 touchPosition)
    {
        // Convert the touch position to a ray in the game world
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        // Perform a 2D raycast to check if it hits the collider of the GameObject
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the hit object is the same as the GameObject this script is attached to
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }



    void HandleTouchExit()
    {
        // This method is called when the touch exits the target area


        if(slicableFruit.GetSlicedObject() == null){
            slicableFruit.SetSlicedObject(sliced);
            slicableFruit.Slice();
            
            isSliced = true; // Mark the fruit as sliced


            // Disable or destroy the Box Collider 2D components
            var boxColliders = GetComponents<BoxCollider2D>();
            foreach (var collider in boxColliders)
            {
                collider.enabled = false; // To disable
                // Destroy(collider); // Uncomment to destroy
            }
        }

    }
}

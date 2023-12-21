using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedHitBox : MonoBehaviour
{

    public SeedSelector seedSelector; // Reference to the SeedSelector
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Consider the first touch

            if (touch.phase == TouchPhase.Began)
            {
                CheckTouch(touch.position);
            }
        }
    }

    void CheckTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            Seed seed = hit.collider.gameObject.GetComponent<Seed>();
            if (seed != null)
            {
                seed.CheckAndDropIfBitter();
            }
        }
    }
}

using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    public bool hasStarted;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    void Update()
    {
        if (!hasStarted)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                hasStarted = true;
            }
        }
        else
        {
            // Assuming you want the scroll based on time
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);

            // Alternatively, for touch-based scroll:
            // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            // {
            //     Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //     transform.position -= new Vector3(0f, touchDeltaPosition.y * Time.deltaTime, 0f);
            // }
        }
    }
}

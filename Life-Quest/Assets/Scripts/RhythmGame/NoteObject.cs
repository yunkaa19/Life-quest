using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;

    void Start()
    {

    }

    void Update()
    {
        // Check for touch input instead of keyboard input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if the touch occurs on an object that can be pressed
            if (canBePressed)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.tag == "Activator")
    {
        canBePressed = true;
        Debug.Log("Can be pressed: " + canBePressed);
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.tag == "Activator")
    {
        canBePressed = false;
        Debug.Log("Can be pressed: " + canBePressed);
    }
}

}

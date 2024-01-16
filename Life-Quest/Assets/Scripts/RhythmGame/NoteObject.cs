using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        canBePressed = false;
    }

    void Update()
    {
        if (transform.position.y <= -3.5f)
        {
            gameObject.SetActive(false);
            gameManager.NoteDeactivated(transform, true);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && canBePressed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    gameObject.SetActive(false);
                    gameManager.NoteDeactivated(transform, false);
                }
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
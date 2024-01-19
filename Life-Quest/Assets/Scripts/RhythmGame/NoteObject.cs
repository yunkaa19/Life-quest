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
        if (transform.position.y <= -3.46f)
        {
            DestroyNoteObject();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            if (canBePressed)
            {
                DestroyNoteObject();
                gameManager.NoteDestroyed(transform, true);
            }
        }
    }

    private void DestroyNoteObject()
    {
        Destroy(gameObject);
    }

}
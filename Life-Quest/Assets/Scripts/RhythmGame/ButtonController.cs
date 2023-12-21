using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;

    private bool isPressed = false;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D buttonCollider = GetComponent<Collider2D>();

                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Note"))
                {
                    hit.collider.gameObject.SetActive(false);
                }

                if (buttonCollider == Physics2D.OverlapPoint(touchPosition))
                {
                    theSR.sprite = pressedImage;
                    isPressed = true;
                }
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isPressed)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D buttonCollider = GetComponent<Collider2D>();

                if (buttonCollider == Physics2D.OverlapPoint(touchPosition))
                {
                    theSR.sprite = defaultImage;
                    isPressed = false;
                }
            }
        }
    }
}
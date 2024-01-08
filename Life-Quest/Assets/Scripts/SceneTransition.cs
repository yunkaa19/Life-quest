using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    SceneManager.LoadScene("Main Menu");
    //    Debug.Log("Clicked!");
    //}

    public Button yourButton;

    void Start()
    {
        yourButton.onClick.AddListener(TaskOnClick);
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == yourButton.gameObject)
                    {
                        TaskOnClick();
                    }
                }
            }
        }
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("Main Menu");
        Debug.Log("Clicked!");
    }
}

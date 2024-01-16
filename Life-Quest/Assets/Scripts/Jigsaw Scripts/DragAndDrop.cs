using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DragAndDrop : MonoBehaviour
{
    public GameObject SelectedPiece;
    [SerializeField] public static int amountOfRightPieces = 0;
    int OIL = 1;
    private AudioManager audioManager;
    private bool IsPopUpActive = true;

    public GameObject popUpCanvas;

    void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.SeeingMiniMusic.start();
    }

    void Update()
    {
        if(amountOfRightPieces == 16)
        {
            CompletionCriteria();
        }
        if(Input.GetMouseButtonDown(0) && IsPopUpActive == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.transform.CompareTag("Puzzle"))
            {
                if(!hit.transform.GetComponent<PiecesScript>().InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    SelectedPiece.GetComponent<PiecesScript>().Selected = true;
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                    OIL++;
                }

            }    
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(SelectedPiece != null)
            {
                SelectedPiece.GetComponent<PiecesScript>().Selected = false;
                SelectedPiece = null;
            }
        }

        if(SelectedPiece != null)
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SelectedPiece.transform.position = new Vector3(MousePoint.x,MousePoint.y,0);
        }

        void CompletionCriteria()
        {
            audioManager.SeeingMiniMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            int minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
            minigamesPlayed ++;
            PlayerPrefs.SetInt("MinigamesPlayed", minigamesPlayed);
            SceneManager.LoadScene("CompletionScreen");
        }


    }        
    //Pop up management
    public void OpenPopUp()
    {
        popUpCanvas.SetActive(true);
        IsPopUpActive = true;
    }

    public void ClosePopUp()
    {
        popUpCanvas.SetActive(false);
        IsPopUpActive = false;
    }
}

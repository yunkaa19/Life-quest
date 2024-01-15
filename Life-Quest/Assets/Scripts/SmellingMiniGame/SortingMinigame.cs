using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SortingMinigame : MonoBehaviour
{
    public Transform startContainer;
    public GameObject boxPrefab;
    public int numberOfBoxes = 10;
    public Sprite[] itemSprites;
    public Transform[] sortedContainers;

    public List<Transform> snapAreas;
    public List<DraggableItem> draggableObjects;
    public float snapDistance = 100f;

    private List<Transform> filledSnapPoints = new List<Transform>();
    private int targetFilledSnapPointsCount = 10;
    private bool isDragging = false;

    public GameObject popUpCanvas;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = AudioManager.Instance;
        GenerateBoxes();
        foreach (DraggableItem item in draggableObjects)
        {
            item.onDragStart.AddListener(() => OnDragStart());
            item.onDragEnd.AddListener(() => OnDragEnded(item));
        }
    }

    void Update()
    {
        foreach (DraggableItem item in draggableObjects)
        {
            if (item.IsDragging())
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                item.UpdatePosition(touchPosition);
            }
        }
    }

    void OnDragStart()
    {
        isDragging = true;
        StopCoroutine(CheckWinConditionAfterDelay());
    }

    private void OnDragEnded(DraggableItem item)
    {
        float closestDistance = -1f;
        Transform closestSnapPoint = null;

        foreach (Transform snapPoint in snapAreas)
        {
            float currentDistance = Vector2.Distance(item.transform.position, snapPoint.position);

            // Check if the snap point is the closest one
            if (closestSnapPoint == null || currentDistance < closestDistance)
            {
                closestSnapPoint = snapPoint;
                closestDistance = currentDistance;
            }
        }

        if (closestSnapPoint != null)
        {
            if (closestDistance >= snapDistance && closestDistance < 200f)
            {
                // Snap to a random position within the snap area
                Vector3 randomPosition = GetRandomPositionInSnapArea(closestSnapPoint, item.GetComponent<RectTransform>().rect);

                // Snap to the center of the snap point
                item.SnapToPosition(randomPosition);

                // Optionally, you can adjust the item's position to make it align with the snap point boundary
                Vector3 offset = item.transform.position - closestSnapPoint.position;
                item.transform.position -= offset;

                if (!filledSnapPoints.Contains(closestSnapPoint))
                {
                    filledSnapPoints.Add(closestSnapPoint);
                }

            }
            else
            {
                Debug.Log("Closest distance greater than snapDistance");
                item.ResetToInitialPosition();
            }
        }
        else
        {
            Debug.Log("No closest snap point found");
            item.ResetToInitialPosition();
        }
        isDragging = false;
        StartCoroutine(CheckWinConditionAfterDelay());
    }

    Vector3 GetRandomPositionInSnapArea(Transform snapArea, Rect itemRect)
    {
        RectTransform snapAreaRect = snapArea.GetComponent<RectTransform>();

        float minX = snapAreaRect.anchoredPosition.x - snapAreaRect.rect.width / 2.0f + itemRect.width / 2.0f;
        float maxX = snapAreaRect.anchoredPosition.x + snapAreaRect.rect.width / 2.0f - itemRect.width / 2.0f;
        float minY = snapAreaRect.anchoredPosition.y - snapAreaRect.rect.height / 2.0f + itemRect.height / 2.0f;
        float maxY = snapAreaRect.anchoredPosition.y + snapAreaRect.rect.height / 2.0f - itemRect.height / 2.0f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector3(randomX, randomY, 0f);
    }

    void GenerateBoxes()
    {
        RectTransform containerRectTransform = startContainer.GetComponent<RectTransform>();
        float containerWidth = containerRectTransform.rect.width;
        float containerHeight = containerRectTransform.rect.height;

        List<Vector2> occupiedPositions = new List<Vector2>();
        List<Sprite> usedSprites = new List<Sprite>();

        for (int i = 0; i < numberOfBoxes; i++)
        {
            GameObject box = Instantiate(boxPrefab, startContainer);
            RectTransform boxRectTransform = box.GetComponent<RectTransform>();
            float minX = -containerWidth / 2.0f + boxRectTransform.rect.width / 2.0f;
            float maxX = containerWidth / 2.0f - boxRectTransform.rect.width / 2.0f;
            float minY = -containerHeight / 2.0f + boxRectTransform.rect.height / 2.0f;
            float maxY = containerHeight / 2.0f - boxRectTransform.rect.height / 2.0f;

            Vector2 randomPosition;
            Sprite randomSprite;

            do
            {
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);
                randomPosition = new Vector2(randomX, randomY);
                randomSprite = GetRandomFruitSprite();
            } while (IsOverlap(randomPosition, occupiedPositions, boxRectTransform.rect) || usedSprites.Contains(randomSprite));

            occupiedPositions.Add(randomPosition);
            usedSprites.Add(randomSprite);

            boxRectTransform.anchoredPosition = randomPosition;
            Image boxImage = box.GetComponent<Image>();
            boxImage.sprite = randomSprite;
            draggableObjects.Add(box.GetComponent<DraggableItem>());
        }
    }

    bool IsOverlap(Vector2 position, List<Vector2> occupiedPositions, Rect boxRect)
    {
        foreach (Vector2 occupiedPosition in occupiedPositions)
        {
            Rect occupiedRect = new Rect(occupiedPosition.x - boxRect.width / 2.0f,
                                         occupiedPosition.y - boxRect.height / 2.0f,
                                         boxRect.width,
                                         boxRect.height);

            if (occupiedRect.Overlaps(new Rect(position.x - boxRect.width / 2.0f,
                                               position.y - boxRect.height / 2.0f,
                                               boxRect.width,
                                               boxRect.height)))
            {
                return true; // Overlapping
            }
        }

        return false; // Not overlapping
    }

    Sprite GetRandomFruitSprite()
    {
        if (itemSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, itemSprites.Length);
            return itemSprites[randomIndex];
        }

        return null;
    }

    IEnumerator CheckWinConditionAfterDelay()
    {
        float elapsedTime = 0f;
        float delay = 3f; // Adjust the delay as needed

        while (elapsedTime < delay)
        {
            yield return null; // Wait for the next frame
            elapsedTime += Time.deltaTime;
        }

        // Check win condition after the delay
        if (!isDragging && filledSnapPoints.Count == targetFilledSnapPointsCount)
        {
            Debug.Log("You win!");

            int minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
            minigamesPlayed++;
            PlayerPrefs.SetInt("MinigamesPlayed", minigamesPlayed);
            audioManager.SmellingMiniMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            SceneManager.LoadScene("CompletionScreen");
        }
    }

    //POP UP
    public void OpenPopUp()
    {
        popUpCanvas.SetActive(true);
        audioManager.SmellingMiniMusic.setPaused(true);
    }

    public void ClosePopUp()
    {
        popUpCanvas.SetActive(false);

        if (audioManager.SmellingMiniMusic.getPaused(out bool isPaused) == FMOD.RESULT.OK)
        {
            if (isPaused)
            {
                audioManager.SmellingMiniMusic.setPaused(false);
            }
            else
            {
                audioManager.SmellingMiniMusic.start();
            }
        }
    }
}

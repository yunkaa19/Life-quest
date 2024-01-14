using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UnityEvent onDragStart;
    public UnityEvent onDragEnd;

    private bool isDragging = false;
    private Vector2 touchStartPos;
    private Vector2 spriteDragStartPosition;
    private Vector2 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (isDragging)
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                UpdatePosition(touchPosition);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDragging();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopDragging();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            UpdatePosition(eventData.position);
        }
    }


    void StartDragging()
    {
        if (!isDragging)
        {
            onDragStart.Invoke();
            isDragging = true;
            touchStartPos = Input.mousePosition;
            spriteDragStartPosition = transform.position;
        }
    }

    void StopDragging()
    {
        if (isDragging)
        {
            onDragEnd.Invoke();
            isDragging = false;
        }
    }

    public void ResetToInitialPosition()
    {
        transform.localPosition = initialPosition;
    }

    public bool IsDragging()
    {
        return isDragging;
    }

    public void UpdatePosition(Vector2 touchPosition)
    {
        transform.position = spriteDragStartPosition + (touchPosition - touchStartPos);
    }

    public void SnapToPosition(Vector2 position)
    {
        transform.localPosition = position;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneSetter : MonoBehaviour
{
   public RectTransform topLine;
    public RectTransform bottomLine;
    public RectTransform bellyImage;

    public float speed = 1.0f;
    public float topTargetPosY = 555f; // Target position Y for the top line
    public float bottomTargetPosY = -575f; // Target position Y for the bottom line
    public float moveDistance = 10f; // Distance to move per step for the lines

    private bool topLineReached = false;
    private bool bottomLineReached = false;
    private bool bellyImageReached = false;
    
    public bool isSceneSet = false; // used to check out if the scene is set up so that the belly rub can be started.

    void Update()
    {
        // Move the top line up to the target position if it hasn't reached yet
        if (!topLineReached)
        {
            float step = speed * Time.deltaTime;
            topLine.anchoredPosition = new Vector2(topLine.anchoredPosition.x, Mathf.MoveTowards(topLine.anchoredPosition.y, topTargetPosY, step));
            if (Mathf.Abs(topLine.anchoredPosition.y - topTargetPosY) < 0.001f)
            {
                topLineReached = true; // Stop moving the top line
            }
        }

        // Move the bottom line down to the target position if it hasn't reached yet
        if (!bottomLineReached)
        {
            float step = speed * Time.deltaTime;
            bottomLine.anchoredPosition = new Vector2(bottomLine.anchoredPosition.x, Mathf.MoveTowards(bottomLine.anchoredPosition.y, bottomTargetPosY, step));
            if (Mathf.Abs(bottomLine.anchoredPosition.y - bottomTargetPosY) < 0.001f)
            {
                bottomLineReached = true; // Stop moving the bottom line
            }
        }

        // Move the belly image right to the target position if it hasn't reached yet
        if (!bellyImageReached && topLineReached && bottomLineReached)
        {
            float step = speed * Time.deltaTime;
            bellyImage.anchoredPosition = new Vector2(Mathf.MoveTowards(bellyImage.anchoredPosition.x, 0, step), bellyImage.anchoredPosition.y);
            if (Mathf.Abs(bellyImage.anchoredPosition.x) < 0.001f)
            {
                bellyImageReached = true; // Stop moving the belly image
                isSceneSet = true;
            }
        }
    }
}


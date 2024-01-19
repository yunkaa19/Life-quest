using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StackingManager : MonoBehaviour
{


    #region Variables
    /// <summary>
    /// The hashset is used to keep track of which objects are touching.
    ///</summary>
    private HashSet<(GameObject, GameObject)> touchingPairs = new HashSet<(GameObject, GameObject)>();

    /// <summary>
    /// The set of these variables are used to keep track of the balancing state.
    /// </summary>
    public bool isBalancing = false;
    private float highestYAtStart;
    private const float YChangeThreshold = 2f;


    [Header("Balancing Countdown")]
    public TMP_Text balancingCountdownText;
    public float getReadyDuration = 2f;
    public float balancingCountdown = 5f;

    private AudioManager audioManager;

    #endregion

    #region Unity Methods
    private void Start()
    {
        balancingCountdownText.text = "";
        audioManager = AudioManager.Instance;
        audioManager.NeutralMiniMusic.start();
    }

    private void Update()
    {
        if (isBalancing)
        {
            ApplyForces();
        }
    }
    #endregion

    #region Custom Methods



    /// <summary>
    ///  Adds a pair of colliding GameObjects to the touchingPairs set and checks if the balancing phase should start.
    /// </summary>
    public void AddCollisionPair(GameObject obj1, GameObject obj2)
    {
        // Create a tuple of the two hexagons, sorted by their instance IDs to avoid duplicates
        var pair = (obj1.GetInstanceID() < obj2.GetInstanceID()) ? (hex1: obj1, hex2: obj2) : (hex2: obj2, hex1: obj1);
        // Add the pair to the set if it's not already there
        if (!touchingPairs.Contains(pair))
        {
            touchingPairs.Add(pair);
            CheckForBalancingStart();
        }
    }


    /// <summary>
    ///  Removes a pair of GameObjects from the touchingPairs set when they are no longer colliding.
    /// </summary>
    public void RemoveCollisionPair(GameObject obj1, GameObject obj2)
    {
        var pair = (obj1.GetInstanceID() < obj2.GetInstanceID()) ? (hex1: obj1, hex2: obj2) : (hex2: obj2, hex1: obj1);
        touchingPairs.Remove(pair);
    }


    /// <summary>
    /// Checks if conditions are met to start the balancing phase (e.g., when a certain number of GameObjects are touching).
    /// </summary>
    private void CheckForBalancingStart()
    {
        // Start balancing if 4 hexagons are touching and not already balancing
        if (touchingPairs.Count == 3 && !isBalancing)
        {
            if (IsProperlyStacked() && CheckRotation())
            {
                isBalancing = true;
                StartCoroutine(BalancingRoutine());
            }
        }
    }

    private bool CheckRotation()
    {
        const float maxRotation = 10f;
        foreach (var pair in touchingPairs)
        {
            float rotation1 = NormalizeAngle(pair.Item1.transform.rotation.eulerAngles.z);
            float rotation2 = NormalizeAngle(pair.Item2.transform.rotation.eulerAngles.z);
            float rotationDiff = Mathf.Abs(rotation1 - rotation2);

            // Check if the rotation is within maxRotation degrees of either 0 or 180
            if (!((rotationDiff <= maxRotation) || (Mathf.Abs(rotationDiff - 180) <= maxRotation)))
            {
                return false;
            }
        }
        return true;
    }

    private float NormalizeAngle(float angle)
    {
        // Normalize angle to be within -180 to 180
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }   

    private bool IsProperlyStacked()
    {
        const float minVerticalDistance = 0.18f;
        HashSet<GameObject> uniqueObjects = new HashSet<GameObject>();

        foreach (var pair in touchingPairs)
        {
            uniqueObjects.Add(pair.Item1);
            uniqueObjects.Add(pair.Item2);
        }

        List<GameObject> sortedObjects = new List<GameObject>(uniqueObjects);
        sortedObjects.Sort((obj1, obj2) => obj1.transform.position.y.CompareTo(obj2.transform.position.y));


        for (int i = 0; i < sortedObjects.Count - 1; i++)
        {
            float yDistance = Mathf.Abs(sortedObjects[i].transform.position.y - sortedObjects[i + 1].transform.position.y);

            if (yDistance < minVerticalDistance)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// Applies forces based on device's tilt (using acceleration) to all GameObjects in touchingPairs, influencing their movement.
    /// </summary>
    private void ApplyForces()
    {
        // Get the device's acceleration vector
        Vector3 acceleration = Input.acceleration;

        // Assuming that the device is held upright, x will represent the tilt from left to right
        float forceMultiplier = 100.0f; // Adjust this value as needed for testing
        Vector2 forceDirection = new Vector2(acceleration.x, 0); // We only care about the x-axis for left/right tilt

        // Scale up the force for testing purposes
        Vector2 scaledForceDirection = forceDirection * forceMultiplier;


        // Apply scaled forces to all hexagons
        foreach (var pair in touchingPairs)
        {
            Rigidbody2D rb1 = pair.Item1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = pair.Item2.GetComponent<Rigidbody2D>();

            rb1?.AddForce(scaledForceDirection * Time.deltaTime);
            rb2?.AddForce(scaledForceDirection * Time.deltaTime);
        }
    }


    /// <summary>
    /// Calculates and returns the highest Y position among all GameObjects in touchingPairs.
    /// </summary>
    private float GetHighestYPosition()
    {
        float highestY = float.MinValue;
        foreach (var pair in touchingPairs)
        {
            highestY = Mathf.Max(highestY, pair.Item1.transform.position.y);
            highestY = Mathf.Max(highestY, pair.Item2.transform.position.y);
        }
        return highestY;
    }

    /// <summary>
    /// Updates the UI text to display the remaining time for balancing.
    /// </summary>
    private void UpdateCountdownDisplay(float time)
    {
        balancingCountdownText.text = "Balance for: " + Mathf.CeilToInt(time).ToString() + "s";
    }

    /// <summary>
    ///  Checks if the balancing criteria are met (based on Y position changes and number of touching pairs) and determines success or failure.
    /// </summary>
    private void CheckBalance()
    {

        if (Mathf.Abs(GetHighestYPosition() - highestYAtStart) > YChangeThreshold)
        {
            // Failure logic here
        }
        else if (touchingPairs.Count >= 3)
        {
            audioManager.NeutralMiniMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            // Success logic here
            int minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed", 0);
            // Increment minigamesPlayed after the game is completed
            minigamesPlayed++;
            PlayerPrefs.SetInt("MinigamesPlayed", minigamesPlayed);
            SceneManager.LoadScene("CompletionScreen");
        }
        else
        {
            // Failure logic here
        }

        isBalancing = false;
    }


    /// <summary>
    /// A coroutine that manages the balancing countdown, updates the display, and performs a final balance check at the end.
    /// </summary>
    private IEnumerator BalancingRoutine()
    {
        highestYAtStart = GetHighestYPosition();

        // Give the player time to get ready
        yield return new WaitForSeconds(getReadyDuration);

        // Start the balancing countdown
        float countdown = balancingCountdown;
        while (countdown > 0)
        {
            if (touchingPairs.Count < 3)
            {
                // If the hexagons are no longer touching, stop the countdown
                UpdateCountdownDisplay(0);
                isBalancing = false;
                balancingCountdownText.text = "";
                break;
            }


            UpdateCountdownDisplay(countdown);
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        // Clear the countdown display
        UpdateCountdownDisplay(0);
        balancingCountdownText.text = "";

        // Final check if the hexagons are still balanced
        CheckBalance();
    }
    #endregion
}

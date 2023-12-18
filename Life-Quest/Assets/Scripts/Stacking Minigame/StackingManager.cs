using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StackingManager : MonoBehaviour
{
    private HashSet<(GameObject, GameObject)> touchingPairs = new HashSet<(GameObject, GameObject)>();
    public bool isBalancing = false;
    
    [Header("Balancing Countdown")]
    public TMP_Text balancingCountdownText;
    public float getReadyDuration = 2f;
    public float balancingCountdown = 5f;
    private void Start()
    {
        Input.gyro.enabled = false;
        balancingCountdownText.text = "";
    }

    private void Update()
    {
        if (isBalancing)
        {
            ApplyGyroForces();
        }
    }


    public void AddCollisionPair(GameObject hex1, GameObject hex2)
    {
        // Create a tuple of the two hexagons, sorted by their instance IDs to avoid duplicates
        var pair = (hex1.GetInstanceID() < hex2.GetInstanceID()) ? (hex1, hex2) : (hex2, hex1);
        // Add the pair to the set if it's not already there
        if (!touchingPairs.Contains(pair))
        {
            touchingPairs.Add(pair);
            CheckForBalancingStart();
        }
    }

    public void RemoveCollisionPair(GameObject hex1, GameObject hex2)
    {
        var pair = (hex1.GetInstanceID() < hex2.GetInstanceID()) ? (hex1, hex2) : (hex2, hex1);
        touchingPairs.Remove(pair);
    }

    private void CheckForBalancingStart()
    {
        // Start balancing if 4 hexagons are touching and not already balancing
        if (touchingPairs.Count == 3 && !isBalancing)
        {
            isBalancing = true;
            StartCoroutine(BalancingRoutine());
        }
    }

    
    private void ApplyGyroForces()
    {
        Quaternion gyroAttitude = Input.gyro.attitude;
        gyroAttitude.z = -gyroAttitude.z; // Invert the z-axis if needed
        gyroAttitude.w = -gyroAttitude.w; // Invert the w-axis if needed

        // Convert to a rotation in 2D space
        Vector2 forceDirection = new Vector2(gyroAttitude.x, gyroAttitude.y);

        // Scale up the force for testing purposes
        float forceMultiplier = 10.0f; // Adjust this value as needed for testing
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

    
    
    private IEnumerator BalancingRoutine()
    {
        // Enable the gyroscope
        Input.gyro.enabled = true;

        // Give the player time to get ready
        yield return new WaitForSeconds(getReadyDuration);

        // Start the balancing countdown
        float countdown = balancingCountdown;
        while (countdown > 0)
        {
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
    
    private void UpdateCountdownDisplay(float time)
    {
        balancingCountdownText.text = "Balance for: " + Mathf.CeilToInt(time).ToString()+ "s";
    }

    private void CheckBalance()
    {
        if (touchingPairs.Count >= 3)
        {
            // Success logic here
            Debug.Log("Balanced Successfully!");
        }
        else
        {
            // Failure logic here
            Debug.Log("Failed to Balance!");
        }
        isBalancing = false;
        Input.gyro.enabled = false;
    }
}

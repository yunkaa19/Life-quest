using UnityEngine;

public class BellyRubGame : MonoBehaviour
{
    private Gyroscope gyro;
    private bool gyroEnabled;
    private bool isRubbing = false;

    void Start()
    {
        // Enable the gyroscope
        gyroEnabled = EnableGyro();
        
        // Enable the accelerometer
        Input.gyro.enabled = true;
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            
            Debug.Log("Gyro Enabled");
            
            return true;
        }
        return false;
    }

    void Update()
    {
        //Handheld.Vibrate();
        if (gyroEnabled)
        {
            DetectRubbingMotion();
        }
    }

    void DetectRubbingMotion()
    {
        // Use both gyro and accelerometer data to detect circular motion
        Vector3 gyroRotation = gyro.attitude.eulerAngles;
        Vector3 acceleration = Input.acceleration;

        // This is a placeholder for your circular motion detection logic
        // You would replace this with your actual detection algorithm
        if (Mathf.Abs(acceleration.x) > 0.5f && Mathf.Abs(acceleration.y) > 0.5f)
        {
            if (!isRubbing)
            {
                // Start the rubbing motion
                isRubbing = true;
                // Trigger haptic feedback
                Handheld.Vibrate();
            }
        }
        else
        {
            if (isRubbing)
            {
                // Stop the rubbing motion
                isRubbing = false;
            }
        }
    }
}
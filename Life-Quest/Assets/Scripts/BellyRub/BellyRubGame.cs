using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BellyRubGame : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Holds the variables for gyroscope control, game state flags, timers,
    /// and UI elements for the BellyRub game.
    /// </summary>

    private Gyroscope gyro;
    private bool gyroEnabled;
    private bool isRubbing = false;
    private float rubbingTimer = 0f; 
    private Coroutine countdownCoroutine;

    public TMP_Text countdownText;

    //fallback for non-Android or when vibration is not supported
    public Button startButton;
    private bool manualRubbing = false;
    
    [Header("Advanced Vibration")]
    public AdvancedVibration advancedVibration;        
    public float RubbingFastThreshold = 1.5f;
    public float RubbingSlowThreshold = 0.4f;
    
    //Values here are based on the values from collection of data from 30 tests. Values may need adjustment.
    [Header("Belly Rub Detection")]
    private float minAccelerationMagnitude = 0.912f; 
    private float maxAccelerationMagnitude = 1.123f; 
    private float gyroSensitivity = 0.921f; 
    
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initializes the gyroscope, accelerometer, and sets the initial countdown text.
    /// This method is called before the first frame update.
    /// </summary>
    void Start()
    {
        gyroEnabled = EnableGyro();
        if (gyroEnabled == true)
        {
            Destroy(startButton.gameObject);
        }
        Input.gyro.enabled = true;
        countdownText.text = "30";
    }


    /// <summary>
    /// Every frame the gyroscope status gets checked and updates
    /// the rubbing timer. Responds to real-time changes and player interactions.
    /// </summary>
    void Update()
    {
        if (gyroEnabled)
        {
            DetectRubbingMotion();
        }

        // Update the rubbing timer if currently rubbing
        if (isRubbing)
        {
            rubbingTimer += Time.deltaTime;
            if (rubbingTimer >= 5f)
            {
                // If rubbing stops for 5 seconds, reset the timer
                ResetCountdown();
            }
        }

    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Enables the gyroscope if the device supports it, device-specific features. Note that if the device does not support the gyroscope, the game will not run. Should add a fallback for this.
    /// </summary>
    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            return true;
        }
        return false;
    }
    /// <summary>
    /// Detects a rubbing motion using the device's accelerometer. 
    /// Core mechanic for the gameplay interaction.
    /// </summary>
    private void DetectRubbingMotion()
    {
        Vector3 acceleration = Input.acceleration;
        float accelerationMagnitude = acceleration.magnitude;
        Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;
        float gyroMagnitude = gyroRotationRate.magnitude;

        // Check if the acceleration and gyro are within the belly rub thresholds
        if (accelerationMagnitude > minAccelerationMagnitude && accelerationMagnitude < maxAccelerationMagnitude &&
            gyroMagnitude > gyroSensitivity)
        {
            if (!isRubbing)
            {
                StartRubbing();
            }
            rubbingTimer = 0f; // Reset timer as rubbing is detected
        }
        else
        {
            if (isRubbing)
            {
                // Continue to track the duration of no rubbing
                // Actual stopping of rubbing is handled in the Update method
            }
        }
    }
    
    
    private void StartRubbing()
    {
        isRubbing = true;
        advancedVibration.Vibrate();
        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(CountdownTimer());
        }
    }
    
    //support for advanced vibration
    private void UpdateVibrationPattern(float rubbingSpeed)
    {
        // Adjust the vibration pattern based on the speed
        if (rubbingSpeed > RubbingFastThreshold)
        {
            // Slow vibration for fast rubbing
            advancedVibration.pattern = new long[] { 0, 1000, 2000, 500 };
        }
        else if (rubbingSpeed < RubbingSlowThreshold)
        {
            // Fast vibration for slow rubbing
            advancedVibration.pattern = new long[] { 0, 100, 500, 100 };
        }
        else
        {
            // No vibration for perfect speed
            advancedVibration.pattern = new long[] { 0 };
        }
    }
    
    

    public void StartManualRubbing()
    {
        Destroy(startButton.gameObject);
        manualRubbing = true;
        StartCoroutine(CountdownWithoutGyro());
    }


    /// <summary>
    /// Manages the countdown timer for the game. As the exercise is to be done for 30 seconds, the timer is set to 30 seconds.
    /// </summary>
    IEnumerator CountdownTimer()
    {
        int timeLeft = 30; // 30 seconds countdown
        countdownText.text = timeLeft.ToString();

        while (timeLeft > 0 && isRubbing)
        {
            yield return new WaitForSeconds(1f);
            if (isRubbing) // Check again after waiting, in case rubbing stopped during the wait
            {
                timeLeft--;
                countdownText.text = timeLeft.ToString();
            }
        }

        if (!isRubbing)
        {
            // If rubbing stopped, reset everything
            ResetCountdown();
        }
        else
        {
            // Countdown finished successfully
            isRubbing = false;
            rubbingTimer = 0f;
            countdownCoroutine = null;

            LoadTargetScene();
        }
    }

    public void LoadTargetScene()
    {
        string targetScene = PlayerPrefs.GetString("TargetScene", "Main Menu");
        SceneManager.LoadScene(targetScene);

    }

    /// <summary>
    /// Resets the countdown and the game's rubbing state, affecting the game's state.
    /// </summary>

    private void ResetCountdown()
    {
        // Stop the current countdown
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        // Reset rubbing state
        isRubbing = false;
        rubbingTimer = 0f;

        // Reset the UI
        countdownText.text = "30";
    }

    /// <summary>
    /// This Coroutine is used as a fallback for when gyroscope is not available.
    /// </summary>
    IEnumerator CountdownWithoutGyro()
    {
        int timeLeft = 30; // 30 seconds countdown
        countdownText.text = timeLeft.ToString() + "s";

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            countdownText.text = timeLeft.ToString() + "s";
        }

        // Countdown finished
        isRubbing = false;
        rubbingTimer = 0f;
        countdownCoroutine = null;
        startButton.enabled = true;
    }
    #endregion
}

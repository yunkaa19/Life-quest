using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AdvancedVibration : MonoBehaviour
{
    public long[] pattern = { 0, 100, 1000, 300 }; // Wait, Vibrate, Wait, Vibrate...
    public int repeat = -1; // -1 means no repeat

    void Start()
    {
        Vibrate();
    }

    public void Vibrate()
    {
        // Check the Android version
        if (Application.platform == RuntimePlatform.Android && SystemInfo.supportsVibration)
        {
            using (var vibrator = new AndroidJavaClass("android.os.Vibrator"))
            {
                if (vibrator != null)
                {
                    // Create an instance of AndroidJavaClass UnityPlayer
                    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        // Get the current activity from UnityPlayer
                        using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            // Get the system service VIBRATOR_SERVICE from the activity
                            using (var systemService = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                            {
                                // Finally, you can invoke the vibrate function
                                // For API 26+ use VibrationEffect
                                if (AndroidVersionCheck(26))
                                {
                                    AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                                    AndroidJavaObject createWaveform = vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                                    systemService.Call("vibrate", createWaveform);
                                }
                                else
                                {
                                    // For older APIs, simply use the vibrate method
                                    systemService.Call("vibrate", pattern, repeat);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // Fallback for non-Android or when vibration is not supported
            Handheld.Vibrate();
        }
    }

    // Helper method to check the Android version
    private bool AndroidVersionCheck(int versionCode)
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT") >= versionCode;
        }
    }
}


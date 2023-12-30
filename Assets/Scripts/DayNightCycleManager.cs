// DayNightCycleManager.cs

using UnityEngine;

public class DayNightCycleManager : MonoBehaviour
{
    public float dayDurationInSeconds = 60f; // Duration of one day in seconds
    public float sunriseCountdown = 10f; // Countdown to sunrise in seconds
    public Light sunLight; // Directional light representing the sun
    public Material daySkybox; // Skybox material for daytime
    public Material nightSkybox; // Skybox material for nighttime

    private bool isTimerActive = false;
    private float timer;

    private void Start()
    {
        // Start the day/night cycle
        StartDayNightCycle();
    }

    private void Update()
    {
        // If the timer is active, update the cycle
        if (isTimerActive)
        {
            UpdateDayNightCycle();
        }
    }

    private void UpdateDayNightCycle()
    {
        // Update sun position (rotate around the scene)
        float angle = (Time.time / dayDurationInSeconds) * 360f;
        sunLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        // Update skybox based on time of day
        UpdateSkybox(angle);

        // If the timer is counting down, update it
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            // If the countdown reaches zero, stop the day/night cycle
            if (timer <= 0f)
            {
                StopDayNightCycle();
            }
        }
    }

    private void UpdateSkybox(float angle)
    {
        // Assuming angle > 180 means it's nighttime, otherwise it's daytime
        RenderSettings.skybox = (angle > 180f) ? nightSkybox : daySkybox;
    }

    public void StartDayNightCycle()
    {
        // Start the day/night cycle
        isTimerActive = true;
        timer = sunriseCountdown;
    }

    public void StopDayNightCycle()
    {
        // Stop the day/night cycle
        isTimerActive = false;
    }
}

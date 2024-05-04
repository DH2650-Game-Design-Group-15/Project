using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    public float timeMultiplier;
    public float nightTimeMultiplier;
    public string currentTimeString;
    public Light sunLight;
    public Light moonLight;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve ambientIntensityMultiplier;
    public AnimationCurve atmosphereMultiplier;
    public AnimationCurve dayExposureMultiplier;
    public AnimationCurve moonIntensityMultiplier;
    public Material daySkyBox;
    public Material nightSkyBox;
    private float timeSpeed;
    private float currentTime;
    private float sunPosition = 1f;
    
    private void Start () {
        UpdateTimeString();
        SetCurrentTime(8f);
        SetTimeSpeed(nightTimeMultiplier);
    }

    private void Update () {
        SetCurrentTime(GetCurrentTime() + Time.deltaTime * GetTimeSpeed());
        if (GetCurrentTime() >= 6 && GetCurrentTime() <= 18) {
            SetTimeSpeed(timeMultiplier);
        } else {
            SetTimeSpeed(nightTimeMultiplier);
        }
        UpdateTimeString();
        UpdateLight();
    }

    private void UpdateTimeString () {
        currentTimeString = Mathf.Floor(GetCurrentTime()).ToString("00") + ":" + ((GetCurrentTime()%1)*60).ToString("00");
    }

    /// <summary>
    /// Updates the lighting in the scene
    /// </summary>
    private void UpdateLight () {
        
        float sunRotation = GetCurrentTime() / 24f*360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, GetSunPosition(), 0f);
        moonLight.transform.rotation = Quaternion.Euler(sunRotation + 90f, GetSunPosition(), 0f);
        float normalizedTime = GetCurrentTime() / 24f;
        RenderSettings.ambientIntensity = ambientIntensityMultiplier.Evaluate(normalizedTime);
        float dayIntensity = (GetCurrentTime() - 6f) / 12f;
        sunLight.intensity = sunIntensityMultiplier.Evaluate(dayIntensity);
        moonLight.intensity = moonIntensityMultiplier.Evaluate(normalizedTime);
        nightSkyBox.SetFloat("_AtmosphereThickness", atmosphereMultiplier.Evaluate(normalizedTime));
        daySkyBox.SetFloat("_Exposure", dayExposureMultiplier.Evaluate(normalizedTime));
        
        if (GetCurrentTime() >= 5.2f && GetCurrentTime() <= 18.5f) {
            sunLight.gameObject.SetActive(true);
            RenderSettings.skybox = daySkyBox;
            RenderSettings.sun = sunLight;
        } else {
            sunLight.gameObject.SetActive(false);
        }
        if (GetCurrentTime() >= 5.5f && GetCurrentTime() <= 18.3f) {
            moonLight.gameObject.SetActive(false);
        } else {
            moonLight.gameObject.SetActive(true);
            RenderSettings.skybox = nightSkyBox;
            RenderSettings.sun = moonLight;
        }

        
    }

    private void SetCurrentTime (float time) {
        if (time >= 24) {
            currentTime = 0;
        } else {
            currentTime = time;
        }
    }

    private float GetCurrentTime () {
        return currentTime;
    }

    private void SetTimeSpeed (float mult) {
        timeSpeed = mult;
    }

    private float GetTimeSpeed () {
        return timeSpeed;
    }

    private void SetSunPosition (float pos) {
        sunPosition = pos;
    }

    private float GetSunPosition () {
        return sunPosition;
    }

}

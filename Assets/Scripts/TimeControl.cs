using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;

public class TimeControl : MonoBehaviour
{
    public Slider TimeSlider;
    public Button TimeButton;
    private float slowTimeDuration = 1.5f; // Zaman�n yava�lat�lma s�resi/2 (timescale 0.5 oldu�u i�in *2 s�re yava�lar)
    private float transitionDuration = 4f; // Slider'�n dolma s�resi
    private bool _canUse = true;

    private void Start()
    {
        TimeSlider.value = 1f; // Slider'�n ba�lang�� de�eri
    }

    public void SlowDownTime()
    {
        if (_canUse)
        {
            HapticFeedback.HeavyFeedback();
            StartCoroutine(SlowDownTimeCoroutine());
        }

    }

    private IEnumerator SlowDownTimeCoroutine()
    {
        _canUse = false;
        TimeButton.interactable = false;

        float startTime = Time.time;
        float endTime = startTime + slowTimeDuration;

        Time.timeScale = 0.5f;

        // 1'den 0'a do�ru ge�i�
        while (Time.time < endTime)
        {
            float normalizedTime = (Time.time - startTime) / slowTimeDuration;
            TimeSlider.value = Mathf.Lerp(1f, 0f, normalizedTime);
            yield return null;
        }

        Time.timeScale = 1f;

        startTime = Time.time;
        endTime = startTime + transitionDuration;

        // 0'dan 1'e do�ru ge�i�
        while (Time.time < endTime)
        {
            float normalizedTime = (Time.time - startTime) / transitionDuration;
            TimeSlider.value = Mathf.Lerp(0f, 1f, normalizedTime);
            yield return null;
        }
        TimeButton.interactable = true;
        _canUse = true;
        HapticFeedback.MediumFeedback();
        TimeSlider.value = 1f; // Slider'� bir'e ayarla
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static GameController;

public class UIController : MonoBehaviour
{
    //UI
    [SerializeField] private TextMeshProUGUI _gpsInfo;
    [SerializeField] private TextMeshProUGUI _speedDial;
    [SerializeField] private TextMeshProUGUI _goalTime;

    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Slider _boostSlider;


    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnBoostChange(float fuelLevel);
    public static OnBoostChange onBoostChange;

    private void OnEnable()
    {
        onBoostChange += UpdateBoostBar;
    }
    private void OnDisable()
    {
        onBoostChange -= UpdateBoostBar;
    }
    public void UpdateUI(float speed, float etaSecs, float goaltime, float progress)
    {
        TimeSpan time = TimeSpan.FromSeconds(etaSecs);
        TimeSpan goalTime = TimeSpan.FromSeconds(goaltime);

        _gpsInfo.text = $"{time}";
        _progressSlider.value = progress;
        _speedDial.text = $"{speed}";
        _goalTime.text = $"{goalTime}";
    }
    public void UpdateBoostBar(float fuel)
    {
        fuel /= 100;
        _boostSlider.value = fuel;
    }
}

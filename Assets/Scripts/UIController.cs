using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _etaInfoText;
    [SerializeField] private TextMeshProUGUI _goalTimeText;
    [SerializeField] private TextMeshProUGUI _speedDialText;

    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Slider _boostSlider;

    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnBoostChange(float fuelLevel);
    public static OnBoostChange onBoostChange;

    public delegate void OnUIChange(float speed, float etaSecs, float goaltime, float progress);
    public static OnUIChange onUIChange;

    private void OnEnable()
    {
        onBoostChange += UpdateBoostBar;
        onUIChange += UpdateUI;
    }
    private void OnDisable()
    {
        onBoostChange -= UpdateBoostBar;
        onUIChange -= UpdateUI;
    }

    private void UpdateUI(float speed, float etaSecs, float goaltime, float progress)
    {
        TimeSpan time = TimeSpan.FromSeconds(etaSecs);
        TimeSpan goalTime = TimeSpan.FromSeconds(goaltime);

        _etaInfoText.text = $"{time}";
        _goalTimeText.text = $"{goalTime}";

        _speedDialText.text = $"{speed}";
        _progressSlider.value = progress;
    }

    private void UpdateBoostBar(float fuel)
    {
        fuel /= 100;
        _boostSlider.value = fuel;
    }
}

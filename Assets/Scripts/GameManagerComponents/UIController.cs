using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _etaInfoText;
    [SerializeField] private TextMeshProUGUI _goalTimeText;
    private GameObject _speedDial;
    [SerializeField]private Image _speedDialImage;
    [SerializeField]private Image _alertDialImage;
    [SerializeField] private TextMeshProUGUI _speedDialText;

    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Slider _boostSlider;

    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnBoostChange(float fuelLevel);
    public static OnBoostChange onBoostChange;

    //event called by telemetry to update the player to the time and progress
    public delegate void OnUIChange(float speed, float etaSecs, float goaltime, float progress);
    public static OnUIChange onUIChange;

    //Event for changing the state of the ui for animations etc.
    public delegate void OnUiStateChange(UIState _uistate);
    public static OnUiStateChange onUiStateChange;

    //Event for changing the state of the ui for animations etc.
    public delegate void OnDroneAlert(bool isAlerted);
    public static OnDroneAlert onDroneAlert;

    public enum UIState { Penalty, Normal, Boost }
    private UIState _uiState { get; set; }

    private void OnEnable()
    {
        onBoostChange += UpdateBoostBar;
        onUIChange += UpdateUI;
        onUiStateChange += UpdateSpeedDialGraphic;
        onDroneAlert += DroneAlert;
    }

    private void OnDisable()
    {
        onBoostChange -= UpdateBoostBar;
        onUIChange -= UpdateUI;
        onUiStateChange -= UpdateSpeedDialGraphic;
        onDroneAlert -= DroneAlert;
    }

    private void Awake()
    {
        _uiState = UIState.Normal;
        UpdateSpeedDialGraphic(_uiState);
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

    private void UpdateSpeedDialGraphic(UIState uiState)
    {
        Color colorRef = Color.magenta;

        switch (uiState)
        {
            case UIState.Penalty:
                colorRef = Color.yellow;
                break;
            case UIState.Normal:
                colorRef = Color.green;
                break;
            case UIState.Boost:
                colorRef = Color.red;
                break;
        }
        StartCoroutine(ColourChange(colorRef));
    }
    private void DroneAlert(bool isAlerted)
    {
        StopAllCoroutines();
        StartCoroutine(SirenEffect(isAlerted));
    }

    private IEnumerator SirenEffect(bool isAlerted)
    {
        Debug.Log("Alert");
        var targetCol1 = Color.red;
        var targetCol2 = Color.blue;
        float dur = 0.5f;

        Color myColour;
        while (isAlerted)
        {
            float timer = 0f;
            while (timer < dur)
            {
                myColour = Color.Lerp(targetCol1, targetCol2, timer);
                _alertDialImage.color = myColour;
                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0f;
            while (timer < dur)
            {
                myColour = Color.Lerp(targetCol2, targetCol1, timer);
                _alertDialImage.color = myColour;
                timer += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        Debug.Log("Stopping siren effect");
        myColour = Color.white;
        _alertDialImage.color = myColour;
        yield return null;
    }

    private IEnumerator ColourChange(Color targetCol)
    {
        var mycolour = _speedDialImage.color;
        float timer = 0f;
        while (timer < 1f)
        {
            mycolour = Color.Lerp(mycolour, targetCol, timer);
            _speedDialImage.color = mycolour;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}

using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField]private Light[] jetLights;
    public bool IsBoosting { get; set; }

    float fuel = 100f;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Booster()
    {
        IsBoosting = true;
        //use fuel
        StartCoroutine(Boosting());

        //play boost animation
        StartCoroutine(Boost());

    }
    private IEnumerator Boost()
    {
        PlayerBoostSFX();

        float progress = 0f;
        float duration = 0.5f;

        float boostPos = 2;
        var startSpeed = GameController.Instance.StandardSpeed_MPH;
        var boostSpeed = GameController.Instance.BoostSpeed_MPH;

        var originPos = transform.position;
        var targetPos = originPos + new Vector3(0, 0, boostPos);

        var jetMat = transform.GetChild(0).GetComponent<Renderer>().material;

        GameController.onSpeedChange?.Invoke(boostSpeed);

        while (!Input.GetKeyUp(KeyCode.W) && IsBoosting)
        {
            transform.SetPositionAndRotation(
            Vector3.Lerp(
                transform.position,
                targetPos,
                progress),
            Quaternion.Slerp(
                transform.rotation,
                Quaternion.identity,
                progress));

            jetMat.SetFloat("_BoostPower", Mathf.Lerp(0, 5, progress));

            foreach (var light in jetLights)
            {
                light.intensity = Mathf.Lerp(0, 100, progress);
            }

            progress += Time.deltaTime / duration;
            yield return null;
        }

        if (IsBoosting)
        {
            IsBoosting = false;
        }
        PlayerBoostSFX();

        duration = 0.25f;
        progress = 0f;

        while (progress <= 1f)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(
                    transform.position,
                    originPos,
                    progress),
                Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.identity,
                    progress));

            jetMat.SetFloat("_BoostPower", Mathf.Lerp(jetMat.GetFloat("_BoostPower"), 0, progress));

            foreach (var item in jetLights)
            {
                item.intensity = Mathf.Lerp(item.intensity, 0, progress);
            }

            progress += Time.deltaTime / duration;
            yield return null;
        }

        GameController.onSpeedChange?.Invoke(startSpeed);

        jetMat.SetFloat("_BoostPower", 0);
    }
    private void PlayerBoostSFX()
    {
        var audioController = GetComponent<AudioSourceController>();
        audioController.ToggleJetSFX();
    }

    //Deplete and Recharge boost bar
    private IEnumerator Boosting()
    {
        while (IsBoosting && fuel > 0)
        {
            fuel -= Time.deltaTime * 10;
            UIController.onBoostChange?.Invoke(fuel);
            yield return null;
        }
        IsBoosting = false;
        StartCoroutine(Refueling());
    }

    private IEnumerator Refueling()
    {
        while (!IsBoosting && fuel <= 100)
        {
            fuel += Time.deltaTime * 5f;
            UIController.onBoostChange?.Invoke(fuel);
            yield return null;
        }
    }
}

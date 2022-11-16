using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour
{
    [SerializeField] Slider boostBar;

    ChunkSpawner[] chunkSpawners;

    public bool IsBoosting { get; set; }


    private void Start()
    {
        chunkSpawners = transform.GetComponent<PlayerController>().ChunkSpawners;
    }

    public void Booster()
    {
        //use fuel
        StartCoroutine(Boosting());
        StartCoroutine(Boost());
    }

    private IEnumerator Boost()
    {
        PlayerBoostSFX();

        float progress = 0f;
        float duration = 0.5f;

        float boostPos = 2;
        var startSpeed = GetComponent<PlayerController>().CurrentSpeed;
        var boostSpeed = GetComponent<PlayerController>().BoostSpeed;

        var originPos = transform.position;
        var targetPos = originPos + new Vector3(0, 0, boostPos);

        var jetMat = transform.GetChild(0).GetComponent<Renderer>().material;
        var jetLights = transform.GetComponentsInChildren<Light>();

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

            foreach (var item in jetLights)
            {
                item.intensity = Mathf.Lerp(0, 100, progress);
            }

            foreach (var chunk in chunkSpawners)
            {
                chunk.movingSpeed = Mathf.Lerp(startSpeed, boostSpeed, progress);
            }
            progress += Time.deltaTime / duration;
            yield return null;
        }
        if (IsBoosting) { IsBoosting = false; }
        duration = 0.25f;
        progress = 0f;
        PlayerBoostSFX();
        while (progress <= 1f)
        {
            //transform.position = Vector3.Lerp(transform.position, originPos, progress);
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
            foreach (var chunk in chunkSpawners)
            {
                chunk.movingSpeed = Mathf.Lerp(chunk.movingSpeed, startSpeed, progress);
            }

            progress += Time.deltaTime / duration;
            yield return null;
        }
        jetMat.SetFloat("_BoostPower", 0);
    }

    private void PlayerBoostSFX()
    {
        var audioController = GetComponent<AudioSourceController>();
        audioController.ToggleJetSFX();
    }

    private IEnumerator Boosting()
    {
        if (IsBoosting) yield break;
        IsBoosting = true;
        while (IsBoosting && boostBar.value >= 0.01f)
        {
            //Debug.Log(boostBar.value);
            boostBar.value -= Time.deltaTime * 0.1f;
            yield return null;
        }
        IsBoosting = false;
        StartCoroutine(Refueling());
    }

    private IEnumerator Refueling()
    {
        if (IsBoosting) yield break;
        while (!IsBoosting && boostBar.value <= 1)
        {
            boostBar.value += Time.deltaTime * 0.1f;
            yield return null;
        }
    }
}

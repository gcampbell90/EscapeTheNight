using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ChunkSpawner[] chunkSpawners;
    public ChunkSpawner[] ChunkSpawners { get { return chunkSpawners; } set { ChunkSpawners = chunkSpawners; } }
    float borderRange = 8;

    public float normal_MPH;
    public float boost_MPH;
    public float NormalSpeed { get; set; }
    public float BoostSpeed { get; set; }
    public float Speed { get; set; }

    private float normalTimer = 0f;
    private float boostTimer = 0f;

    public float totalLevelDistanceMiles;
    public float DistanceCovered { get; set; }
    public float TotalDistanceRemaining { get; set; }

    private const float meterToMile = 0.00062f;
    private const float mphToMs = 0.44704f;
    private const float msToMph = 2.2369f;
    public TextMeshProUGUI gps;

    public Slider progressSlider;

    bool dir;
    float rot;
    float pos;

    public Vector3 TartgetPos => SetTargetPosition();
    public Quaternion TargetRot => SetTargetRotation();

    BoostController boostController;

    private void Awake()
    {
        NormalSpeed = normal_MPH * mphToMs;
        BoostSpeed = boost_MPH * mphToMs;
        Speed = NormalSpeed;

        boostController = GetComponent<BoostController>();

        foreach (var chunk in chunkSpawners)
        {
            chunk.movingSpeed = Speed;
        }

    }

    float distanceNormal, distanceBoost;
    float timer;
    // Update is called once per frame
    void Update()
    {
        CalculateMetrics();
        HandleInput();
    }

    private void UpdateUI(float countDown, float eta, string message, Color col)
    {
        gps.color = col;
        gps.text = $"{message} Gate Closing in {countDown.ToString("#.##")}\nDistance Rem(Miles):{TotalDistanceRemaining.ToString(".##")}\nETA: {eta.ToString("#.##")}";
    }

    private void CalculateMetrics()
    {
        float countDown, eta;
        string message;
        Color col;

        if (!boostController.IsBoosting)
        {
            //Debug.Log("normal vel:");
            Speed = NormalSpeed;
            distanceNormal = Speed * normalTimer;
            normalTimer += Time.deltaTime;
        }
        else if (boostController.IsBoosting)
        {
            //Debug.Log("Boost vel:");
            Speed = BoostSpeed;
            distanceBoost = Speed * boostTimer;
            boostTimer += Time.deltaTime;
        }

        DistanceCovered = (distanceNormal * meterToMile) + (distanceBoost * meterToMile);

        TotalDistanceRemaining = totalLevelDistanceMiles - DistanceCovered;

        var progress = DistanceCovered / totalLevelDistanceMiles;
        progressSlider.value = progress;

        var targetETA = ((totalLevelDistanceMiles / normal_MPH)) * 0.8f;
        countDown = targetETA - timer;
        eta = (TotalDistanceRemaining / normal_MPH);
        message = (eta < countDown) ? "Good - Dont let up!" : "Hurry!";
        col = (eta < countDown) ? Color.green : Color.red;
        timer += Time.deltaTime;

        UpdateUI(countDown, eta, message, col);

    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            boostController.Booster();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            //Dont allow turning if boosting
            if (boostController.IsBoosting) return;

            //StopCoroutine(RotateY());
            if (Input.GetKeyDown(KeyCode.A))
            {
                dir = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                dir = false;
            }


            //if (isRunningMove) StopCoroutine(MoveX());
            //if (isRunningRot) StopCoroutine(RotateY());

            //Separate for now, find a way to merge when/if including physics or velocity etc.
            StartCoroutine(MoveX());
            //StartCoroutine(RotateY());
        }
    }

    bool isRunningMove;
    IEnumerator MoveX()
    {
        //if (isRunningMove) yield break;
        isRunningMove = true;

        float progress = 0f;
        float duration = 0.5f;

        var origin = transform.position;

        while (progress <= 1f)
        {
            transform.position = Vector3.Lerp(origin, TartgetPos, progress);
            progress += Time.deltaTime / duration;
            //Debug.Log("Progress " + progress);
            yield return null;
        }

        isRunningMove = false;

        //Debug.Log("Changing Lanes" + dir + " " + targetPos);
    }
    Vector3 SetTargetPosition()
    {
        pos = dir ? -borderRange / 2 : borderRange / 2;
        return new Vector3(pos, 0, 0);
    }

    bool isRunningRot;
    private IEnumerator RotateY()
    {
        isRunningRot = true;

        //PlayerTurnSFX();
        float progress = 0f;
        float duration = 0.3f;

        //Rotate
        while (progress <= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot, progress);
            progress += Time.deltaTime / duration;
            yield return null;
        }
        //reset rotation
        duration = 0.15f;
        progress = 0f;
        while (progress <= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, progress);
            progress += Time.deltaTime / duration;
            yield return null;
        }
        isRunningRot = false;
    }
    private void PlayerTurnSFX()
    {
        var audioController = GetComponent<AudioSourceController>();
        audioController.PlayTireScreech();
    }

    Quaternion SetTargetRotation()
    {
        rot = dir ? -25 : 25;
        return Quaternion.Euler(0, rot, 0);
    }

}

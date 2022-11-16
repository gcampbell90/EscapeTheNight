using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Player Setup")]
    [SerializeField] PlayerController playerController;
    [SerializeField] private float standardSpeed_MPH;
    [SerializeField] private float boostSpeed_MPH;

    [Header("Level Setup")]
    [SerializeField] private float levelLength_Miles;
    public float StandardSpeed_MPH { get => standardSpeed_MPH; private set => standardSpeed_MPH = value; }
    public float BoostSpeed_MPH { get => boostSpeed_MPH; private set => boostSpeed_MPH = value; }
    public float LevelLength_Miles { get => levelLength_Miles; private set => levelLength_Miles = value; }
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

}


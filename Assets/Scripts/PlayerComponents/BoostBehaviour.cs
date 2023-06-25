using System.Collections;
using UnityEngine;

public class BoostBehaviour : MonoBehaviour
{
    public delegate void BoostControllerDelegate(bool boost);
    public static BoostControllerDelegate boostEvent;

    [SerializeField] private Light[] jetLights;
    float fuel = 100f;
    private bool IsBoosting = false;

    private void OnEnable()
    {
        boostEvent += ActivateBooster;
    }
    private void OnDisable()
    {
        boostEvent -= ActivateBooster;
    }

    public void ActivateBooster(bool boost)
    {
        IsBoosting = boost;

        if (boost)
        {
            //play boost animation
            StartCoroutine(BoostCoroutine());
        }
    }
    private IEnumerator BoostCoroutine()
    {
        PlayBoostSFX();
        //use fuel
        StartCoroutine(DepleteFuel());
        StartCoroutine(AnimateLights());
        float progress = 0f;
        float duration = 0.5f;

        float boostPos = 2;
        var startSpeed = GameController.Instance.StandardSpeed_MPH;
        var boostSpeed = GameController.Instance.BoostSpeed_MPH;

        var originPos = transform.position;
        var targetPos = originPos + new Vector3(0, 0, boostPos);
        var originRot = Quaternion.Euler(-90, 0, 0);

        var jetMat = transform.GetChild(0).GetComponent<Renderer>().material;

        GameController.onSpeedChange?.Invoke(boostSpeed);

        while (IsBoosting)
        {
            transform.SetPositionAndRotation(
            Vector3.Lerp(
                transform.position,
                targetPos,
                progress),
            Quaternion.Slerp(
                transform.rotation,
                originRot,
                progress));

            jetMat.SetFloat("_BoostPower", Mathf.Lerp(0, 5, progress));

            progress += Time.deltaTime / duration;
            yield return null;
        }

        duration = 0.25f;
        progress = 0f;

        while (progress <= 0.5f)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(
                    transform.position,
                    originPos,
                    progress),
                Quaternion.Slerp(
                    transform.rotation,
                    originRot,
                    progress));

            jetMat.SetFloat("_BoostPower", Mathf.Lerp(jetMat.GetFloat("_BoostPower"), 0, progress));
            progress += Time.deltaTime / duration;
            yield return null;
        }

        GameController.onSpeedChange?.Invoke(startSpeed);

        jetMat.SetFloat("_BoostPower", 0);
    }
    private IEnumerator AnimateLights()
    {
        float minIntensity = Random.Range(10f, 30f);
        float maxIntensity = Random.Range(50f, 100f);

        while (IsBoosting)
        {
            float randomValue = Random.Range(minIntensity, maxIntensity);
            jetLights[0].intensity = randomValue;

            yield return new WaitForSeconds(0.1f);
        }
        jetLights[0].intensity = 0f;
    }
    private void PlayBoostSFX()
    {
        var audioController = GetComponent<PlayerAudioBehaviour>();
        audioController.ToggleJetSFX();
    }

    //Deplete and Recharge boost bar
    private IEnumerator DepleteFuel()
    {
        while (IsBoosting && fuel > 0)
        {
            fuel -= Time.deltaTime * 20;
            UIController.onBoostChange?.Invoke(fuel);
            yield return null;
        }
        IsBoosting = false;
        StartCoroutine(Refuel());
    }
    private IEnumerator Refuel()
    {
        while (!IsBoosting && fuel <= 100)
        {
            fuel += Time.deltaTime * 5f;
            UIController.onBoostChange?.Invoke(fuel);
            yield return null;
        }
    }
}

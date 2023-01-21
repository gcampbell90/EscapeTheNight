using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioBehaviour : MonoBehaviour
{
    [SerializeField] AudioClip engineRunningClip;
    [SerializeField] AudioClip jetStartingClip;
    [SerializeField] AudioClip jetRunningClip;
    [SerializeField] AudioClip jetEndingClip;

    [SerializeField]
    AudioClip[] tireScreeching;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayEngine();
    }
    public void PlayEngine()
    {
        audioSource.clip = engineRunningClip;
        audioSource.loop = true;
        audioSource.pitch = 0.9f;
        audioSource.Play();
    }

    bool toggle;
    public void ToggleJetSFX()
    {
        toggle = !toggle;
        if (toggle)
        {
            StartCoroutine(PlayJetSFX());
            //Debug.Log("Starting Jet SFX");
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(JetEndingClip());
        }
    }
    AudioSource oneShot;
    public void PlayTireScreech()
    {
        if (oneShot == null) {
            oneShot = gameObject.AddComponent<AudioSource>();
        }
        StartCoroutine(TireScreech());
    }
    IEnumerator TireScreech()
    {
        var randomClip = tireScreeching[Random.Range(0, tireScreeching.Length)];
        audioSource.volume = 0.7f;
        audioSource.PlayOneShot(randomClip);
        while (oneShot.isPlaying)
        {
            yield return null;
        }

    }

    private IEnumerator PlayJetSFX()
    {

        audioSource.clip = jetStartingClip;
        audioSource.loop = false;
        audioSource.pitch = 1.3f;
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.clip = jetRunningClip;
        audioSource.loop = true;
        audioSource.pitch = 1.3f;
        audioSource.Play();

        while (audioSource.isPlaying && !Input.GetKeyUp(KeyCode.W))
        {
            yield return null;
        }

        yield return StartCoroutine(JetEndingClip());

    }

    private IEnumerator JetEndingClip()
    {

        audioSource.clip = jetEndingClip;
        audioSource.loop = false;
        audioSource.pitch = 1.1f;
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        PlayEngine();
    }
}

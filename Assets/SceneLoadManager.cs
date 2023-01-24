using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager _instance;

    public static SceneLoadManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Update()
    {
        // Press the space key to start coroutine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    public async Task LoadMainSceneAsync()
    {
        //StartCoroutine(LoadYourAsyncScene());
        await LoadSceneAsync();
    }

    public async Task LoadSceneAsync()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }

        //Unload intro scene before destroying scene loader
        asyncLoad = SceneManager.UnloadSceneAsync("IntroScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }
        Destroy(this);
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Destroy(this);
    }
}

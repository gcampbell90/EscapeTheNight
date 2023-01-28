using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance { get { return _instance; } }

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

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Press the space key to start coroutine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Use a coroutine to load the Scene in the background
            LoadMainSceneAsync();
        }
    }

    public async Task LoadMainSceneAsync()
    {
        await LoadSceneAsync("MainScene");
        await UnloadSceneAsync("IntroScene");
    }

    public async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while(asyncLoad.progress < 0.9f)
        {
            //Debug.Log(asyncLoad.progress);
            await Task.Yield();
        }
        //Activate the Scene
        asyncLoad.allowSceneActivation = true;


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public async Task UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnLoad = SceneManager.UnloadSceneAsync(sceneName);
        // Wait until the asynchronous scene fully loads
        while (!asyncUnLoad.isDone)
        {
            await Task.Yield();
        }
    }

    public void MoveGameObject(GameObject m_MyGameObject)
    {
        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(m_MyGameObject, SceneManager.GetSceneByName("MainScene"));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroTextBehaviour : ScrollingTextBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if (GameController.Instance.isWinner)
        {
            WinText();
        }
        else
        {
            LoseText();
        }
    }

    public async void Restart()
    {
        await SceneController.Instance.LoadSceneAsync("IntroScene");
        await SceneController.Instance.UnloadSceneAsync("OutroScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

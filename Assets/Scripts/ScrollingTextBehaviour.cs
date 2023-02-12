using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

public class ScrollingTextBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text scrollingText;
    [SerializeField] private float scrollSpeed = 0.1f;
    [TextArea(minLines: 1, maxLines: 50)][SerializeField] private string textToShow = "This is a sample text for scrolling text";
    [SerializeField] private float fadeDuration = 2f;

    private int currentIndex = 0;
    private float delay = 0.5f;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartScrolling(textToShow);
    }

    public void Outro()
    {
        Debug.Log("Outro Shitttt");
    }

    public void StartScrolling(string text)
    {
        textToShow = text;
        scrollingText.text = "";
        currentIndex = 0;
        InvokeRepeating("UpdateTextAsync", 0f, scrollSpeed);
    }

    private async Task UpdateTextAsync()
    {
        scrollingText.text += textToShow[currentIndex];
        currentIndex++;

        if (currentIndex == textToShow.Length)
        {
            CancelInvoke();
            StartCoroutine(FadeText());
            return;
        }
        if (textToShow[currentIndex] == ',' || textToShow[currentIndex] == '.')
        {
            CancelInvoke();
            InvokeRepeating("UpdateTextAsync", delay, scrollSpeed);
        }
    }

    private IEnumerator FadeCanvas()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = 1 - (elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    private IEnumerator FadeText()
    {
        var textCol = scrollingText.color;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textCol.a = 1 - (elapsedTime / fadeDuration);
            scrollingText.color = textCol;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textCol.a = 0;
        //StartCoroutine(FadeCanvas());
    }

    public void WinText()
    {
        scrollingText.text = "You have escaped the city";
    }

    public void LoseText()
    {
        scrollingText.text = "You have been captured";
    }

}

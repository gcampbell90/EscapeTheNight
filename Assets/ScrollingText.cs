using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public TMP_Text scrollingText;
    public float scrollSpeed = 0.1f;
    [SerializeField] private string textToShow = "This is a sample text for scrolling text";
    private bool scrolling = false;

    private void Start()
    {
        StartScrolling(textToShow);
    }

    public void StartScrolling(string text)
    {
        textToShow = text;
        scrollingText.text = "";
        scrolling = true;
        InvokeRepeating("UpdateText", 0f, scrollSpeed);
    }

    private void UpdateText()
    {
        if (scrolling)
        {
            if (scrollingText.text.Length >= textToShow.Length)
            {
                scrollingText.text = "";
            }
            scrollingText.text += textToShow[0];
            textToShow = textToShow.Substring(1) + textToShow[0];
            if (textToShow[0] == ',' || textToShow[0] == '.')
            {
                CancelInvoke();
                InvokeRepeating("UpdateText", 0.5f, scrollSpeed);
            }
        }
    }
}

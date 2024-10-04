using UnityEngine;
using TMPro;

public class TextFader : MonoBehaviour
{
    public TextMeshProUGUI text; // Reference to the TextMeshPro component
    public float fadeDuration = 0.5f; // Time for fade in and out
    private bool fadingIn = true; // Track whether we're fading in or out
    private float targetAlpha = 0.5f; // The target alpha value (1 = fully visible)

    void Start()
    {
        // Initialize text with full transparency (alpha = 0)
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        Color tempColor = text.color;
        tempColor.a = 0;
        text.color = tempColor;
    }

    void Update()
    {
        // Determine the target alpha based on whether we're fading in or out
        if (fadingIn)
        {
            targetAlpha = 1.0f; // Fully visible
        }
        else
        {
            targetAlpha = 0.0f; // Fully transparent
        }

        // Gradually change the alpha value
        Color currentColor = text.color;
        float alpha = Mathf.MoveTowards(currentColor.a, targetAlpha, Time.deltaTime / fadeDuration);
        text.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

        // Check if we've reached the target alpha and reverse the fade direction
        if (Mathf.Approximately(alpha, targetAlpha))
        {
            fadingIn = !fadingIn;
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuChangeAllUi : MonoBehaviour
{
    float maxWight = 200;
    float maxHeight = 100;
    float maxFont = 30;

    void Start()
    {
#if UNITY_ANDROID
        Button[] butttons = FindObjectsOfType<Button>();
        foreach (Button button in butttons)
        {
            RectTransform rect = button.GetComponent<RectTransform>();
            Vector2 size = rect.sizeDelta;
            if(size.x > maxWight) size.x = maxWight;
            if (size.y > maxHeight) size.y = maxHeight;
            rect.sizeDelta = size;
        }
        Text[] texts = FindObjectsOfType<Text>();
        foreach (Text text in texts)
        {
            if (text.fontSize > maxFont) text.fontSize = (int)maxFont;
        }
#endif
    }

}

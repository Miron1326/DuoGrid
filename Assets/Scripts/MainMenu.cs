using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private bool Close;
    private GameObject MainMenuGameObject;
    private void Start()
    {
        MainMenuGameObject = GameObject.Find("PausePanel");
        Close = true;
        CloseMenu();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Close = !Close;
            if(!Close)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }

        }
    }
    public void OpenMenu()
    {
        GameManager.Instance.OpenInfo();
        CanvasGroup canvasGroup = MainMenuGameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = MainMenuGameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // включить все Image компоненты
        foreach (Image image in MainMenuGameObject.GetComponentsInChildren<Image>(true))
        {
            image.enabled = true;
        }

        // включить все Text компоненты
        foreach (Text text in MainMenuGameObject.GetComponentsInChildren<Text>(true))
        {
            text.enabled = true;
        }
    }
    public void CloseMenu()
    {
        GameManager.Instance.CloseInfo();
        CanvasGroup canvasGroup = MainMenuGameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = MainMenuGameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Отключить все Image компоненты
        foreach (Image image in MainMenuGameObject.GetComponentsInChildren<Image>(true))
        {
            image.enabled = false;
        }

        // Отключить все Text компоненты
        foreach (Text text in MainMenuGameObject.GetComponentsInChildren<Text>(true))
        {
            text.enabled = false;
        }
    }
}

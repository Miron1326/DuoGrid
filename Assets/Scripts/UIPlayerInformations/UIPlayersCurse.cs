using UnityEngine;

public class UIPlayersCurse : MonoBehaviour
{
    public CanvasGroup CursePlayer1;
    public CanvasGroup CursePlayer2;

    void Start()
    {
        CursePlayer1 = GameObject.Find("CursePlayer1").GetComponent<CanvasGroup>();
        CursePlayer2 = GameObject.Find("CursePlayer2").GetComponent<CanvasGroup>();
    }

    public void ShowCursePlayer1()
    {
        Debug.LogError(12);
        CursePlayer1.interactable = true;
        CursePlayer1.alpha = 1;
        CursePlayer1.blocksRaycasts = true;
    }
    public void ShowCursePlayer2()
    {
        Debug.LogError(22);
        CursePlayer2.interactable = true;
        CursePlayer2.alpha = 1;
        CursePlayer2.blocksRaycasts = true;
    }

    public void FadeCursePlayer1()
    {
        Debug.LogError(13);
        CursePlayer1.interactable = false;
        CursePlayer1.alpha = 0;
        CursePlayer1.blocksRaycasts = false;
    }
    public void FadeCursePlayer2()
    {
        Debug.LogError(23);
        CursePlayer2.interactable = false;
        CursePlayer2.alpha = 0;
        CursePlayer2.blocksRaycasts = false;
    }
}

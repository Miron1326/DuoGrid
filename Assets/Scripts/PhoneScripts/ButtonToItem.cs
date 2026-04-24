using UnityEngine;

public class ButtonToItem : MonoBehaviour
{
    public void ButtonClicked()
    {
        MovementOnTheMouseManager movementOnTheMouseManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
        if(movementOnTheMouseManager.Player1Move)
        {
            Debug.LogError(GameManager.Instance.Player1OpenItems);
            if (!GameManager.Instance.Player1OpenItems)
            {
                GameManager.Instance.Player1ItemsOpenCanvas();
            }
            else
            {
                GameManager.Instance.Player1ItemsCloseCanvas();
            }

        }
        else
        {
            Debug.LogError(GameManager.Instance.Player2OpenItems);
            if (!GameManager.Instance.Player2OpenItems)
            {
                GameManager.Instance.Player2ItemsOpenCanvas();
            }
            else
            {
                GameManager.Instance.Player2ItemsCloseCanvas();
            }
        }
    }
}

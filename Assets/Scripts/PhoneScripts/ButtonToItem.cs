using UnityEngine;

public class ButtonToItem : MonoBehaviour
{
    public void ButtonClicked()
    {
        MovementOnTheMouseManager movementOnTheMouseManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
        if(movementOnTheMouseManager.Player1Move)
        {
            if (!GameManager.Instance.player1IsOpenItems)
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
            if (!GameManager.Instance.player2IsOpenItems)
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

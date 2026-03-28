using UnityEngine;

public class UITurnInfo : MonoBehaviour
{
    private Vector3 newPos;
    private GameObject player1;
    private GameObject player2;
    private void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
    }
    private void Update()
    {
        if(GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
        {
            if(newPos != player1.transform.position)
            {
                newPos = player1.transform.position;
                transform.position = newPos + new Vector3(0, 1.7f, 0);
            }

        }
        if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
        {
            if (newPos != player2.transform.position)
            {
                newPos = player2.transform.position;
                transform.position = newPos + new Vector3(0, 1.7f, 0);
            }

        }
    }
}

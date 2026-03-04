using UnityEngine;

public class GameManagerAI : MonoBehaviour
{

    public Vector2 aiPosition;
    public Vector2 playerPosition;
    public int aiHealth;
    private GameObject AIPlayer;
    public float M;
    void Start()
    {
        aiHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MovementToOneCell();
        }
        AIPlayer = GameObject.Find("AI");
        aiPosition = AIPlayer.transform.position;
        playerPosition = GameObject.Find("Player1").transform.position;
    }

    public void MovementToOneCell()
    {
        AIPlayer.transform.position += new Vector3(M, 0);
    }
}

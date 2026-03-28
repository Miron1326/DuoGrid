using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 currentPos;
    private int speedx = 2;
    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        currentPos = transform.position;
        if(currentPos.x > 37)
        {
            transform.position = startPos;
        }
        else
        {
            transform.position += new Vector3(speedx, 0, 0) * Time.deltaTime;
        }
    }
}

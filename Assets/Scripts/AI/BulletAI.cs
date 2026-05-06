using UnityEngine;

public class BulletAI : MonoBehaviour
{
    public string nameCollision;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == nameCollision)
        {
            Destroy(gameObject);
        }
    }
}

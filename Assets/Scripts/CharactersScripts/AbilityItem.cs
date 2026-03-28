using UnityEngine;

public class AbilityItem : MonoBehaviour
{
    public Collider2D[] colliders;
    public AbilityType abilityType;
    private Vector2 sizeCheckAround = new Vector2(0.1f,0.1f);
    private Vector2 VectorToCheckSomeItems = new Vector2(2, 2);
    public void Start()
    {
        switch (abilityType)
        {
            case AbilityType.Aprodox:
                GameManager.Instance.OnSwitchTurn += CheckAround;
                GameManager.Instance.OnSwitchTurn += CheckToDestroyAndExplosionSome;
                break;
        }
    }

    private void CheckAround()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, sizeCheckAround, 0);
        foreach (Collider2D collider in colliders)
        {
            CellType cell = collider.GetComponent<CellType>();
            if(cell != null)
            {
                cell.ActiveCell = false;
            }
        }
        
    }
    private void CheckToDestroyAndExplosionSome()
    {
         colliders = Physics2D.OverlapBoxAll(transform.position, VectorToCheckSomeItems, 0);
        foreach (Collider2D collider in colliders)
        {
            AbilityItem abilityItem = collider.GetComponent<AbilityItem>();
            if(abilityItem != null)
            {
                if(collider == gameObject.GetComponent<Collider2D>())
                {
                    Debug.Log("saaKak0");
                    return;
                }
                if(abilityItem.abilityType == abilityType)
                {
                    Debug.Log("saaKak1");
                    Explose();
                    Destroy(abilityItem.gameObject);
                    Destroy(gameObject);
                }
            }
        }

            
        
    }

    private void Explose()
    {

    }

    private void OnDestroy()
    {
        switch (abilityType)
        {
            case AbilityType.Aprodox:
                Debug.Log("saaKak");
                GameManager.Instance.OnSwitchTurn -= CheckAround;
                GameManager.Instance.OnSwitchTurn -= CheckToDestroyAndExplosionSome;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, sizeCheckAround);
    }
}

public enum AbilityType
{
    Aprodox
}
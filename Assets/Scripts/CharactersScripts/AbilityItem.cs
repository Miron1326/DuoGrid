using UnityEngine;

public class AbilityItem : MonoBehaviour
{
    public Collider2D[] colliders;
    public AbilityType abilityType;
    private Vector2 BasicSizeCheckAround = new Vector2(0.01f,0.01f);
    public Vector2 currentSizeCheckAround;
    private Vector2 VectorToCheckSomeItems = new Vector2(2, 2);
    public void Start()
    {
        switch (abilityType)
        {
            case AbilityType.Aprodox:
                currentSizeCheckAround = BasicSizeCheckAround;
                GameManager.Instance.OnSwitchTurn += CheckAround;
                GameManager.Instance.OnSwitchTurn += CheckToDestroyAndExplosionSome;
                break;
        }
    }

    public void Change(Vector2 MoggedTO)
    {
        BasicSizeCheckAround += MoggedTO;
    }

    private void CheckAround()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, currentSizeCheckAround, 0);
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
        Gizmos.DrawWireCube(transform.position, currentSizeCheckAround);
    }
}

public enum AbilityType
{
    Aprodox
}
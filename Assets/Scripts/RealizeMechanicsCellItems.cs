using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealizeMechanicsCellItems : MonoBehaviour
{
    public TypeOfBoom currentBoomType;
    public CellType currentCellType;
    public EffectType currentType;
    private float armLenght = 2;
    private float armWidth = 2;
    private Button GuavaBoomButton;
    private Button GuavaBoomButton2;

    void Start()
    {
        currentCellType = GetComponent<CellType>();
        currentType = currentCellType.currentType;
        GuavaBoomButton = GameObject.Find("GuavaCell").GetComponent<Button>();
        GuavaBoomButton.interactable = false;
        GuavaBoomButton2 = GameObject.Find("GuavaCell2").GetComponent<Button>();
        GuavaBoomButton2.interactable = false;
    }

    public void RealizeMechanic(CellType cellType)
    {
        switch (currentBoomType)
        {
            case TypeOfBoom.Standart:
                
                Vector2 center = transform.position;
                Collider2D[] vertical = Physics2D.OverlapBoxAll(center, new Vector2(armWidth, armLenght * 2), 0);
                Collider2D[] horizontal = Physics2D.OverlapBoxAll(center, new Vector2(armLenght * 2, armWidth), 0);

                HashSet<Collider2D> allHits = new HashSet<Collider2D>();
                foreach (Collider2D col in vertical)
                {
                    allHits.Add(col);
                }
                foreach (Collider2D col in horizontal)
                {
                    allHits.Add(col);
                }


                foreach (Collider2D hit in allHits)
                {
                    if (hit.gameObject != gameObject)
                    {
                        CellType typeSelectedCell = hit.GetComponent<CellType>();
                        if (typeSelectedCell != null)
                        {
                            if (GameManager.Instance.TypeCellCanToBoom.Contains(typeSelectedCell.currentType))
                            {
                                typeSelectedCell.ChangeType(EffectType.FireCell);
                                if(typeSelectedCell.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
                                {
                                    Destroy(rigidbody);
                                }
                                if (typeSelectedCell.TryGetComponent<Collider2D>(out Collider2D colider))
                                {
                                    colider.isTrigger = true;
                                }
                            }
                        }
                    }
                }
                currentCellType.ChangeType(EffectType.None);
                Destroy(this);
                GuavaBoomButton.interactable = true;
                GuavaBoomButton2.interactable = true;
                break;



            default: 
                break;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(armWidth, armLenght * 2, 0));
        Gizmos.DrawWireCube(transform.position, new Vector3(armLenght * 2, armWidth, 0));
    }

}

public enum TypeOfBoom
{ 
    None,
    Standart
}

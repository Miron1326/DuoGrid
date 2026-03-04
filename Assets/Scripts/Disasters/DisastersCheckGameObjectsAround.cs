using System.Collections.Generic;
using UnityEngine;

public class DisastersCheckGameObjectsAround : MonoBehaviour
{
    public float radiusCheck = 2.2f;
    private Color gizmoColor = Color.green;
    public CellType currentCell;
    public List<CellType> cellTypesInRadius = new List<CellType>();
    public int cellInRadius;

    private void Start()
    {
        currentCell = GetComponent<CellType>(); 
    }
    void Update()
    {
        CellType currentType = GetComponent<CellType>();
        if (currentType.currentType != EffectType.NoneWithNoneEffectedMushrooms)
        {
            Animator thisAnimator = GetComponent<Animator>();
            Destroy(thisAnimator);
            currentType.UpdateVisual();
            Destroy(GetComponent<DisastersCheckGameObjectsAround>());
        }
        DisastersManager disastersManager = GameObject.Find("GameManager").GetComponent<DisastersManager>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radiusCheck);
        foreach (Collider2D colider in colliders)
        {
            if(colider.gameObject != gameObject)
            {
                Debug.DrawLine(transform.position, colider.gameObject.transform.position, gizmoColor);
                CellType cellTypeCurrentCollider = colider.gameObject.GetComponent<CellType>();
                if (cellTypeCurrentCollider != null)
                {
                        if (cellTypeCurrentCollider.currentType == EffectType.NoneWithNoneEffectedMushrooms)
                        {
                            if (cellInRadius == 3)
                            {
                                cellTypeCurrentCollider.currentType = EffectType.None;
                            }

                            if (cellTypesInRadius.Contains(cellTypeCurrentCollider))
                            {

                            }
                            else
                            {
                                cellTypesInRadius.Add(cellTypeCurrentCollider);

                                if (cellTypesInRadius.Count > 2)
                                {
                                    disastersManager.DisastersHistory.Add(DisasterTipe.Mutation);
                                    disastersManager.cellsFindedForMushroomAppearence.AddRange(cellTypesInRadius);
                                    disastersManager.gameObjectAround = gameObject;
                                    disastersManager.thisType = DisasterTipe.Mutation;
                                    gizmoColor = Color.red;
                                    cellInRadius = 3;
                                DisastersCheckGameObjectsAround disastersCheckGameObjectsAround = this;
                                Destroy(disastersCheckGameObjectsAround);
                                }
                            }
                    }
                    
                    
                }
                
            }
        }
    }
}

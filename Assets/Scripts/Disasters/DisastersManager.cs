using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisastersManager : MonoBehaviour
{
    
    public bool CellCreatingInThisTurn;
    public List<EffectType> CellTypesCantChange = new List<EffectType>();
    public List<DisasterTipe> disastersTipes = new List<DisasterTipe>();
    public List<CellType> cellTypesAct = new List<CellType>();
    public DisasterTipe thisType;
    public List<DisasterTipe> DisastersHistory = new List<DisasterTipe>();
    public int MovementAppear;
    public List<CellType> allCellType;
    public List<CellType> cellsFindedForMushroomAppearence = new List<CellType>();
    public GameObject gameObjectAround;

    private void Start()
    {
        GameManager.Instance.OnSwitchTurn += MushroomAppearenceDisaster;
        MushroomAppearenceDisaster();
    }
    private void Update()
    {
        foreach(CellType cell in cellTypesAct)
        {
            cell.UpdateVisual();
        }

        DoActionToDisaster();

        
    }
    public void MushroomAppearenceDisaster() 
    {
        if (!DisastersHistory.Contains(thisType))
        {
            DisastersHistory.Add(thisType);
        }
        GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
        foreach (GameObject playerCell in player1Cells)
        {

            CellType cellComponent = playerCell.GetComponent<CellType>();
            if (!allCellType.Contains(cellComponent))
            {
                if (cellComponent != null && cellComponent.currentType != EffectType.Wall)
                {
                    allCellType.Add(cellComponent);
                }
            }

        }
        GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
        foreach (GameObject playerCell in player2Cells)
        {
            CellType cellComponent = playerCell.GetComponent<CellType>();
            if (!allCellType.Contains(cellComponent))
            {
                if (cellComponent != null && cellComponent.currentType != EffectType.Wall)
                {
                    allCellType.Add(cellComponent);
                }
            }
        }
        CellCreatingInThisTurn = true;
        CheckMushroomAppearence();
    }
    private void CheckMushroomAppearence() // создание грибов
    {
        if (MovementAppear == 0)
        {
            CellCreatingInThisTurn = false; return;
        }
        if(MovementAppear % 2 == 0 && CellCreatingInThisTurn)
        {
            int RandomGrid = Random.Range(1, 3);
            int RandomCellNumber = Random.Range(1, allCellType.Count);
            CellType selectedType = allCellType[RandomCellNumber];
            if (!CellTypesCantChange.Contains(selectedType.currentType))
            {
                selectedType.ChangeType(EffectType.NoneWithNoneEffectedMushrooms);
            }
            else
            {
                CheckMushroomAppearence();
                return;
            }

            cellTypesAct.Add(selectedType);
            MovementAppear = 0;
            GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
            foreach (GameObject playerCell in player1Cells)
            {
                CellType cellComponent = playerCell.GetComponent<CellType>();
                cellComponent.UpdateVisual();
                
            }
            GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
            foreach (GameObject playerCell in player2Cells)
            {
                CellType cellComponent = playerCell.GetComponent<CellType>();
                cellComponent.UpdateVisual();
            }
            CellCreatingInThisTurn = false;
        }
        else
        {
            CellCreatingInThisTurn = false;
        }
    }
    public void DoActionToDisaster()
    {
        switch (thisType)
        {
            case DisasterTipe.Mutation:

               // int indexFor = Random.Range(0, cellsFindedForMushroomAppearence.Count + 1);
               // CellType newMutationCell = cellsFindedForMushroomAppearence[indexFor];
                CellType cellAround = gameObjectAround.GetComponent<CellType>();
                if (gameObject.TryGetComponent<DisastersCheckGameObjectsAround>(out DisastersCheckGameObjectsAround disastersCheckGameObjectsAround))
                {
                    Destroy(disastersCheckGameObjectsAround);
                }
                cellAround.ChangeType(EffectType.Tentacle);
                Destroy(cellAround.GetComponent<DisastersCheckGameObjectsAround>());
                cellAround.UpdateVisual();
                cellsFindedForMushroomAppearence.Remove(cellAround);

                foreach (CellType othercell in cellsFindedForMushroomAppearence.ToList()) //СОЗДАЮ КОПИЮ
                {
                    othercell.ChangeType(EffectType.None);
                    Destroy(othercell.GetComponent<DisastersCheckGameObjectsAround>());
                    othercell.UpdateVisual();
                    cellsFindedForMushroomAppearence.Remove(othercell);
                }

                thisType = DisasterTipe.Standart;
                DisastersHistory.Add(thisType);
                break;
        }

    }

    public void DeleteAllEnemy()
    {
        foreach(CellType cellType in cellTypesAct)
        {
            CellEnemyType cellEnemyType = cellType.gameObject.GetComponent<CellEnemyType>();
            if (cellEnemyType != null)
            {
                cellEnemyType.DestroyEnemy();
            }
        }
        cellTypesAct.Clear();
        DisastersHistory.Clear();
    }
}

public enum DisasterTipe
{
    Standart,
    Mutation
}

using UnityEngine;

public class CellEffectFromSacrifice : MonoBehaviour
{
    public string Activator;
    public EffectType effectType;
    public EffectType effectTypeToChange_M1;
    public GameObject[] _gameObjectsInRadius;
    private int _radius;
    void Start()
    {
        switch (effectType)
        {
            case EffectType.Sacrifice:
                _radius = 3;
                effectTypeToChange_M1 = EffectType.None;
                break;
        }
    }

    public void AfterNeededAction(string activator)
    {
        switch(effectType)
        {
            case EffectType.Sacrifice:
                Activator = activator;
                ChangeObjectForClass_Many(CellClass.cactusLike);
                break;
        }
    }

    private void CheckObjectInRadius()
    {

    }

    private void ChangeObjectForClass_Many(CellClass changeable)
    {
        CellType[] cellTypes = FindObjectsByType<CellType>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CellType cellType in cellTypes)
        {
            if(cellType.currentClass == changeable)
            {
                ActivateForManeObjects(null , 1, cellType.gameObject);
            }
        }
    }

    private void ActivateForManeObjects(CellType cellType = null, int levelToChange = 0, GameObject gameObjectChange = null)
    {
        switch(effectType)
        {
            case EffectType.Sacrifice:
 

                if(gameObjectChange != null)
                {
                    CellEnemyType cellEnemyType = gameObjectChange.GetComponent<CellEnemyType>();
                    if(cellEnemyType != null)
                    {
                        cellEnemyType.ChangeLevel(levelToChange, Activator);
                    }

                }
                break;
        }
    }
}

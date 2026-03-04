using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFiller : MonoBehaviour
{
    [SerializeField] private int gridWight = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private int cellSize = 10;
    [SerializeField] private Vector2  gridOffset = Vector2.zero;

    private GameObject CellPrephab;
    public Sprite[] possibleSprites;

    private Transform _parent;

    private List<GameObject> spawnedCells = new List<GameObject>();

    void Start()
    {
        CellPrephab = GameObject.Find("CellPrephabForBackGround");
        _parent = GameObject.Find("ParentForBackGround").transform;
        possibleSprites = Resources.LoadAll<Sprite>("sprites/CellsBackGround");
        FillGrid();
    }
    public void FillGrid()
    {
        for (int x = 0; x < gridWight; x++)
        {
            for(int y = 0; y < gridHeight; y++)
            {
                Vector2 position = new Vector2(x * cellSize + gridOffset.x, y * cellSize + gridOffset.y);
                GameObject cell = Instantiate(CellPrephab, position, Quaternion.identity, _parent);
                SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
                if(spriteRenderer == null)
                {
                    cell.AddComponent<SpriteRenderer>();
                }
                int randomSprite = Random.Range(0, possibleSprites.Length);
                spriteRenderer.sprite = possibleSprites[randomSprite];
                spawnedCells.Add(cell);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public int gridWidth = 9;
    public int gridHeight = 5;

    [Header("Размер клетки")]
    public float cellVisualScale = 0.9f; 
    [Header("ПРОБЕЛ МЕЖДУ КЛЕТКАМИ")]
    public float spacing = 0.1f; 

    [Header("Промежуток между двумя полями")]
    public float spaceBetweenFields = 5f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Очищаем старые клетки
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Рассчитываем
        float cellSizeWithGap = cellVisualScale + spacing; // Расстояние между центрами
        float fieldWidth = gridWidth * cellSizeWithGap;

        // Центры полей
        float leftFieldCenterX = -(fieldWidth + spaceBetweenFields) / 2f;
        float rightFieldCenterX = (fieldWidth + spaceBetweenFields) / 2f;

        CreatePlayerField(leftFieldCenterX, 0, "Player1Cell");
        CreatePlayerField(rightFieldCenterX, 0, "Player2Cell");
    }

    void CreatePlayerField(float centerX, float centerY, string playerTag)
    {
        float cellSizeWithGap = cellVisualScale + spacing;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject cell = Instantiate(cellPrefab);

                // Позиция
                float posX = centerX + (x - gridWidth / 2f + 0.5f) * cellSizeWithGap;
                float posY = centerY + (y - gridHeight / 2f + 0.5f) * cellSizeWithGap;

                cell.transform.position = new Vector3(posX, posY, 0);
                cell.transform.localScale = new Vector3(cellVisualScale, cellVisualScale, 1f);
                cell.transform.SetParent(transform);
                cell.tag = playerTag;
            }
        }
    }
}

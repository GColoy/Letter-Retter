using UnityEngine;

/// <summary>
/// GridWorld - Generates a 2D tile grid at startup.
/// Attach this to an empty GameObject called "GridWorld".
/// </summary>
public class GridWorld : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 20;
    public int height = 15;
    public float tileSize = 1f;

    [Header("Tile Sprites")]
    public Sprite groundSprite;   // Assign a ground/grass sprite in the Inspector
    public Sprite borderSprite;   // Optional: assign a border/wall sprite

    [Header("Colors")]
    public Color tileColorA = new Color(0.45f, 0.76f, 0.40f); // grass green
    public Color tileColorB = new Color(0.40f, 0.70f, 0.35f); // slightly darker green

    // 2D array holding every tile GameObject
    private GameObject[,] tiles;

    void Start()
    {
        GenerateGrid();
        CenterCamera();
    }

    /// <summary>Builds the grid of tile GameObjects.</summary>
    public void GenerateGrid()
    {
        // Clean up any previously generated tiles
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        tiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isBorder = (x == 0 || x == width - 1 || y == 0 || y == height - 1);
                GameObject tile = CreateTile(x, y, isBorder);
                tiles[x, y] = tile;
            }
        }
    }

    /// <summary>Creates a single tile at grid position (x, y).</summary>
    private GameObject CreateTile(int x, int y, bool isBorder)
    {
        GameObject tile = new GameObject($"Tile_{x}_{y}");
        tile.transform.parent = transform;
        tile.transform.position = new Vector3(x * tileSize, y * tileSize, 0f);

        // Sprite Renderer
        SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 0;

        if (isBorder && borderSprite != null)
        {
            sr.sprite = borderSprite;
            sr.color = new Color(0.3f, 0.55f, 0.25f);
        }
        else
        {
            sr.sprite = groundSprite;
            // Checkerboard tint for visual variety
            sr.color = ((x + y) % 2 == 0) ? tileColorA : tileColorB;
        }

        // Box Collider so raycasts (for object placement) hit the grid
        BoxCollider2D col = tile.AddComponent<BoxCollider2D>();
        col.size = Vector2.one * tileSize;

        // Store grid coords for easy lookup
        TileData data = tile.AddComponent<TileData>();
        data.gridX = x;
        data.gridY = y;
        data.isOccupied = isBorder; // border tiles start as occupied

        return tile;
    }

    /// <summary>Returns world position of a grid cell's centre.</summary>
    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0f);
    }

    /// <summary>Returns the TileData at grid position (x,y), or null if out of bounds.</summary>
    public TileData GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return tiles[x, y]?.GetComponent<TileData>();
    }

    /// <summary>Moves the main camera so the grid is centred on screen.</summary>
    private void CenterCamera()
    {
        if (Camera.main == null) return;
        float cx = (width  - 1) * tileSize * 0.5f;
        float cy = (height - 1) * tileSize * 0.5f;
        Camera.main.transform.position = new Vector3(cx, cy, -10f);

        // Fit the grid height into the camera view with a small margin
        Camera.main.orthographicSize = (height * tileSize * 0.5f) + 1f;
    }
}

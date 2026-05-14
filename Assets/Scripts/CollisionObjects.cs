using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollisionObjects : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the GridWorld GameObject here.")]
    public GridWorld gridWorld;
    private Vector3 worldPos;
    private List<TileData> occupied = new List<TileData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        worldPos = transform.position;
        if (gridWorld == null)
        {
            Debug.LogError("GridMover: GridWorld reference is not assigned!", this);
            enabled = false;
            yield break;
        }
        yield return null;
        OccupyOverlapped();
    }
    void OnDestroy()
    {
        ReleaseAll();
    }

    public void OccupyOverlapped()
    {
        ReleaseAll();
        if (gridWorld == null) return;

        // Reuse the helper you already have (or copy its code inline)
        var overlapped = GetOverlappedTiles();
        foreach (var t in overlapped)
        {
            if (t == null) continue;
            // Use owner-aware Occupy if your TileData supports it:
            // t.Occupy(this);
            // Otherwise just:
            t.Occupy();
            occupied.Add(t);
        }
    }

    public void ReleaseAll()
    {
        foreach (var t in occupied)
        {
            if (t == null) continue;
            // t.Release(this);
            t.Release();
        }
        occupied.Clear();
    }
    
    // Returns list of TileData for every tile overlapped by the table's BoxCollider2D
    public List<TileData> GetOverlappedTiles()
    {
        var tiles = new List<TileData>();
        if (gridWorld == null) return tiles;

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return tiles;

        // World-space bounds of the collider
        Bounds b = box.bounds;
        float ts = gridWorld.tileSize;

        // Convert bounds to grid coordinates (inclusive)
        const float eps = 0.0001f;
        int minX = Mathf.FloorToInt((b.min.x + eps) / ts) +1;
        int minY = Mathf.FloorToInt((b.min.y + eps) / ts) +1;
        int maxX = Mathf.FloorToInt((b.max.x - eps) / ts); // subtract tiny epsilon to avoid touching next tile
        int maxY = Mathf.FloorToInt((b.max.y - eps) / ts);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                TileData t = gridWorld.GetTile(x, y);
                if (t != null) tiles.Add(t);
            }
        }

        return tiles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

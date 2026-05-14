using UnityEngine;

/// <summary>
/// TileData - Attached automatically to every tile by GridWorld.
/// Tracks whether this cell is occupied and what object sits on it.
/// </summary>
public class TileData : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public bool isOccupied = false;
    public GameObject placedObject = null;

    // Highlight colors
    private static readonly Color HoverColor    = new Color(1f, 1f, 0.4f, 0.6f);
    private static readonly Color OccupiedColor = new Color(1f, 0.3f, 0.3f, 0.6f);

    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    /// <summary>Call when the mouse enters this tile.</summary>
    public void OnHoverEnter()
    {
        sr.color = isOccupied ? OccupiedColor : HoverColor;
    }

    /// <summary>Call when the mouse leaves this tile.</summary>
    public void OnHoverExit()
    {
        sr.color = originalColor;
    }

    /// <summary>Marks this tile occupied by the supplied object.</summary>
    public void Place(GameObject obj)
    {
        placedObject = obj;
        isOccupied   = true;
        sr.color      = originalColor; // restore after placement
    }

    /// <summary>Clears the tile (removes the placed object and resets state).</summary>
    public void Clear()
    {
        if (placedObject != null)
            Destroy(placedObject);
        placedObject = null;
        isOccupied   = false;
        sr.color      = originalColor;
    }
}

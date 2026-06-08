using UnityEngine;

/// <summary>
/// TileData - Attached automatically to every tile by GridWorld.
/// Tracks whether a GridMover is occupying this cell.
/// </summary>
public class TileData : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public bool isOccupied = false;

    /// <summary>Marks this tile occupied by a GridMover.</summary>
    public void Occupy()
    {
        isOccupied = true;
    }

    /// <summary>Releases this tile when a GridMover moves away.</summary>
    public void Release()
    {
        isOccupied = false;
    }
}

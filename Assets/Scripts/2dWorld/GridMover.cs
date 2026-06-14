using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// GridMover - Moves a GameObject one tile at a time on the GridWorld.
/// Uses the new Unity Input System.
///
/// Controls:
///   W / Arrow Up    → move up
///   S / Arrow Down  → move down
///   A / Arrow Left  → move left
///   D / Arrow Right → move right
///
/// Setup:
///   1. Create a GameObject (e.g. a coloured square sprite), name it "Player".
///   2. Attach this script to it.
///   3. Assign the GridWorld reference in the Inspector.
///   4. Set Start Grid X / Y to the tile you want the player to begin on.
/// </summary>
public class GridMover : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Header("References")]
    [Tooltip("Drag the GridWorld GameObject here.")]
    public GridWorld gridWorld;

    [Header("Animation")]
    public Animator animator;

    [Header("Starting Position")]
    public int startGridX = 5;
    public int startGridY = 5;

    [Header("Movement")]
    [Tooltip("Seconds it takes to slide one tile.")]
    [Range(0.05f, 0.5f)]
    public float moveTime = 0.10f;

    [Tooltip("How long after landing before the next move is accepted (prevents double-steps).")]
    [Range(0f, 0.3f)]
    public float inputCooldown = 0.05f;

    [Header("Visual")]
    [Tooltip("Sorting order so the player renders above tiles.")]
    public int sortingOrder = 2;

    // ── State ─────────────────────────────────────────────────────────────────

    private int   currentX;
    private int   currentY;
    private bool  isMoving      = false;
    private float cooldownTimer = 0f;
    private bool canMove = true; 
    // Tracks the current held direction so held keys keep moving
    private Vector2 heldDirection = Vector2.zero;

    // ── Input Actions ─────────────────────────────────────────────────────────

    private InputAction moveAction;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        // Create a composite binding that reads WASD and Arrow Keys as a 2D vector
        moveAction = new InputAction("Move", binding: "<Keyboard>/w");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up",    "<Keyboard>/w")
            .With("Up",    "<Keyboard>/upArrow")
            .With("Down",  "<Keyboard>/s")
            .With("Down",  "<Keyboard>/downArrow")
            .With("Left",  "<Keyboard>/a")
            .With("Left",  "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        moveAction.Enable();
    }

    void OnDestroy()
    {
        moveAction.Disable();
        moveAction.Dispose();
    }

    void Start()
    {
        if (gridWorld == null)
        {
            Debug.LogError("GridMover: GridWorld reference is not assigned!", this);
            enabled = false;
            return;
        }

        // Ensure SpriteRenderer sorts above tiles
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = sortingOrder;

        // Snap to starting tile
        currentX = startGridX;
        currentY = startGridY;

        TileData startTile = gridWorld.GetTile(currentX, currentY);
        if (startTile == null || startTile.isOccupied)
        {
            Debug.LogWarning($"GridMover: Starting tile ({currentX},{currentY}) is invalid or occupied. Searching for a free tile…");
            FindAndSnapToFreeTile();
            return;
        }

        SnapToTile(currentX, currentY);
    }

    void Update()
    {
        if(!canMove) return; 

        // Tick cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        if (isMoving) return;

        // Read the current direction from held keys
        Vector2 dir = moveAction.ReadValue<Vector2>();

        if (dir == Vector2.zero) return;

        // Favour cardinal directions — pick whichever axis is stronger
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            TryMove(dir.x > 0 ? 1 : -1, 0);
        else
            TryMove(0, dir.y > 0 ? 1 : -1);
    }

    private void OnEnable()
    {
        Dialogue.OnDialogueStarted += DisableMovement;
        Dialogue.OnDialogueEnded += EnableMovement;
    }

    private void OnDisable()
    {
        Dialogue.OnDialogueStarted -= DisableMovement;
        Dialogue.OnDialogueEnded -= EnableMovement;
    }

    // ── Movement ──────────────────────────────────────────────────────────────

    /// <summary>Attempts to move by (dx, dy) grid steps.</summary>
    private void TryMove(int dx, int dy)
    {
        int targetX = currentX + dx;
        int targetY = currentY + dy;

        TileData targetTile = gridWorld.GetTile(targetX, targetY);

        // Blocked: out of bounds or occupied
        if (targetTile == null || targetTile.isOccupied)
        {
            return;
        }

        // Release current tile
        gridWorld.GetTile(currentX, currentY)?.Release();

        // Claim target tile
        targetTile.Occupy();

        // Update logical position
        currentX = targetX;
        currentY = targetY;

        animator?.SetFloat("MoveX", dx);
        animator?.SetFloat("MoveY", dy);
        animator?.SetFloat("Speed", 1f);

        // Animate
        StartCoroutine(SlideTo(gridWorld.GridToWorld(currentX, currentY)));

    }

    /// <summary>Smoothly slides the object to the target world position.</summary>
    private IEnumerator SlideTo(Vector3 target)
    {
        isMoving      = true;
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < moveTime)
        {
            elapsed           += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, elapsed / moveTime);
            yield return null;
        }

        transform.position = target; // snap exactly on arrival
        isMoving           = false;
        animator?.SetFloat("MoveX", 0f);
        animator?.SetFloat("MoveY", 0f);
        animator?.SetFloat("Speed", 0f); // idle
        cooldownTimer      = inputCooldown;
    }

    // ── Event-Handlers ──────────────────────────────────────────────────────────────
    private void DisableMovement()
    {
        canMove = false;
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Instantly places the object on tile (x, y) and marks it occupied.</summary>
    private void SnapToTile(int x, int y)
    {
        Vector3 pos        = gridWorld.GridToWorld(x, y);
        transform.position = new Vector3(pos.x, pos.y, 0f);
        gridWorld.GetTile(x, y)?.Occupy();
    }

    /// <summary>Fallback: scans the grid for the first free, non-border tile.</summary>
    private void FindAndSnapToFreeTile()
    {
        for (int x = 1; x < gridWorld.width  - 1; x++)
        for (int y = 1; y < gridWorld.height - 1; y++)
        {
            TileData t = gridWorld.GetTile(x, y);
            if (t != null && !t.isOccupied)
            {
                currentX = x;
                currentY = y;
                SnapToTile(x, y);
                return;
            }
        }
        Debug.LogError("GridMover: No free tile found in the entire grid!");
    }

    // ── Gizmos ────────────────────────────────────────────────────────────────

    void OnDrawGizmos()
    {
        if (gridWorld == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gridWorld.GridToWorld(currentX, currentY), 0.3f);
    }
}

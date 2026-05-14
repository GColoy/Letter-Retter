using UnityEngine;

/// <summary>
/// CameraController - Middle-mouse drag to pan; scroll wheel to zoom.
/// Attach this to the Main Camera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 5f;
    public bool  invertPan = false;

    [Header("Zoom")]
    public float zoomSpeed     = 2f;
    public float minZoom       = 3f;
    public float maxZoom       = 20f;

    private Camera cam;
    private Vector3 dragOrigin;
    private bool    isDragging;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        // Begin drag on middle-mouse press
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(2))
            isDragging = false;

        if (!isDragging) return;

        Vector3 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 delta       = dragOrigin - currentPos;
        if (invertPan) delta = -delta;

        transform.position += new Vector3(delta.x, delta.y, 0f);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f)) return;

        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize  = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}

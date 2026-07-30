using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;
    [SerializeField] Transform background1;
    [SerializeField] Transform background2;

    SpriteRenderer renderer1;
    SpriteRenderer renderer2;
    Camera mainCamera;
    bool initialized = false;

    void Start()
    {
        if (background1 == null || background2 == null)
        {
            Debug.LogError("InfiniteBackground: background1 and background2 references must be assigned!");
            enabled = false;
            return;
        }

        renderer1 = background1.GetComponent<SpriteRenderer>();
        renderer2 = background2.GetComponent<SpriteRenderer>();

        if (renderer1 == null || renderer2 == null)
        {
            Debug.LogError("InfiniteBackground: both background objects must have SpriteRenderer components!");
            enabled = false;
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("InfiniteBackground: no Camera.main found!");
            enabled = false;
            return;
        }

        // Initial placement: position background1 at world (0,0,0) to match the
        // original scene setup, then position background2 directly above it
        // using actual SpriteRenderer world-space bounds.
        float bg1HalfHeight = renderer1.bounds.extents.y;
        background1.position = new Vector3(background1.position.x, 0f, background1.position.z);

        // Recalculate bounds after repositioning
        // We need to force Unity to update bounds after moving the transform
        float bg2ExtentY = renderer2.bounds.extents.y;
        float bg1TopY = renderer1.bounds.max.y;
        float newBg2Y = bg1TopY + bg2ExtentY;

        background2.position = new Vector3(
            background1.position.x,
            newBg2Y,
            background1.position.z
        );

        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            return;

        // Move both backgrounds downward
        Vector3 movement = Vector3.down * scrollSpeed * Time.deltaTime;
        background1.position += movement;
        background2.position += movement;

        // Get the camera's bottom edge in world space
        float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;

        // Determine which background is above and which is below
        float maxY1 = renderer1.bounds.max.y;
        float maxY2 = renderer2.bounds.max.y;

        Transform upperBg, lowerBg;
        SpriteRenderer upperRenderer, lowerRenderer;

        if (maxY1 >= maxY2)
        {
            upperBg = background1;
            lowerBg = background2;
            upperRenderer = renderer1;
            lowerRenderer = renderer2;
        }
        else
        {
            upperBg = background2;
            lowerBg = background1;
            upperRenderer = renderer2;
            lowerRenderer = renderer1;
        }

        // When the lower background's top edge has scrolled below the camera's
        // bottom edge, reposition it directly above the upper background.
        if (lowerRenderer.bounds.max.y < cameraBottom)
        {
            float lowerExtentY = lowerRenderer.bounds.extents.y;
            float newY = upperRenderer.bounds.max.y + lowerExtentY;
            lowerBg.position = new Vector3(lowerBg.position.x, newY, lowerBg.position.z);
        }
    }
}
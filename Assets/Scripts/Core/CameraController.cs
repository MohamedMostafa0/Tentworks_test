using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Renderer groundRenderer;
    [SerializeField][Range(0f, 1f)] private float padding = 0.05f;

    private Camera mainCamera;
    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Start()
    {
        FitCameraToGround();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    private void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            FitCameraToGround();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }

    public void FitCameraToGround()
    {
        if (groundRenderer == null || mainCamera == null) return;

        Bounds bounds = groundRenderer.bounds;

        // Apply padding as a fraction of the plane's size
        float planeWidth = bounds.size.x * (1f + padding);
        float planeDepth = bounds.size.z * (1f + padding);
        Vector3 center = bounds.center;

        float aspectRatio = (float)Screen.width / Screen.height;
        float halfFovRad = mainCamera.fieldOfView * Mathf.Deg2Rad * 0.5f;

        // Distance needed so the vertical FOV covers the plane's depth
        float distForDepth = planeDepth * 0.5f / Mathf.Tan(halfFovRad);
        // Distance needed so the horizontal FOV (derived from vertical FOV + aspect) covers the plane's width
        float distForWidth = planeWidth * 0.5f / (aspectRatio * Mathf.Tan(halfFovRad));

        float distance = Mathf.Max(distForDepth, distForWidth);

        transform.position = new Vector3(center.x, center.y + distance, center.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}

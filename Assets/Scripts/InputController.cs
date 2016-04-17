using UnityEngine;

public class InputController : MonoBehaviour
{
    public Transform Player;
    [Range(0f, 1f)]
    public float RotationSpeed = 0.5f;
    [Range(0f, 1f)]
    public float ScaleSpeed = 0.2f;

    public void Start()
    {
        minDistance = Player.localScale.x;
        maxDistance = 8f;
        targetArea = Player.localScale.x * Player.localScale.y;

        targetRotation = Player.rotation;
        targetScale = Player.localScale;

        playerLayer = 1 << LayerMask.NameToLayer("Player");
        inputPlaneLayer = 1 << LayerMask.NameToLayer("InputPlane");
    }

    public void Update()
    {
        if (GameController.Instance.TimeScale == 0f)
        {
            dragging = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, playerLayer))
            {
                dragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, inputPlaneLayer))
        {
            // Rotation
            var direction = hit.point - transform.position;
            var angle = Vector3.Angle(Vector3.up, direction);
            targetRotation = Quaternion.Euler(0f, 0f, hit.point.x > 0 ? -angle : angle);

            // Scale
            var distance = Mathf.Clamp(Vector3.Distance(transform.position, hit.point) * 2, minDistance, maxDistance);
            targetScale = new Vector3(targetArea / distance, distance, Player.localScale.z);
        }
    }

    public void FixedUpdate()
    {
        Player.rotation = Quaternion.Lerp(Player.rotation, targetRotation, RotationSpeed);
        Player.localScale = Vector3.Lerp(Player.localScale, targetScale, ScaleSpeed);
    }

    private float minDistance;
    private float maxDistance;
    private float targetArea;   
    private int playerLayer;
    private int inputPlaneLayer;

    private RaycastHit hit;
    private bool dragging = false;
    private Quaternion targetRotation;
    private Vector3 targetScale;
}

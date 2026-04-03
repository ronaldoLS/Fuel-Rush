using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    [SerializeField] private float maxSteeringAngle = 45f;
    [SerializeField] private float steeringSpeed = 5f;

    private float currentRotation;
    private float sideBoundary;

    private void Start()
    {
        sideBoundary = GameManager.Instance.sideBoundary;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver) return;

        if (GameManager.Instance.IsOnBoundary) return;
        bool boundary = GameManager.Instance.IsOnBoundary;
        float input = boundary ? 0f : Input.GetAxis("Horizontal");

        float target = input * maxSteeringAngle;

        currentRotation = Mathf.Lerp(currentRotation, -target, steeringSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
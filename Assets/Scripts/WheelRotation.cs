using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 500f;

    void FixedUpdate()
    {
        float speed = GameManager.Instance.speed;

        transform.Rotate(Vector3.right * speed * rotationSpeed * Time.deltaTime);
    }
}
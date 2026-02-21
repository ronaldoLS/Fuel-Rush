using UnityEngine;
[DefaultExecutionOrder(2)]
public class RoadMovement : MonoBehaviour
{
    private float speed;
    private readonly float reposition = 17.5f;
    private readonly float startPos = 30f;

    private void Start()
    {
        speed = GameManager.Instance.speed;
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        if (transform.position.z < reposition)
        {
            transform.position = new(transform.position.x, transform.position.y, startPos);
        }
    }
}

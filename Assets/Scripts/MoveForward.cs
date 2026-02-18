using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5.0f;
    private float zBound = 4.0f;
    bool isStopped = false;

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
            transform.Translate(Vector3.forward * -speed * Time.deltaTime, Space.World);

        if (transform.position.z < -zBound)
        {
            Destroy(gameObject);
        }
    }

    public void StopMovement()
    {
        isStopped = true;
    }
}

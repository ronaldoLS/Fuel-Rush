using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float zBound = 4f;

    private float speed;
    private bool isStopped;

    private ObjectPool pool;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Reset movimento
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Reset rotação
        transform.rotation = Quaternion.identity;

        speed = GameManager.Instance.speed;
        isStopped = false;
    }

    public void SetPool(ObjectPool poolReference)
    {
        pool = poolReference;
    }

    private void Update()
    {
        if (isStopped) return;

        transform.Translate(speed * Time.deltaTime * Vector3.back, Space.World);

        if (transform.position.z < -zBound)
        {
            ReturnToPool();
        }
    }

    public void StopMovement()
    {
        isStopped = true;
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }
}
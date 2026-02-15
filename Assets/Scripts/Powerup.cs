using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float moveSpeed = 5f;         // velocidade de movimento
    private readonly float rotationSpeed = 120.0f; // Graus por segundo
    private readonly float amplitude = 0.5f; // Amplitude do movimento de flutuação
    private readonly float frequency = 3f;   // Frequência do movimento de flutuação


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    void Update()
    {
        AutoRotation();
        FloatEffect();

    }

    void AutoRotation()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }
    void FloatEffect()
    {
        
        Vector3 position = transform.position;
        position.y += Mathf.Sin(Time.time * frequency) * amplitude * Time.deltaTime;
        transform.position = position;

    }
    void moveBackward()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }
}

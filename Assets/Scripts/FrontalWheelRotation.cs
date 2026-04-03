using UnityEngine;

public class FrontalWheelRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float maxSteeringAngle = 45f;
    [SerializeField] private float steeringSpeed = 5f;

    private float currentSteeringAngle; // Rotação Y (Direção)
    private float currentRollingAngle;  // Rotação X (Aceleração)

    void FixedUpdate()
    {


        //Cálculo da rotação da roda (girando para frente)
        float speed = GameManager.Instance.speed;
        currentRollingAngle += speed * rotationSpeed * Time.deltaTime;

        //Cálculo do esterço (esquerda/direita)
        bool boundary = GameManager.Instance.IsOnBoundary;
        float input = boundary ? 0f : Input.GetAxis("Horizontal");
        float targetSteer = input * maxSteeringAngle;
        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, targetSteer, steeringSpeed * Time.deltaTime);

        // Aplicar as rotações combinadas
        transform.localRotation = Quaternion.Euler(currentRollingAngle, currentSteeringAngle, 0);
    }
}
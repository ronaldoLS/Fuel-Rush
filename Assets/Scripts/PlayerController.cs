using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(2)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float lowFuelThreshold = 0.15f;
    [SerializeField] private float stutterAmount = 0.15f;
    [SerializeField] private float stutterSpeed = 20f;

    private float stutterTimer;
    private float stutterOffsetZ;
    private float speed = 5.0f;
    private float sideBoundary = 2.5f;
    MoveForward moveForwardScript;

    // --- Novas Variáveis para Rotação ---
    private float rotationAngle = 20.0f; // Ângulo máximo de inclinação (em graus)
    private float rotationSpeed; // Velocidade da transição de rotação

    private float currentHorizontalInput = 0f;
    // ------------------------------------

    void Start()
    {


        // Define a velocidade de rotação com base na velocidade de movimento
        rotationSpeed = speed * 0.75f;

        

    }
    void Update()
    {
        ApplyLowFuelStutter();

    }
    void FixedUpdate()
    {

        // Move o jogador com base na entrada do usuário
        MovePlayer();
    

        // Aplica a rotação suave do carro com base na entrada do jogador
        RotateCar();

        // Aplica o stutter de baixa combustível ajustando a posição Z do carro
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            stutterOffsetZ
        );
        
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");

        float newX = transform.position.x + horizontal * speed * Time.deltaTime;

        // Clamp direto
        float clampedX = Mathf.Clamp(newX, -sideBoundary, sideBoundary);

        // Detecta se bateu no limite
        bool hitBoundary = Mathf.Abs(newX - clampedX) > 0.001f;

        // Se bateu no limite, zera input (para rotação)
        currentHorizontalInput = hitBoundary ? 0f : horizontal;

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void horizontalLimite()
    {
        // Clamp the player's position within the side boundaries
        // Uso de Mathf.Clamp para uma sintaxe mais concisa
        float clampedX = Mathf.Clamp(transform.position.x, -sideBoundary, sideBoundary);

        // Aplica a posição limitada apenas se ela mudou
        if (clampedX != transform.position.x)
        {
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Aplica uma rotação suave (inclinação) ao carro no eixo Z com base na entrada horizontal.
    /// </summary>
    void RotateCar()
    {
        // O ângulo alvo de rotação é o input horizontal (entre -1 e 1) 
        // multiplicado pelo ângulo máximo de inclinação.
        float targetYRotation = currentHorizontalInput * rotationAngle;


        // Cria o Quaternion (rotação) alvo
        Quaternion targetRotation = Quaternion.Euler(
            transform.localEulerAngles.x, // Mantém a rotação X (pitch)
            targetYRotation, // Mantém a rotação Y (yaw)
            transform.localEulerAngles.z// Define a nova rotação Z (roll)
        );


        // Aplica uma rotação suave (interpolação) em direção ao alvo usando Slerp
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
    void ApplyLowFuelStutter()
    {
        float fuelPercent = GameManager.Instance.FuelPercent;

        stutterOffsetZ = 0f;

        if (fuelPercent < lowFuelThreshold && fuelPercent > 0)
        {
            stutterTimer -= Time.deltaTime;

            if (stutterTimer <= 0)
            {
                float fuelFactor = 1f - GameManager.Instance.FuelPercent;
                stutterTimer = Random.Range(0.05f, 0.2f) * (1f - fuelFactor * 0.7f);

                float offset = Mathf.Sin(Time.time * stutterSpeed) * stutterAmount;
                stutterOffsetZ = -Mathf.Abs(offset);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        // Handle collisions with Cars and Barrels
        if (collision.gameObject.CompareTag("Car"))
        {
            AudioManager.Instance.PlayCrash();
            GameManager.Instance.GameOver();
            Debug.Log("Collided with a Car!");
        }
        if (collision.gameObject.CompareTag("Barrel"))
        {
            AudioManager.Instance.PlayCrash();
            GameManager.Instance.GameOver();
            Debug.Log("Collided with a Barrel!");

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            AudioManager.Instance.PlayFuelPickup();
            GameManager.Instance.IncreaseFuel(10);

            MoveForward move = other.GetComponent<MoveForward>();

            if (move != null)
                move.ReturnToPool();
        }

    }
}
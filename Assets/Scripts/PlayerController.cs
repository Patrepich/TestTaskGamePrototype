using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float startMoveSpeed = 5f;

    [SerializeField] Joystick joystick;

    [SerializeField] bool useGyro = false;
    [SerializeField] float gyroSensitivity = 2f;

    private float currentMoveSpeed;
    private float speedBoostTimer = 0f;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    private Rigidbody2D rb;

    void Start()
    {
        currentMoveSpeed = startMoveSpeed;

        rb = GetComponent<Rigidbody2D>();

        if (SystemInfo.supportsGyroscope && useGyro)
        {
            Input.gyro.enabled = true;
        }

    }

    void Update()
    {
        Vector2 input = GetMovementInput();

        rb.linearVelocity = input * currentMoveSpeed;

        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;

            if (speedBoostTimer <= 0)
            {
                currentMoveSpeed = startMoveSpeed;
            }
        }
    }

    private Vector2 GetMovementInput()
    {
        Vector2 keyboardInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        Vector2 gyroInput = Vector2.zero;
        if (useGyro && Input.gyro.enabled)
        {
            gyroInput = new Vector2(
                Input.gyro.rotationRateUnbiased.z * gyroSensitivity,
                -Input.gyro.rotationRateUnbiased.x * gyroSensitivity
            );
        }

        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        return (keyboardInput + gyroInput + joystickInput).normalized;
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"Очки: {score}";
    }

    public void ActivateSpeedBoost(float multiplayer, float duration)
    {
        currentMoveSpeed = startMoveSpeed * multiplayer;
        speedBoostTimer = duration;
    }
}
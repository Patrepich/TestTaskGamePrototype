using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float startMoveSpeed = 5f;
    [SerializeField] Joystick joystick;

    [SerializeField] float gyroSensitivity = 2f;

    [SerializeField] float accelerometerSensitivity = 0.5f;
    [SerializeField] float accelerometerDeadZone = 0.1f;

    [Header("Speed Boost")]
    private float currentMoveSpeed;
    private float speedBoostTimer = 0f;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    [Header("Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private Sprite leftSprite;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector3 calibratedOffset = Vector3.zero;

    void Start()
    {
        currentMoveSpeed = startMoveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        InitializeSensors();
    }

    void InitializeSensors()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }

        if (SystemInfo.supportsAccelerometer)
        {
            Input.compensateSensors = true;
            CalibrateAccelerometer();
        }
    }

    void Update()
    {
        Movement();
        SpriteDirection();
        SpeedBoost();
    }

    void Movement()
    {
        Vector2 input = GetMovementInput();
        rb.linearVelocity = input * currentMoveSpeed;
    }

    Vector2 GetMovementInput()
    {
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector2 gyroInput = GetGyroInput();
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        Vector2 accelerometerInput = GetAccelerometerInput();

        return (keyboardInput + gyroInput + joystickInput + accelerometerInput).normalized;
    }

    Vector2 GetGyroInput()
    {
        if (!Input.gyro.enabled) return Vector2.zero;

        return new Vector2(Input.gyro.rotationRateUnbiased.z * gyroSensitivity, -Input.gyro.rotationRateUnbiased.x * gyroSensitivity);
    }

    Vector2 GetAccelerometerInput()
    {
        if (!SystemInfo.supportsAccelerometer) return Vector2.zero;

        Vector3 rawAcceleration = Input.acceleration - calibratedOffset;

        float x = Mathf.Abs(rawAcceleration.x) > accelerometerDeadZone ? rawAcceleration.x * accelerometerSensitivity : 0;
        float y = Mathf.Abs(rawAcceleration.y) > accelerometerDeadZone ? rawAcceleration.y * accelerometerSensitivity : 0;

        return new Vector2(x, y);
    }

    void SpriteDirection()
    {
        Vector2 velocity = rb.linearVelocity;


        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            spriteRenderer.sprite = rightSprite;
            spriteRenderer.flipX = velocity.x < 0;
        }
        else
        {
            spriteRenderer.sprite = velocity.y > 0 ? upSprite : downSprite;
        }
    }

    void SpeedBoost()
    {
        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0)
            {
                currentMoveSpeed = startMoveSpeed;
            }
        }
    }

    public void CalibrateAccelerometer()
    {
        if (!SystemInfo.supportsAccelerometer) return;

        calibratedOffset = Input.acceleration;
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"Очки: {score}";
    }

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        currentMoveSpeed = startMoveSpeed * multiplier;
        speedBoostTimer = duration;
    }
}
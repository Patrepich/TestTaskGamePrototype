using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] Joystick joystick;

    [SerializeField] bool useGyro = false;
    [SerializeField] float gyroSensitivity = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (SystemInfo.supportsGyroscope && useGyro)
        {
            Input.gyro.enabled = true;
        }

    }

    void Update()
    {
        Vector2 input = GetMovementInput();

        rb.linearVelocity = input * moveSpeed;
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

        Vector2 joystickInput = new Vector2(
            joystick.Horizontal,
            joystick.Vertical
        );

        return (keyboardInput + gyroInput + joystickInput).normalized;
    }
}
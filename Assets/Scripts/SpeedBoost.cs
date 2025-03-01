using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private float speedMultiplayer = 1.5f;

    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ActivateSpeedBoost(speedMultiplayer, duration);
            Destroy(gameObject);
        }
    }
}

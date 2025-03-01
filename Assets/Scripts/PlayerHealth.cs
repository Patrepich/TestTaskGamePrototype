using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Image[] hearts;
    private int currentLives;

    [SerializeField] private float invincibilityTime = 2f;
    private float flashInterval = 0.1f;

    private float lastDamageTime = -999f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLives = maxLives;
        UpdateHeartsUI();
    }

    void Update()
    {
        if (Time.time < lastDamageTime + invincibilityTime)
        {
            float timer = Time.time - lastDamageTime;
            spriteRenderer.enabled = Mathf.FloorToInt(timer / flashInterval) % 2 == 0;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime + invincibilityTime)
        {
            return;
        }

        currentLives -= damage;
        lastDamageTime = Time.time;

        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < currentLives) ?
                Resources.Load<Sprite>("Sprites/Heart_Full") :
                Resources.Load<Sprite>("Sprites/Heart_Empty");
        }
    }

    public bool CanHeal()
    {
        return currentLives < maxLives;
    }

    public void Heal()
    {
        currentLives++;
        UpdateHeartsUI();
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;

    [SerializeField] private Image[] hearts;

    private int currentLives;

    void Start()
    {
        currentLives = maxLives;
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;

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
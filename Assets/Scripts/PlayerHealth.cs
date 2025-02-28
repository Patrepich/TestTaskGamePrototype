using UnityEngine;
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

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < currentLives) ?
                Resources.Load<Sprite>("Sprites/Heart_Full") :
                Resources.Load<Sprite>("Sprites/Heart_Empty");
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // You'll need to link these in the Unity Inspector.
    public Slider healthBar;
    public GameObject hintPanel;
    public Text hintText;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    public void ShowHintMessage(string message)
    {
        if (hintPanel != null && hintText != null)
        {
            hintText.text = message;
            hintPanel.SetActive(true);
            // You can add a timer here to hide the panel automatically.
        }
    }

    public void HideHintMessage()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }
}
using System.Collections;
using UnityEngine;

// Points de vie du dino : il perd une vie en touchant un obstacle,
// clignote un moment (invincible), puis c'est GAME OVER à 0.
// À placer sur le Dino (tag « Player »). Les obstacles ont le tag « Obstacle » et Is Trigger coché.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;             // nombre de vies au départ
    public float invincibleTime = 1.5f;   // durée du clignotement après un choc (secondes)
    public GameObject gameOverPanel;      // le panneau GAME OVER (désactivé au départ)
    public HealthUI healthUI;             // l'affichage des cœurs (chapitre 5)
    public AudioClip hurtSound;           // son du choc (optionnel)

    private int currentHealth;
    private bool isInvincible = false;
    private SpriteRenderer sprite;

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        RefreshUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        // Pendant l'invincibilité (ou si déjà mort), on ignore le choc
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log("Aïe ! Vies restantes : " + currentHealth);
        RefreshUI();

        if (hurtSound != null)
        {
            AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    // Appelée par un cœur ramassé (chapitre 5)
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (healthUI != null)
        {
            healthUI.Refresh(currentHealth, maxHealth);
        }
    }

    // Une coroutine : une fonction qui peut faire des pauses (yield return)
    private IEnumerator Invincibility()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibleTime)
        {
            sprite.enabled = !sprite.enabled;           // le dino clignote
            yield return new WaitForSeconds(0.1f);      // pause de 0,1 seconde
            elapsed += 0.1f;
        }

        sprite.enabled = true;
        isInvincible = false;
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SaveHighScore();     // garder le record
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;                            // figer le jeu
    }
}
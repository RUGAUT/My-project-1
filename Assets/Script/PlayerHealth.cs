using System.Collections;
using UnityEngine;

// Points de vie du dino : il perd une vie en touchant un obstacle (Trigger),
// clignote un moment (invincible), puis c'est GAME OVER à 0.
// Remplace DinoCollision. À placer sur le Dino.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;             // nombre de vies au départ
    public float invincibleTime = 1.5f;   // durée du clignotement après un choc
    public GameObject gameOverPanel;      // le texte ou panneau GAME OVER (désactivé au départ)

    private int currentHealth;
    private bool isInvincible = false;
    private SpriteRenderer sprite;

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Les obstacles sont des Triggers : le dino les traverse, mais Unity nous prévient
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log("Aïe ! Vies restantes : " + currentHealth);

        if (currentHealth <= 0)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            StartCoroutine(Invincibility());
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
}
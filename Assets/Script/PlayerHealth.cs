using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vie")]
    [SerializeField] private int viesMax = 3;

    [Header("Invincibilité après un coup")]
    [SerializeField] private float dureeInvincible = 1.5f;

    private int vies;
    private bool invincible;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        vies = viesMax;
    }

    private void Start()
    {
        HeartsUI.instance.MettreAJour(vies);
    }

    // Renvoie true si le joueur est toujours vivant, false s'il est mort
    public bool PrendreDegat(int degat, bool ignorerInvincibilite = false)
    {
        if (invincible && !ignorerInvincibilite) return true;

        vies -= degat;
        if (vies < 0) vies = 0;
        HeartsUI.instance.MettreAJour(vies);

        if (vies <= 0)
        {
            Mourir();
            return false;
        }

        if (!invincible) StartCoroutine(Invincibilite());
        return true;
    }

    private IEnumerator Invincibilite()
    {
        invincible = true;
        float fin = Time.time + dureeInvincible;

        // Le sprite clignote pendant l'invincibilité
        while (Time.time < fin)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.enabled = true;
        invincible = false;
    }

    private void Mourir()
    {
        GameOverPanel.instance.Afficher();
    }
}
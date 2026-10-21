using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadZone : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform pointRespawn;   // où le joueur réapparaît (optionnel)

    [Header("Dégâts")]
    [SerializeField] private int degat = 1;

    private Vector3 positionDepart;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        // Si aucun point de respawn n'est assigné, on utilise la position de départ du joueur
        GameObject joueur = GameObject.FindGameObjectWithTag("Player");
        if (joueur != null) positionDepart = joueur.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        bool toujoursVivant = true;

        if (other.TryGetComponent(out PlayerHealth vie))
        {
            // true = ignore l'invincibilité (tomber dans le vide fait toujours perdre une vie)
            toujoursVivant = vie.PrendreDegat(degat, true);
        }

        if (!toujoursVivant) return;   // le joueur est mort, la scène va se recharger

        // Remet le joueur sur le sol
        other.transform.position = pointRespawn != null ? pointRespawn.position : positionDepart;

        if (other.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;   // Unity 2022 ou plus ancien : rb.velocity
        }
    }

    // Affiche la zone en rouge dans la vue Scene
    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
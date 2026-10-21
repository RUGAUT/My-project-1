using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int degat = 1;

    // Si le collider de l'ennemi n'est PAS en trigger
    private void OnCollisionEnter2D(Collision2D collision) { Toucher(collision.gameObject); }
    private void OnCollisionStay2D(Collision2D collision) { Toucher(collision.gameObject); }

    // Si le collider de l'ennemi est en trigger
    private void OnTriggerEnter2D(Collider2D other) { Toucher(other.gameObject); }
    private void OnTriggerStay2D(Collider2D other) { Toucher(other.gameObject); }

    private void Toucher(GameObject objet)
    {
        if (!objet.CompareTag("Player")) return;

        if (objet.TryGetComponent(out PlayerHealth vie))
        {
            vie.PrendreDegat(degat);
        }
    }
}
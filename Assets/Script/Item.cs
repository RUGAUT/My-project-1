using UnityEngine;

// Première version d'un objet à ramasser : on compte les points dans la Console.
// Sur le Prefab de la pièce : Collider 2D avec Is Trigger coché + ObstacleMover + ce script.
public class Item : MonoBehaviour
{
    // Valeur de cet objet (modifiable dans l'Inspector)
    public int pointValue = 10;

    // « static » : une seule variable partagée par TOUTES les pièces
    public static int score = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Seul le dino (tag « Player ») ramasse l'objet
        if (other.CompareTag("Player"))
        {
            score += pointValue;
            Debug.Log("Objet ramassé ! Score total : " + score);
            Destroy(gameObject);
        }
    }
}
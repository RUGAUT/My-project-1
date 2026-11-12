using UnityEngine;

// Les types d'objets à ramasser
public enum ItemType
{
    Coin,    // pièce : des points
    Gem,     // gemme : beaucoup de points
    Heart    // cœur : +1 vie
}

// Objet à ramasser. Sur le Prefab : SpriteRenderer + Circle Collider 2D (Is Trigger coché)
// + ObstacleMover (pour qu'il défile) + ce script.
public class Collectible : MonoBehaviour
{
    public ItemType type = ItemType.Coin;
    public int points = 10;             // points donnés par une pièce ou une gemme
    public AudioClip pickupSound;       // son de collecte (optionnel)
    public GameObject pickupEffect;     // effet visuel à faire apparaître (optionnel)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Seul le joueur peut ramasser l'objet
        if (!other.CompareTag("Player")) return;

        switch (type)
        {
            case ItemType.Coin:
            case ItemType.Gem:
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.AddScore(points);
                }
                break;

            case ItemType.Heart:
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.Heal(1);
                }
                break;
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("Ramassé : " + type);
        Destroy(gameObject);
    }
}
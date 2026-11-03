using UnityEngine;

// Fait avancer un objet vers la gauche (cactus, oiseau, pièce…),
// puis le détruit quand il sort de l'écran.
// À placer sur chaque Prefab qui doit défiler.
public class ObstacleMover : MonoBehaviour
{
    public float speed = 6f;        // vitesse de défilement (unités par seconde)
    public float destroyX = -12f;   // position X où l'objet est détruit

    void Update()
    {
        // Vector2.left = (-1, 0) : on recule un peu à chaque image
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Sorti de l'écran à gauche ? On le supprime
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
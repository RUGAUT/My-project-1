using UnityEngine;

// Déplace le dino vers la gauche ou la droite avec les flèches (ou A / D).
// À placer sur le Dino, qui a un Rigidbody2D et un collider.
public class DinoController : MonoBehaviour
{
    public float speed = 5f;     // vitesse de déplacement
    public float minX = -7f;     // limite gauche
    public float maxX = -2f;     // limite droite : le dino reste à gauche de l'écran

    private Rigidbody2D rb;

    void Start()
    {
        // On récupère le Rigidbody2D du dino une seule fois
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = 0f;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveX = 1f;     // vers la droite
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveX = -1f;    // vers la gauche
        }

        // Arrivé à une limite, on n'avance plus dans cette direction
        if (transform.position.x <= minX && moveX < 0f) moveX = 0f;
        if (transform.position.x >= maxX && moveX > 0f) moveX = 0f;

        // On change la vitesse horizontale et on garde la vitesse verticale (gravité, saut)
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }
}
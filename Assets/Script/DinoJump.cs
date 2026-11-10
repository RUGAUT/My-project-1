using UnityEngine;

// Fait sauter le dino avec Espace (ou la flèche du haut), seulement quand il touche le sol.
// À placer sur le Dino. Le sol doit avoir le tag « Ground ».
public class DinoJump : MonoBehaviour
{
    public float jumpForce = 9f;          // puissance du saut
    public float fallMultiplier = 2.5f;   // gravité renforcée en descente : saut plus nerveux
    public AudioClip jumpSound;           // son du saut (optionnel)

    private Rigidbody2D rb;
    private bool isGrounded = false;

    // Les autres scripts (animation…) peuvent lire cette valeur sans la modifier
    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpPressed && isGrounded)
        {
            // On annule la vitesse verticale puis on donne une impulsion vers le haut
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            if (jumpSound != null)
            {
                AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position);
            }
        }
    }

    void FixedUpdate()
    {
        // En descente, on ajoute de la gravité : le dino retombe plus vite
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * rb.gravityScale * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
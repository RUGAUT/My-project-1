using UnityEngine;
using UnityEngine.InputSystem;

// Déplacement gauche/droite + saut avec ground check (nouveau Input System).
// Touches : flèches ou Q/D (AZERTY) pour bouger, Espace ou flèche haut pour sauter.
// Manette : stick gauche pour bouger, bouton A (Xbox) / Croix (PlayStation) pour sauter.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float vitesse = 5f;

    [Header("Saut")]
    [SerializeField] private float forceSaut = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;          // objet vide placé sous les pieds
    [SerializeField] private float rayonGroundCheck = 0.2f;
    [SerializeField] private LayerMask coucheSol;            // la couche "Ground" de ton sol

    [Header("Options")]
    [SerializeField] private bool retournerSprite = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float entreeHorizontale;
    private bool demandeSaut;
    private bool estAuSol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        entreeHorizontale = 0f;

        Keyboard clavier = Keyboard.current;
        Gamepad manette = Gamepad.current;

        // --- Déplacement ---
        if (clavier != null)
        {
            // Key.A = touche physique en haut à gauche, donc Q sur un clavier AZERTY
            if (clavier.leftArrowKey.isPressed || clavier.aKey.isPressed) entreeHorizontale -= 1f;
            if (clavier.rightArrowKey.isPressed || clavier.dKey.isPressed) entreeHorizontale += 1f;
        }

        if (manette != null && entreeHorizontale == 0f)
        {
            float stick = manette.leftStick.x.ReadValue();
            if (Mathf.Abs(stick) > 0.2f) entreeHorizontale = Mathf.Sign(stick);
        }

        // --- Saut (lu dans Update pour ne rater aucun appui) ---
        bool appuiSaut =
            (clavier != null && (clavier.spaceKey.wasPressedThisFrame || clavier.upArrowKey.wasPressedThisFrame)) ||
            (manette != null && manette.buttonSouth.wasPressedThisFrame);

        if (appuiSaut && estAuSol)
        {
            demandeSaut = true;
        }

        // --- Orientation du sprite ---
        if (retournerSprite && spriteRenderer != null && entreeHorizontale != 0f)
        {
            spriteRenderer.flipX = entreeHorizontale < 0f;
        }
    }

    private void FixedUpdate()
    {
        // Vérifie si un cercle sous les pieds touche la couche du sol
        estAuSol = groundCheck != null &&
                   Physics2D.OverlapCircle(groundCheck.position, rayonGroundCheck, coucheSol);

        // Unity 6 : linearVelocity. Pour Unity 2022 ou plus ancien, remplacer par rb.velocity
        Vector2 v = rb.linearVelocity;
        v.x = entreeHorizontale * vitesse;

        if (demandeSaut)
        {
            v.y = forceSaut;
            demandeSaut = false;
        }

        rb.linearVelocity = v;
    }

    // Affiche le cercle du ground check dans la vue Scene (vert = au sol, rouge = en l'air)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = estAuSol ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, rayonGroundCheck);
    }
}
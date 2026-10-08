using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float vitesse = 5f;

    [Header("Saut")]
    [SerializeField] private float forceSaut = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float rayonGroundCheck = 0.2f;
    [SerializeField] private LayerMask coucheSol;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float entreeHorizontale;
    private bool demandeSaut;
    private bool estAuSol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        entreeHorizontale = 0f;

        Keyboard clavier = Keyboard.current;
        Gamepad manette = Gamepad.current;

        if (clavier != null)
        {
            if (clavier.leftArrowKey.isPressed || clavier.aKey.isPressed) entreeHorizontale -= 1f;
            if (clavier.rightArrowKey.isPressed || clavier.dKey.isPressed) entreeHorizontale += 1f;
        }

        if (manette != null && entreeHorizontale == 0f)
        {
            float stick = manette.leftStick.x.ReadValue();
            if (Mathf.Abs(stick) > 0.2f) entreeHorizontale = Mathf.Sign(stick);
        }

        bool appuiSaut =
            (clavier != null && (clavier.spaceKey.wasPressedThisFrame || clavier.upArrowKey.wasPressedThisFrame)) ||
            (manette != null && manette.buttonSouth.wasPressedThisFrame);

        if (appuiSaut && estAuSol)
        {
            demandeSaut = true;
        }

        // Flip du sprite
        if (entreeHorizontale != 0f)
        {
            spriteRenderer.flipX = entreeHorizontale < 0f;
        }

        // Animations
        animator.SetFloat("speed", Mathf.Abs(entreeHorizontale));
        animator.SetBool("isJumping", !estAuSol);
    }

    private void FixedUpdate()
    {
        estAuSol = Physics2D.OverlapCircle(groundCheck.position, rayonGroundCheck, coucheSol);

        Vector2 v = rb.linearVelocity;
        v.x = entreeHorizontale * vitesse;

        if (demandeSaut)
        {
            v.y = forceSaut;
            demandeSaut = false;
        }

        rb.linearVelocity = v;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = estAuSol ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, rayonGroundCheck);
    }
}
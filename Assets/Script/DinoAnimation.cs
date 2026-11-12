using UnityEngine;

// Relie l'Animator au jeu : animation de course au sol, pose de saut en l'air.
// Dans l'Animator, créez un paramètre Bool nommé « AuSol ».
// À placer sur le Dino (avec Animator et DinoJump).
public class DinoAnimation : MonoBehaviour
{
    private Animator animator;
    private DinoJump jump;

    void Start()
    {
        animator = GetComponent<Animator>();
        jump = GetComponent<DinoJump>();
    }

    void Update()
    {
        animator.SetBool("AuSol", jump.IsGrounded);
    }
}
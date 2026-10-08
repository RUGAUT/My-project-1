using UnityEngine;

// Ennemi qui poursuit le Player.
// - Même niveau : il marche vers le Player sans tomber dans le vide.
// - Player plus bas : il cherche le bord le plus proche où un saut l'amène sur un terrain plus bas,
//   marche jusque-là et saute (le terrain du haut est entouré de vide, il faut sauter d'une plateforme à l'autre).
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform cible;                   // le Player (trouvé automatiquement si vide)

    [Header("Déplacement")]
    [SerializeField] private float vitesse = 2.5f;
    [SerializeField] private float distanceArret = 0.8f;        // s'arrête quand il est assez près
    [SerializeField] private float hauteurPourDescendre = 1.2f; // cible plus basse que ça = on cherche à descendre

    [Header("Saut")]
    [SerializeField] private float forceSaut = 9f;
    [SerializeField] private float vitesseEnSaut = 3.5f;        // vitesse horizontale pendant le saut
    [SerializeField] private float distanceAtterrissage = 3f;   // où on cherche du sol après le bord
    [SerializeField] private float porteeRechercheBord = 20f;

    [Header("Détection du sol")]
    [SerializeField] private LayerMask coucheSol;
    [SerializeField] private float distanceVerifSol = 1.2f;     // rayon vers le bas depuis les pieds

    [Header("Options")]
    [SerializeField] private bool retournerSprite = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private float directionBord;     // direction choisie pour descendre (-1 ou +1), 0 = aucune
    private bool enSaut;
    private float directionSaut;
    private float tempsSaut;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;

        if (cible == null)
        {
            GameObject joueur = GameObject.Find("Player");
            if (joueur != null) cible = joueur.transform;
        }
    }

    private void FixedUpdate()
    {
        if (cible == null) return;

        Vector2 v = rb.linearVelocity;
        bool auSol = SolSousPoint(PiedsX(0f));

        // --- En plein saut : on garde la direction jusqu'à l'atterrissage ---
        if (enSaut)
        {
            if (auSol && v.y <= 0.1f && Time.time - tempsSaut > 0.3f)
            {
                enSaut = false;
                directionBord = 0f;
            }
            else
            {
                v.x = directionSaut * vitesseEnSaut;
                rb.linearVelocity = v;
                Orienter(directionSaut);
                return;
            }
        }

        float dx = cible.position.x - transform.position.x;
        float dy = cible.position.y - transform.position.y;
        float direction = 0f;

        if (auSol)
        {
            bool cibleEnDessous = dy < -hauteurPourDescendre;

            if (cibleEnDessous)
            {
                if (directionBord == 0f) directionBord = ChoisirBordPourDescendre();

                if (directionBord != 0f)
                {
                    // Au bord ? alors on saute, sinon on marche vers le bord
                    if (!SolSousPoint(PiedsX(directionBord * 0.8f)))
                    {
                        Sauter(directionBord);
                        return;
                    }
                    direction = directionBord;
                }
                else
                {
                    direction = MarcheSansTomber(dx);
                }
            }
            else
            {
                directionBord = 0f;
                direction = MarcheSansTomber(dx);
            }
        }

        v.x = direction * vitesse;
        rb.linearVelocity = v;
        Orienter(direction);
    }

    private float MarcheSansTomber(float dx)
    {
        if (Mathf.Abs(dx) <= distanceArret) return 0f;
        float dir = Mathf.Sign(dx);
        return SolSousPoint(PiedsX(dir * 0.8f)) ? dir : 0f;
    }

    private void Sauter(float dir)
    {
        enSaut = true;
        directionSaut = dir;
        tempsSaut = Time.time;
        rb.linearVelocity = new Vector2(dir * vitesseEnSaut, forceSaut);
        Orienter(dir);
    }

    private void Orienter(float direction)
    {
        if (retournerSprite && spriteRenderer != null && direction != 0f)
            spriteRenderer.flipX = direction < 0f;
    }

    private Vector2 PiedsX(float decalageX)
    {
        Bounds b = col.bounds;
        return new Vector2(b.center.x + decalageX, b.min.y + 0.05f);
    }

    private bool SolSousPoint(Vector2 depart)
    {
        return Physics2D.Raycast(depart, Vector2.down, distanceVerifSol, coucheSol).collider != null;
    }

    // Renvoie la direction (-1 / +1) du bord le plus proche après lequel il y a un terrain plus bas, 0 si aucun
    private float ChoisirBordPourDescendre()
    {
        float gauche = DistanceBordValide(-1f);
        float droite = DistanceBordValide(1f);
        if (gauche == float.MaxValue && droite == float.MaxValue) return 0f;
        return droite <= gauche ? 1f : -1f;
    }

    private float DistanceBordValide(float sens)
    {
        Vector2 pieds = PiedsX(0f);
        for (float d = 0.2f; d < porteeRechercheBord; d += 0.2f)
        {
            if (!SolSousPoint(PiedsX(sens * d)))
            {
                // Bord trouvé : y a-t-il un terrain plus bas à portée de saut après ce bord ?
                float x = pieds.x + sens * (d + distanceAtterrissage);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, pieds.y - 0.3f), Vector2.down, 30f, coucheSol);
                return hit.collider != null ? d : float.MaxValue;
            }
        }
        return float.MaxValue;
    }
}

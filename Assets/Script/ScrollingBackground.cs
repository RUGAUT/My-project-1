using UnityEngine;

// Fait défiler un décor en boucle (sol, nuages, montagnes…).
// Mettez DEUX copies de l'image côte à côte, enfants d'un objet vide, et placez ce script sur cet objet.
// Plus la vitesse est faible, plus le décor paraît lointain : c'est l'effet de parallaxe.
public class ScrollingBackground : MonoBehaviour
{
    public float speed = 2f;     // vitesse de défilement
    public float width = 16f;    // largeur d'UNE image en unités (pixels ÷ PPU : 256 ÷ 16 = 16)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Mathf.Repeat garde la valeur entre 0 et width : le décor reboucle sans à-coup
        float offset = Mathf.Repeat(Time.time * speed, width);
        transform.position = startPosition + Vector3.left * offset;
    }
}
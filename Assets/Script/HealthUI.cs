using UnityEngine;
using UnityEngine.UI;

// Affiche les vies sous forme de cœurs (Images de l'UI) : pleins ou vides.
// À placer sur l'objet qui contient les cœurs, dans le Canvas.
public class HealthUI : MonoBehaviour
{
    public Image[] hearts;       // les Images des cœurs, de gauche à droite
    public Sprite fullHeart;     // sprite du cœur plein (UI/Coeurs_vie_0)
    public Sprite emptyHeart;    // sprite du cœur vide  (UI/Coeurs_vie_3)

    // Appelée par PlayerHealth à chaque changement de vie
    public void Refresh(int current, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < max;                                // autant de cœurs que de vies max
            hearts[i].sprite = i < current ? fullHeart : emptyHeart;    // plein ou vide
        }
    }
}
using UnityEngine;

// Écran titre : le jeu reste en pause tant qu'on n'a pas cliqué sur « Jouer ».
// À placer sur l'objet « GameManager ».
public class MenuJeu : MonoBehaviour
{
    public GameObject menuDemarrage;   // le panneau avec le titre et le bouton « Jouer »

    void Start()
    {
        Time.timeScale = 0f;              // pause pendant le menu
        menuDemarrage.SetActive(true);
    }

    // Appelée par le bouton « Jouer » (On Click)
    public void LancerJeu()
    {
        menuDemarrage.SetActive(false);
        Time.timeScale = 1f;              // c'est parti !
    }
}
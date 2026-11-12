using UnityEngine;
using UnityEngine.SceneManagement;

// Boutons de l'écran de fin. À placer sur l'objet « GameManager ».
// Dans l'Inspector du bouton « Rejouer » : On Click () > GameManager > GameOverManager.Rejouer
public class GameOverManager : MonoBehaviour
{
    public void Rejouer()
    {
        Time.timeScale = 1f;   // remettre le temps en marche !
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quitter()
    {
        Debug.Log("Quitter le jeu (ne fonctionne que dans le jeu exporté)");
        Application.Quit();
    }
}
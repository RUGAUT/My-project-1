using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    public static GameOverPanel instance;

    [Header("Panel Game Over")]
    [SerializeField] private GameObject panel;              // le panel (désactivé au lancement par le script)
    [SerializeField] private Button boutonRestart;
    [SerializeField] private TextMeshProUGUI texteTemps;    // texte dans le panel : temps de survie

    [Header("Timer en jeu (optionnel)")]
    [SerializeField] private TextMeshProUGUI texteTimerEnJeu;   // texte affiché pendant la partie, hors du panel

    private float temps;
    private bool partieTerminee;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;   // sécurité après un restart

        if (panel != null) panel.SetActive(false);
        if (boutonRestart != null) boutonRestart.onClick.AddListener(Recommencer);
    }

    private void Update()
    {
        if (partieTerminee) return;

        temps += Time.deltaTime;

        if (texteTimerEnJeu != null)
            texteTimerEnJeu.text = FormaterTemps(temps);
    }

    // Appelé par PlayerHealth quand le joueur meurt
    public void Afficher()
    {
        if (partieTerminee) return;
        partieTerminee = true;

        if (texteTemps != null)
            texteTemps.text = "Temps : " + FormaterTemps(temps);

        panel.SetActive(true);
        Time.timeScale = 0f;   // met le jeu en pause
    }

    public void Recommencer()
    {
        Time.timeScale = 1f;   // IMPORTANT : sinon le jeu reste figé après le reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private string FormaterTemps(float secondes)
    {
        int minutes = Mathf.FloorToInt(secondes / 60f);
        int sec = Mathf.FloorToInt(secondes % 60f);
        return minutes.ToString("00") + ":" + sec.ToString("00");
    }

    private void OnDestroy()
    {
        if (boutonRestart != null) boutonRestart.onClick.RemoveListener(Recommencer);
    }
}
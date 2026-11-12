using UnityEngine;
using TMPro;

// Gère le score : des points gagnés en courant (comme le dino de Google)
// + les points des objets ramassés. Garde aussi le record (PlayerPrefs).
// À placer sur un objet vide « GameManager ». Un seul ScoreManager par scène.
public class ScoreManager : MonoBehaviour
{
    // Accès facile depuis n'importe quel script : ScoreManager.instance
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;        // texte « SCORE 00042 »
    public TextMeshProUGUI highScoreText;    // texte « RECORD 00120 » (optionnel)
    public float pointsPerSecond = 10f;      // points gagnés juste en survivant

    private float score = 0f;
    private const string HIGH_SCORE_KEY = "HighScore";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateHighScoreText();
    }

    void Update()
    {
        // Le score monte avec le temps (et s'arrête quand Time.timeScale = 0)
        score += pointsPerSecond * Time.deltaTime;
        scoreText.text = "SCORE " + Mathf.FloorToInt(score).ToString("00000");
    }

    // Appelée par les objets ramassés
    public void AddScore(int points)
    {
        score += points;
    }

    // Appelée au GAME OVER : on garde le meilleur score
    public void SaveHighScore()
    {
        int current = Mathf.FloorToInt(score);
        int best = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        if (current > best)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, current);
            PlayerPrefs.Save();
            Debug.Log("Nouveau record : " + current);
        }

        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "RECORD " + PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0).ToString("00000");
        }
    }
}
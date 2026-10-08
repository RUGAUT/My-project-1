using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [SerializeField] private TextMeshProUGUI texteScore;

    private int nombrePieces = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        MettreAJourTexte();
    }

    public void AjouterPiece(int valeur)
    {
        nombrePieces += valeur;
        MettreAJourTexte();
    }

    private void MettreAJourTexte()
    {
        texteScore.text = "Pièces : " + nombrePieces;
    }
}
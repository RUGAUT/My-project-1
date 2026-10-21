using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI instance;

    [SerializeField] private Image[] coeurs;       // 3 images, de gauche à droite
    [SerializeField] private Sprite coeurPlein;
    [SerializeField] private Sprite coeurVide;

    private void Awake()
    {
        instance = this;
    }

    public void MettreAJour(int vies)
    {
        for (int i = 0; i < coeurs.Length; i++)
        {
            coeurs[i].sprite = (i < vies) ? coeurPlein : coeurVide;
        }
    }
}
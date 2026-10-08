using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int valeur = 1;

    private void Reset()
    {
        // Met automatiquement le collider en Trigger quand tu ajoutes le script
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AjouterPiece(valeur);
            Destroy(gameObject);
        }
    }
}
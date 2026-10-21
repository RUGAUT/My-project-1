using UnityEngine;

public class SpawnerAleatoire : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject prefab;

    [Header("Intervalle (en secondes)")]
    [SerializeField] private float intervalleMin = 1f;
    [SerializeField] private float intervalleMax = 3f;

    [Header("Zone de spawn")]
    [SerializeField] private Vector2 tailleZone = new Vector2(10f, 2f);

    [Header("Options")]
    [SerializeField] private bool spawnActif = true;
    [SerializeField] private Transform parent;   // optionnel : range les objets spawnés dans un parent

    private float minuteur;

    private void Start()
    {
        ProgrammerProchainSpawn();
    }

    private void Update()
    {
        if (!spawnActif || prefab == null) return;

        minuteur -= Time.deltaTime;

        if (minuteur <= 0f)
        {
            Spawn();
            ProgrammerProchainSpawn();
        }
    }

    private void Spawn()
    {
        // Position aléatoire dans la zone (centrée sur le spawner)
        float x = Random.Range(-tailleZone.x / 2f, tailleZone.x / 2f);
        float y = Random.Range(-tailleZone.y / 2f, tailleZone.y / 2f);
        Vector3 position = transform.position + new Vector3(x, y, 0f);

        Instantiate(prefab, position, Quaternion.identity, parent);
    }

    private void ProgrammerProchainSpawn()
    {
        minuteur = Random.Range(intervalleMin, intervalleMax);
    }

    // Affiche la zone de spawn dans la vue Scene
    private void OnDrawGizmos()
    {
        Vector3 taille = new Vector3(tailleZone.x, tailleZone.y, 0f);

        Gizmos.color = new Color(1f, 0.8f, 0f, 0.2f);   // remplissage jaune transparent
        Gizmos.DrawCube(transform.position, taille);

        Gizmos.color = Color.yellow;                     // contour
        Gizmos.DrawWireCube(transform.position, taille);
    }
}
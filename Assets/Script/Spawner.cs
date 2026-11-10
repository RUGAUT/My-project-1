using UnityEngine;

// Fait apparaître des objets à intervalles aléatoires (cactus, oiseaux… ou pièces).
// À placer sur un objet vide « Spawner », à droite de l'écran.
// Astuce : un Spawner pour les cactus, un autre pour les oiseaux, un autre pour les pièces.
public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;        // les modèles possibles (glissez vos Prefabs ici)
    public float minInterval = 1f;      // délai minimum entre deux apparitions (secondes)
    public float maxInterval = 2.5f;    // délai maximum
    public float[] heights = { 0f };    // hauteurs possibles au-dessus du Spawner

    private float timer = 0f;
    private float nextSpawn = 1f;

    void Update()
    {
        // On compte le temps écoulé
        timer += Time.deltaTime;

        if (timer >= nextSpawn)
        {
            timer = 0f;

            // Prochain délai, choisi au hasard
            nextSpawn = Random.Range(minInterval, maxInterval);

            // Quel objet ? À quelle hauteur ?
            int index = Random.Range(0, prefabs.Length);
            float height = heights[Random.Range(0, heights.Length)];

            // Création d'une copie du Prefab
            Vector3 position = transform.position + new Vector3(0f, height, 0f);
            Instantiate(prefabs[index], position, Quaternion.identity);
        }
    }
}
using UnityEngine;


/// Le Spawner crée des obstacles à intervalles réguliers.
/// La difficulté (fréquence + vitesse + dégâts) est ajustée par le GameManager.

public class Spawner : MonoBehaviour
{
    [Header("Prefab de l'obstacle à instancier")]
    [SerializeField] private Obstacle ObstaclePrefab;

    [Header("Zone de spawn (en local)")]
    [SerializeField] private Vector2 SpawnBounds;

    [Header("Délai entre deux spawns (min, max)")]
    [SerializeField] private Vector2 SpawnDelay;

    private float _nextSpawn; // Temps auquel le prochain obstacle doit apparaître

    private void Update()
    {
        // Si on a dépassé le temps prévu pour le prochain spawn
        if (Time.time > _nextSpawn)
        {
            SpawnSphere();

            // Délai aléatoire entre min et max
            float delay = Random.Range(SpawnDelay.x, SpawnDelay.y);

            // Plus le niveau est dur, plus ça spawn vite
            delay /= GameManager.Instance.SpawnRateMultiplier;

            _nextSpawn = Time.time + delay;
        }
    }

    /// Instancie un obstacle dans la zone définie et lui applique la difficulté du niveau.
    private void SpawnSphere()
    {
        // Création de l'obstacle comme enfant du Spawner
        Obstacle o = Instantiate(ObstaclePrefab, transform);

        // Position locale aléatoire dans la zone
        o.transform.localPosition = new Vector3(
            Random.Range(-SpawnBounds.x, SpawnBounds.x),
            Random.Range(-SpawnBounds.y, SpawnBounds.y),
            0
        );

        // Applique la difficulté du niveau actuel à l'obstacle
        o.SetDifficulty(
            GameManager.Instance.ObstacleSpeedMultiplier,
            GameManager.Instance.ObstacleDamage
        );
    }

    /// Affiche la zone de spawn dans la scène (éditeur uniquement).
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, (Vector3)SpawnBounds);
    }
}
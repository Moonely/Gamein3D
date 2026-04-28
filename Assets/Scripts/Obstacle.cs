using UnityEngine;


/// Obstacle : sphère qui avance vers le joueur.
/// Sa vitesse et ses dégâts sont configurés par le Spawner via SetDifficulty.

public class Obstacle : MonoBehaviour
{
    //vitesse de l'obstacle au niveau 1
    [SerializeField] private float BaseSpeed = 5f;
    [SerializeField] private float DestroyDistance = -10f;
    [SerializeField] private int BaseDamages = 1;

    private float CurrentSpeed;
    private int CurrentDamages;

    //  Awake (et pas Start) pour que l'init se fasse AVANT que le Spawner appelle SetDifficulty
    private void Awake()
    {
        CurrentSpeed = BaseSpeed;
        CurrentDamages = BaseDamages;
    }

    private void Update()
    {
        // L'obstacle avance vers le joueur (axe Z négatif)
        transform.position += new Vector3(0, 0, -CurrentSpeed * Time.deltaTime);

        // S'il dépasse la zone, on le détruit
        if (transform.position.z < DestroyDistance)
            Destroy(gameObject);
    }

    /// Détruit l'obstacle et retourne les dégâts qu'il inflige.
    public int Explode()
    {
        Destroy(gameObject);
        return CurrentDamages;
    }

    /// Configure la difficulté de cet obstacle selon le niveau.
    public void SetDifficulty(float speedMultiplier, int damage)
    {
        CurrentSpeed = BaseSpeed * speedMultiplier;
        CurrentDamages = damage;
    }

    public float GetCurrentSpeed() => CurrentSpeed;
    public int GetCurrentDamages() => CurrentDamages;
}
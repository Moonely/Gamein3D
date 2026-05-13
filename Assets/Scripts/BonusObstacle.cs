using UnityEngine;

public enum BonusType
{
    Health,
    Score
}

[RequireComponent(typeof(Obstacle))]
public class BonusObstacle : MonoBehaviour
{
    /*public BonusType BonusType;
    public int HealthBonus = 20;*/
    public int ScoreBonus = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        /*if (BonusType == BonusType.Health)
            GameManager.Instance.AddHealth(HealthBonus);
        else*/
        GameManager.Instance.AddScore(ScoreBonus);

        Destroy(gameObject);
    }
}

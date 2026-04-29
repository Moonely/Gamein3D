using TMPro;
using UnityEngine;

public class CountScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    public int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (score <= 0)
            score = 0;
        score += (int)(15 * Time.deltaTime);
        scoreText.text = "Score : " + score; 
    }
}

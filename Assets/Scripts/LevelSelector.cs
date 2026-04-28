using UnityEngine;
using UnityEngine.SceneManagement;

// Script du menu pour choisir le niveau de départ
public class LevelSelector : MonoBehaviour
{
    [Tooltip("Nom exact de la scène de jeu (MainMenu)")]
    public string gameSceneName = "GameScene";

    public void ChoisirNiveau1()
    {
        GameManager.SelectedLevel = 1;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ChoisirNiveau2()
    {
        GameManager.SelectedLevel = 2;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ChoisirNiveau3()
    {
        GameManager.SelectedLevel = 3;
        SceneManager.LoadScene(gameSceneName);
    }
}
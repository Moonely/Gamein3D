using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

// ==========================================================
// GAME MANAGER (mode Mission)
// ==========================================================
// - Le joueur démarre à un niveau choisi dans le menu.
// - Il doit survivre 20 secondes pour réussir le niveau.
// - À la fin, écran de réussite : "Continuer" ou "Retour menu".
// - Au niveau 3 réussi : écran de victoire totale.
// - Game Over géré par Player.cs (à 0 HP).
// ==========================================================
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int SelectedLevel = 1;

    [Header("Réglages des niveaux")]
    [Tooltip("Durée d'un niveau en secondes")]
    public float levelDuration = 20f;

    [Tooltip("Niveau maximum (au-delà = victoire totale)")]
    public int maxLevel = 3;

    [Header("Affichage UI")]
    public TMP_Text levelText;
    public Slider progressBar;
    public TMP_Text levelChangePopup;

    [Header("Écrans Mission")]
    [Tooltip("Panneau qui s'affiche quand un niveau est réussi (sauf le dernier)")]
    public GameObject levelCompletePanel;

    [Tooltip("Texte du panneau Réussite (ex: 'Niveau 1 terminé !')")]
    public TMP_Text levelCompleteText;

    [Tooltip("Panneau qui s'affiche quand le jeu est terminé (niveau 3 réussi)")]
    public GameObject victoryPanel;

    [SerializeField]
    private TextMeshProUGUI CountScore;

    // Multiplicateurs lus par les autres scripts
    public float SpawnRateMultiplier { get; private set; } = 1f;
    public float ObstacleSpeedMultiplier { get; private set; } = 1f;
    public int ObstacleDamage { get; private set; } = 1;

    private int currentLevel = 1;
    private float levelTimer = 0f;
    private bool isPaused = false;  // Quand un panneau est affiché

    // ==========================================================
    // AWAKE
    // ==========================================================
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            return;
        }
        Instance = this;
    }

    // ==========================================================
    // START
    // ==========================================================
    void Start()
    {
        // Cache tous les panneaux au démarrage
        if (levelChangePopup != null) levelChangePopup.gameObject.SetActive(false);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        // Sécurité : on remet le temps à vitesse normale
        Time.timeScale = 1f;

        // Lance le niveau choisi
        StartLevel(SelectedLevel);
    }

    // ==========================================================
    // UPDATE : timer du niveau
    // ==========================================================
    void Update()
    {
        // Si le jeu est en pause (panneau affiché), on ne fait rien
        if (isPaused) return;

        // Incrémente le timer
        levelTimer += Time.deltaTime;

        // Met à jour la barre de progression
        if (progressBar != null)
            progressBar.value = levelTimer / levelDuration;

        // Quand le temps est écoulé → niveau réussi !
        if (levelTimer >= levelDuration)
        {
            OnLevelComplete();
        }
    }

    // ==========================================================
    // START LEVEL : démarre un niveau spécifique
    // ==========================================================
    public void StartLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, maxLevel);
        levelTimer = 0f;
        isPaused = false;

        // Multiplicateurs selon le niveau
        switch (currentLevel)
        {
            case 1:
                SpawnRateMultiplier = 1f;
                ObstacleSpeedMultiplier = 1f;
                ObstacleDamage = 1;
                break;
            case 2:
                SpawnRateMultiplier = 2f;
                ObstacleSpeedMultiplier = 2f;
                ObstacleDamage = 2;
                break;
            case 3:
                SpawnRateMultiplier = 4f;
                ObstacleSpeedMultiplier = 3.5f;
                ObstacleDamage = 4;
                break;
        }

        if (levelText != null) levelText.text = "Niveau " + currentLevel;
        if (progressBar != null) progressBar.value = 0f;

        if (levelChangePopup != null)
            StartCoroutine(ShowLevelChangePopup());

        Debug.Log("[GameManager] Démarrage du niveau " + currentLevel);
    }

    // ==========================================================
    // ON LEVEL COMPLETE : appelé quand le timer atteint la fin
    // ==========================================================
    private void OnLevelComplete()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Met le jeu en pause

        if (currentLevel >= maxLevel)
        {
            // Dernier niveau réussi → Victoire totale
            if (victoryPanel != null) victoryPanel.SetActive(true);
            
            Debug.Log("[GameManager] VICTOIRE TOTALE !");
        }
        else
        {
            // Niveau réussi → propose de continuer
            if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
            if (levelCompleteText != null)
                levelCompleteText.text = "Niveau " + currentLevel + " terminé !";
            Debug.Log("[GameManager] Niveau " + currentLevel + " terminé");
        }
    }

    // ==========================================================
    // BOUTONS DES PANNEAUX
    // ==========================================================

    /// Bouton "Continuer" du panneau Niveau Réussi
    public void ContinueToNextLevel()
    {
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        Time.timeScale = 1f;
        StartLevel(currentLevel + 1);
    }

    /// Bouton "Menu" (panneau Réussi ou Victoire)
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// Bouton "Rejouer" du panneau Victoire
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SelectedLevel = 1;  // On recommence depuis le niveau 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ==========================================================
    // GET CURRENT LEVEL
    // ==========================================================
    public int GetCurrentLevel() => currentLevel;

    // ==========================================================
    // POPUP ANIMÉ "NIVEAU X !"
    // ==========================================================
    private IEnumerator ShowLevelChangePopup()
    {
        levelChangePopup.text = "NIVEAU " + currentLevel + " !";
        levelChangePopup.gameObject.SetActive(true);

        float duration = 2f;
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 endScale = Vector3.one * 1.5f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // unscaled pour marcher même en pause
            float t = elapsed / duration;
            levelChangePopup.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            Color c = levelChangePopup.color;
            c.a = 1f - t;
            levelChangePopup.color = c;
            yield return null;
        }

        levelChangePopup.gameObject.SetActive(false);
        Color reset = levelChangePopup.color;
        reset.a = 1f;
        levelChangePopup.color = reset;
        levelChangePopup.transform.localScale = Vector3.one;
    }

    internal void AddHealth(int healthBonus)
    {
        throw new NotImplementedException();
    }

    internal void AddScore(int scoreBonus)
    {
        CountScore cs = FindAnyObjectByType<CountScore>();
        cs.score += scoreBonus ;
    }
}
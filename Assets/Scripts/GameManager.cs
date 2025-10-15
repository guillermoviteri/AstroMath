using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int lives = 3;
    public int initialLives = 3; // Guardar el número inicial de vidas

    [Header("Configuración de Victoria")]
    public int scoreParaGanar = 10;
    public string siguienteEscena = "Nivel2";

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text pointFinal;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Audio")]
    public AudioClip gameMusic;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        initialLives = lives; // Guardar vidas iniciales

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (gameMusic != null)
        {
            audioSource.clip = gameMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        UpdateUI();
        UpdateFinal();
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
        UpdateFinal();

        if (correctAnswerSound != null)
        {
            audioSource.PlayOneShot(correctAnswerSound);
        }

        CheckVictory();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (wrongAnswerSound != null)
        {
            audioSource.PlayOneShot(wrongAnswerSound);
        }

        if (lives <= 0)
        {
            // Verificar si puede mostrar última oportunidad
            if (LastChanceManager.instance.CanShowLastChance())
            {
                LastChanceManager.instance.ShowLastChance();
            }
            else
            {
                GameOver();
            }
        }
    }

    // Nuevo método para revivir con todas las vidas
    public void ReviveWithFullLives()
    {
        lives = initialLives;
        UpdateUI();

        // Reproducir sonido de éxito si lo deseas
        if (correctAnswerSound != null)
        {
            audioSource.PlayOneShot(correctAnswerSound);
        }
    }

    // Cambiar GameOver a público para que LastChanceManager pueda llamarlo
    public void GameOver()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void UpdateFinal()
    {
        pointFinal.text = "Ecuaciones correctas: " + score;
    }

    void UpdateUI()
    {
        scoreText.text = "Puntos: " + score + " / " + scoreParaGanar;
        livesText.text = "Vidas: " + lives;
    }

    void CheckVictory()
    {
        if (score >= scoreParaGanar)
        {
            Victory();
        }
    }

    void Victory()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void NextLevel()
    {
        Time.timeScale = 1; // Asegurar que esté en 1
        if (SceneExists(siguienteEscena))
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            Debug.LogWarning("La escena '" + siguienteEscena + "' no existe. Volviendo al menú principal.");
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (scene == sceneName)
                return true;
        }
        return false;
    }
}
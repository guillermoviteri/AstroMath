using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int lives = 3;

    [Header("Configuración de Victoria")]
    public int scoreParaGanar = 10; // Puntaje requerido para ganar
    public string siguienteEscena = "Nivel2"; // Nombre de la siguiente escena

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text pointFinal;
    public GameObject gameOverPanel;
    public GameObject victoryPanel; // Panel de victoria

    [Header("Audio")]
    public AudioClip gameMusic;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    //public AudioClip victorySound; // Sonido de victoria
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
        // Obtener o agregar el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar y reproducir la música
        if (gameMusic != null)
        {
            audioSource.clip = gameMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        UpdateUI();
        UpdateFinal();
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false); // Asegurar que el panel de victoria esté oculto
        Time.timeScale = 1;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
        UpdateFinal();

        // Reproducir sonido de respuesta correcta
        if (correctAnswerSound != null)
        {
            audioSource.PlayOneShot(correctAnswerSound);
        }

        // Verificar si el jugador ganó
        CheckVictory();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        // Reproducir sonido de respuesta incorrecta
        if (wrongAnswerSound != null)
        {
            audioSource.PlayOneShot(wrongAnswerSound);
        }

        if (lives <= 0)
            GameOver();
    }

    void UpdateFinal()
    {
        pointFinal.text = "Ecuaciones correctas: " + score;
    }

    void UpdateUI()
    {
        scoreText.text = "Puntos: " + score + " / " + scoreParaGanar; // Mostrar progreso
        livesText.text = "Vidas: " + lives;
    }

    void CheckVictory()
    {
        // Verificar si el puntaje alcanzó el objetivo
        if (score >= scoreParaGanar)
        {
            Victory();
        }
    }

    void Victory()
    {
        // Detener la música del juego
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Reproducir sonido de victoria
        /*if (victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }*/

        // Mostrar panel de victoria
        victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void GameOver()
    {
        // Detener la música cuando el juego termina
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // Método para pasar al siguiente nivel
    public void NextLevel()
    {
        Time.timeScale = 1; // Restaurar escala de tiempo

        // Verificar si la escena existe
        if (SceneExists(siguienteEscena))
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            Debug.LogWarning("La escena '" + siguienteEscena + "' no existe. Volviendo al menú principal.");
            // Puedes cargar una escena por defecto o mostrar un mensaje
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Restaurar escala de tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Método para verificar si una escena existe
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
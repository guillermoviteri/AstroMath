using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int lives = 3;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text pointFinal;
    public GameObject gameOverPanel;
    public AudioClip gameMusic; // Música del juego
    public AudioClip correctAnswerSound; // Sonido para respuesta correcta
    public AudioClip wrongAnswerSound; // Sonido para respuesta incorrecta
    private AudioSource audioSource; // AudioSource para reproducir la música

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
            // Usar PlayOneShot para no interrumpir la música de fondo
            audioSource.PlayOneShot(correctAnswerSound);
        }
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
        scoreText.text = "Puntos: " + score;
        livesText.text = "Vidas: " + lives;
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

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Botones")]
    public Button btnReiniciar;
    public Button btnMenuPrincipal;

    [Header("Audio")] // NUEVO
    public AudioClip gameOverSound; // NUEVO
    private AudioSource audioSource; // NUEVO

    void Start()
    {
        // NUEVO: Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // NUEVO: Reproducir sonido de game over
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        btnReiniciar.onClick.AddListener(ReiniciarNivel);
        btnMenuPrincipal.onClick.AddListener(IrAlMenu);
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Botones")]
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
        btnMenuPrincipal.onClick.AddListener(IrAlMenu);
    }
    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
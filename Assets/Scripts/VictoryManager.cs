using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [Header("Botones")]
    public Button btnSiguienteNivel;
    public Button btnMenuPrincipal;

    [Header("Configuración de Escenas")]
    public string siguienteNivel;

    [Header("Audio")] // NUEVO
    public AudioClip victorySound; // NUEVO
    private AudioSource audioSource; // NUEVO

    void Start()
    {
        // NUEVO: Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // NUEVO: Reproducir sonido de victoria
        if (victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }

        // Configurar los listeners de los botones
        btnSiguienteNivel.onClick.AddListener(IrAlSiguienteNivel);
        btnMenuPrincipal.onClick.AddListener(IrAlMenu);
    }

    public void IrAlSiguienteNivel()
    {
        if (!string.IsNullOrEmpty(siguienteNivel))
        {
            SceneManager.LoadScene(siguienteNivel);
        }
        else
        {
            Debug.LogWarning("No se ha definido una escena siguiente en el Inspector");
            SceneManager.LoadScene("Menu");
        }
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
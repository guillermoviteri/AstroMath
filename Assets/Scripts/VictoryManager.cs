using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [Header("Botones")]
    public Button btnSiguienteNivel;
    public Button btnMenuPrincipal;

    [Header("Configuración de Escenas")]
    public string siguienteNivel; // Define el nombre de la siguiente escena en el Inspector

    void Start()
    {
        // Configurar los listeners de los botones
        btnSiguienteNivel.onClick.AddListener(IrAlSiguienteNivel);
        btnMenuPrincipal.onClick.AddListener(IrAlMenu);
    }

    public void IrAlSiguienteNivel()
    {
        // Verificar si se definió una escena siguiente
        if (!string.IsNullOrEmpty(siguienteNivel))
        {
            SceneManager.LoadScene(siguienteNivel);
        }
        else
        {
            Debug.LogWarning("No se ha definido una escena siguiente en el Inspector");
            // Opcional: cargar una escena por defecto o mostrar mensaje
            SceneManager.LoadScene("Menu");
        }
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
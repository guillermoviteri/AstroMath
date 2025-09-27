using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [Header("Botones")]
    public Button btnJugar;
    public Button btnSalir;

    void Start()
    {
        
        btnJugar.onClick.AddListener(Jugar);
        btnSalir.onClick.AddListener(Salir);
    }

    
    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

  
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

    }
}

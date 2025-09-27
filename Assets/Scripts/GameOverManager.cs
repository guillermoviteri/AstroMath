using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Botones")]
    public Button btnReiniciar;
    public Button btnMenuPrincipal;

    void Start()
    {
    
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
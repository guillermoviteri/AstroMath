using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager1 : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public Button botonJugar;
    public Button botonSalir;

    [Header("Configuraci�n de Animaci�n")]
    public float duracionAnimacion = 0.8f;
    public float delayEntreBotones = 0.2f;
    public float escalaFinal = 2f;

    private Sequence secuenciaEntrada;

    void Start()
    {
        Debug.Log("MenuManager1 - Start() ejecutado");

        if (botonJugar == null || botonSalir != null)
        {
            Debug.LogError("�Referencias de botones no asignadas en el Inspector!");
            return;
        }

        ConfigurarEventosBotones();
        IniciarAnimacionMenu();
    }

    void OnEnable()
    {
        Debug.Log("MenuManager1 - OnEnable() ejecutado");
        // Reiniciar animaci�n cuando el objeto se active
        IniciarAnimacionMenu();
    }

    void IniciarAnimacionMenu()
    {
        Debug.Log("Iniciando animaci�n del men�...");

        // Limpiar secuencia anterior de forma segura
        LimpiarAnimaciones();

        // Resetear estado de los botones
        botonJugar.transform.localScale = Vector3.zero;
        botonSalir.transform.localScale = Vector3.zero;
        botonJugar.interactable = false;
        botonSalir.interactable = false;

        // Crear NUEVA secuencia
        secuenciaEntrada = DOTween.Sequence();

        // Animaci�n del bot�n Jugar
        secuenciaEntrada.Append(botonJugar.transform.DOScale(escalaFinal, duracionAnimacion)
            .SetEase(Ease.OutBack)
            .OnStart(() => Debug.Log("Animando bot�n Jugar")));

        // Delay entre botones
        secuenciaEntrada.AppendInterval(delayEntreBotones);

        // Animaci�n del bot�n Salir
        secuenciaEntrada.Append(botonSalir.transform.DOScale(escalaFinal, duracionAnimacion)
            .SetEase(Ease.OutBack)
            .OnStart(() => Debug.Log("Animando bot�n Salir")));

        // Activar interacci�n cuando termine la animaci�n
        secuenciaEntrada.OnComplete(() => {
            botonJugar.interactable = true;
            botonSalir.interactable = true;
            Debug.Log("Animaci�n completada - Botones interactivos");
        });

        // Configuraci�n importante para evitar problemas
        secuenciaEntrada.SetUpdate(true);
        secuenciaEntrada.SetAutoKill(false);
    }

    void ConfigurarEventosBotones()
    {
        botonJugar.onClick.RemoveAllListeners();
        botonSalir.onClick.RemoveAllListeners();

        botonJugar.onClick.AddListener(() =>
        {
            Debug.Log("Clic en Bot�n Jugar");
            EfectoClicBoton(botonJugar.transform, CargarJuego);
        });

        botonSalir.onClick.AddListener(() =>
        {
            Debug.Log("Clic en Bot�n Salir");
            EfectoClicBoton(botonSalir.transform, SalirDelJuego);
        });
    }

    void EfectoClicBoton(Transform botonTransform, System.Action accion)
    {
        // Crear una secuencia INDEPENDIENTE para el efecto de clic
        Sequence secuenciaClic = DOTween.Sequence();

        secuenciaClic.Append(botonTransform.DOScale(escalaFinal * 0.8f, 0.1f));
        secuenciaClic.Append(botonTransform.DOScale(escalaFinal, 0.3f).SetEase(Ease.OutBack));
        secuenciaClic.OnComplete(() => accion?.Invoke());

        // Esta secuencia se auto-destruir� al completarse
        secuenciaClic.SetAutoKill(true);
    }

    void CargarJuego()
    {
        Debug.Log("Cargando escena del juego...");

        // Limpiar antes de cambiar de escena
        LimpiarAnimaciones();

        // Peque�o delay para que se vea el efecto de clic
        StartCoroutine(CargarEscenaConDelay("Nivel 1", 0.1f));
    }

    void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        // Limpiar antes de salir
        LimpiarAnimaciones();

        StartCoroutine(SalirConDelay(0.1f));
    }

    IEnumerator CargarEscenaConDelay(string nombreEscena, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nombreEscena);
    }

    IEnumerator SalirConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void LimpiarAnimaciones()
    {
        // Limpiar secuencia principal de forma controlada
        if (secuenciaEntrada != null && secuenciaEntrada.IsActive())
        {
            secuenciaEntrada.Kill();
            secuenciaEntrada = null;
        }

        // Limpiar tweens individuales de los botones de forma segura
        if (botonJugar != null)
        {
            botonJugar.transform.DOKill();
            botonJugar.interactable = true; // Asegurar que sea interactivo
        }
        if (botonSalir != null)
        {
            botonSalir.transform.DOKill();
            botonSalir.interactable = true; // Asegurar que sea interactivo
        }
    }

    void OnDisable()
    {
        Debug.Log("MenuManager1 - OnDisable() - Limpiando animaciones");
        LimpiarAnimaciones();
    }

    void OnDestroy()
    {
        Debug.Log("MenuManager1 - OnDestroy() - Limpieza final");
        // Solo limpiar los tweens de ESTE objeto, no todos
        if (botonJugar != null) botonJugar.transform.DOKill();
        if (botonSalir != null) botonSalir.transform.DOKill();

        // NO usar DOTween.KillAll() aqu� - eso causa problemas
    }
}
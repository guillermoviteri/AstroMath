using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MenuManager1 : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public Button botonJugar;
    public Button botonSalir;

    [Header("Configuraci�n de Animaci�n")]
    public float duracionAnimacion = 0.8f;
    public float fuerzaRebote = 0.3f;
    public float delayEntreBotones = 0.2f;

    void Start()
    {
        // Ocultar botones al inicio
        OcultarBotones();

        // Iniciar animaci�n de entrada
        StartCoroutine(MostrarBotonesConEfecto());

        // Configurar eventos de clic
        ConfigurarEventosBotones();
    }

    void OcultarBotones()
    {
        // Establecer escala 0 para ocultar
        botonJugar.transform.localScale = Vector3.zero;
        botonSalir.transform.localScale = Vector3.zero;
    }

    IEnumerator MostrarBotonesConEfecto()
    {
        // Peque�o delay al inicio
        yield return new WaitForSeconds(0.5f);

        // Animaci�n del bot�n Jugar
        botonJugar.transform.DOScale(Vector3.one, duracionAnimacion)
            .SetEase(Ease.OutBack) // Efecto de rebote
            .SetDelay(0f);

        // Delay entre botones
        yield return new WaitForSeconds(delayEntreBotones);

        // Animaci�n del bot�n Salir
        botonSalir.transform.DOScale(Vector3.one, duracionAnimacion)
            .SetEase(Ease.OutBack) // Efecto de rebote
            .SetDelay(0f);
    }

    void ConfigurarEventosBotones()
    {
        // Evento para el bot�n Jugar
        botonJugar.onClick.AddListener(() =>
        {
            EfectoClicBoton(botonJugar.transform, "Iniciando juego...");
            // Aqu� puedes cargar la escena del juego
        });

        // Evento para el bot�n Salir
        botonSalir.onClick.AddListener(() =>
        {
            EfectoClicBoton(botonSalir.transform, "Saliendo del juego...");

            // Salir del juego (en build)
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    void EfectoClicBoton(Transform botonTransform, string mensaje)
    {
        Debug.Log(mensaje);

        // Efecto de pulsaci�n
        Sequence secuencia = DOTween.Sequence();

        secuencia.Append(botonTransform.DOScale(Vector3.one * 0.8f, 0.1f));
        secuencia.Append(botonTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
    }

    // M�todo p�blico para reiniciar animaciones (�til si necesitas mostrar/ocultar men�)
    public void MostrarMenu()
    {
        StartCoroutine(MostrarBotonesConEfecto());
    }

    public void OcultarMenu()
    {
        // Animaci�n de salida
        botonJugar.transform.DOScale(Vector3.zero, duracionAnimacion / 2).SetEase(Ease.InBack);
        botonSalir.transform.DOScale(Vector3.zero, duracionAnimacion / 2).SetEase(Ease.InBack);
    }

    void OnDestroy()
    {
        // Limpiar tweens cuando se destruya el objeto
        DOTween.KillAll();
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MenuManager1 : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public Button botonJugar;
    public Button botonSalir;

    [Header("Configuración de Animación")]
    public float duracionAnimacion = 0.8f;
    public float fuerzaRebote = 0.3f;
    public float delayEntreBotones = 0.2f;

    void Start()
    {
        // Ocultar botones al inicio
        OcultarBotones();

        // Iniciar animación de entrada
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
        // Pequeño delay al inicio
        yield return new WaitForSeconds(0.5f);

        // Animación del botón Jugar
        botonJugar.transform.DOScale(Vector3.one, duracionAnimacion)
            .SetEase(Ease.OutBack) // Efecto de rebote
            .SetDelay(0f);

        // Delay entre botones
        yield return new WaitForSeconds(delayEntreBotones);

        // Animación del botón Salir
        botonSalir.transform.DOScale(Vector3.one, duracionAnimacion)
            .SetEase(Ease.OutBack) // Efecto de rebote
            .SetDelay(0f);
    }

    void ConfigurarEventosBotones()
    {
        // Evento para el botón Jugar
        botonJugar.onClick.AddListener(() =>
        {
            EfectoClicBoton(botonJugar.transform, "Iniciando juego...");
            // Aquí puedes cargar la escena del juego
        });

        // Evento para el botón Salir
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

        // Efecto de pulsación
        Sequence secuencia = DOTween.Sequence();

        secuencia.Append(botonTransform.DOScale(Vector3.one * 0.8f, 0.1f));
        secuencia.Append(botonTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
    }

    // Método público para reiniciar animaciones (útil si necesitas mostrar/ocultar menú)
    public void MostrarMenu()
    {
        StartCoroutine(MostrarBotonesConEfecto());
    }

    public void OcultarMenu()
    {
        // Animación de salida
        botonJugar.transform.DOScale(Vector3.zero, duracionAnimacion / 2).SetEase(Ease.InBack);
        botonSalir.transform.DOScale(Vector3.zero, duracionAnimacion / 2).SetEase(Ease.InBack);
    }

    void OnDestroy()
    {
        // Limpiar tweens cuando se destruya el objeto
        DOTween.KillAll();
    }
}

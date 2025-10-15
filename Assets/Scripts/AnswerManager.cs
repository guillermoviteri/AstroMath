using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AnswerManager : MonoBehaviour
{
    public static AnswerManager instance;
    public GameObject answerButtonPrefab;
    public Transform answersPanel;
    public GameObject answersPanelObject;
    public TMP_Text timerText;
    public TMP_Text equationText; // Nuevo texto para mostrar la ecuación

    private List<GameObject> answerButtons = new List<GameObject>();
    private int correctAnswer;
    private Meteor currentMeteor;
    private Coroutine timerCoroutine;
    private float timeRemaining = 8f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        answersPanelObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        equationText.gameObject.SetActive(false); // Ocultar texto de ecuación al inicio
    }

    public void ShowAnswers(Meteor meteor)
    {
        currentMeteor = meteor;
        correctAnswer = meteor.GetCorrectAnswer();
        answersPanelObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        equationText.gameObject.SetActive(true); // Mostrar texto de ecuación

        // Mostrar la ecuación del meteorito
        equationText.text = meteor.GetEquationText();

        // Reiniciar temporizador
        timeRemaining = 8f;
        timerText.text = timeRemaining.ToString("F1");

        // Iniciar corrutina del temporizador
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(AnswerTimer());

        // Limpiar respuestas anteriores
        ClearAnswers();

        // Crear 4 opciones de respuesta
        List<int> answers = new List<int> { correctAnswer };

        while (answers.Count < 4)
        {
            int variation = Random.Range(1, 6);
            int wrongAnswer = correctAnswer + (Random.Range(0, 2) == 0 ? variation : -variation);

            if (wrongAnswer != correctAnswer && !answers.Contains(wrongAnswer) && wrongAnswer > 0)
                answers.Add(wrongAnswer);
        }

        answers = ShuffleList(answers);

        foreach (int answer in answers)
        {
            GameObject button = Instantiate(answerButtonPrefab, answersPanel);
            button.GetComponentInChildren<TMP_Text>().text = answer.ToString();

            int currentAnswer = answer;
            button.GetComponent<Button>().onClick.AddListener(() => OnAnswerSelected(currentAnswer));

            answerButtons.Add(button);
        }
    }

    IEnumerator AnswerTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.unscaledDeltaTime; // Usar unscaled porque el juego está pausado
            timerText.text = timeRemaining.ToString("F1");
            yield return null;
        }

        // Tiempo agotado
        OnTimeOut();
    }

    void OnAnswerSelected(int selectedAnswer)
    {
        // Detener temporizador
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        CheckAnswer(selectedAnswer);
    }

    void OnTimeOut()
    {
        // Tiempo agotado, tratar como respuesta incorrecta
        Debug.Log("¡Tiempo agotado! El meteorito viene más rápido");
        HidePanel();
        currentMeteor.IncreaseSpeed(2f);
        GameManager.instance.LoseLife();
        ClearAnswers();
    }

    void CheckAnswer(int selectedAnswer)
    {
        HidePanel();

        if (selectedAnswer == correctAnswer)
        {
            Destroy(currentMeteor.gameObject);
            GameManager.instance.AddScore(1);
        }
        else
        {
            currentMeteor.IncreaseSpeed(2f);
            GameManager.instance.LoseLife();
        }

        ClearAnswers();
    }

    void HidePanel()
    {
        answersPanelObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        equationText.gameObject.SetActive(false); // Ocultar texto de ecuación
        Time.timeScale = 1f;
    }

    void ClearAnswers()
    {
        foreach (GameObject btn in answerButtons)
            Destroy(btn);
        answerButtons.Clear();
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LastChanceManager : MonoBehaviour
{
    public static LastChanceManager instance;

    [Header("UI Elements")]
    public GameObject lastChancePanel;
    public TMP_Text equationText;
    public TMP_Text timerText;
    public Transform answersPanel;
    public GameObject answerButtonPrefab;

    [Header("Game Settings")]
    public float timeLimit = 10f;

    private List<GameObject> answerButtons = new List<GameObject>();
    private int correctAnswer;
    private Coroutine timerCoroutine;
    private float currentTime;
    private bool hasUsedLastChance = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        lastChancePanel.SetActive(false);
    }

    public bool CanShowLastChance()
    {
        return !hasUsedLastChance;
    }

    public void ShowLastChance()
    {
        if (hasUsedLastChance) return;

        hasUsedLastChance = true;
        Time.timeScale = 0f;
        lastChancePanel.SetActive(true);

        GenerateComplexEquation();
        StartTimer();
    }

    void GenerateComplexEquation()
    {
        // Generar una ecuación más compleja con 3 operaciones
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        int num3 = Random.Range(1, 10);

        // Elegir operaciones aleatorias
        string[] operations = { "+", "-", "*", "/" };
        string op1 = operations[Random.Range(0, operations.Length)];
        string op2 = operations[Random.Range(0, operations.Length)];

        // Calcular la respuesta correcta
        correctAnswer = CalculateResult(num1, num2, num3, op1, op2);

        // Mostrar la ecuación
        equationText.text = $"{num1} {op1} {num2} {op2} {num3} = ?";

        // Generar respuestas
        GenerateAnswers();
    }

    int CalculateResult(int a, int b, int c, string op1, string op2)
    {
        // Calcular según el orden de operaciones (multiplicación/división primero)
        int result;

        if ((op1 == "*" || op1 == "/") && (op2 == "+" || op2 == "-"))
        {
            // Primera operación primero
            int firstResult = ApplyOperation(a, b, op1);
            result = ApplyOperation(firstResult, c, op2);
        }
        else if ((op2 == "*" || op2 == "/") && (op1 == "+" || op1 == "-"))
        {
            // Segunda operación primero
            int secondResult = ApplyOperation(b, c, op2);
            result = ApplyOperation(a, secondResult, op1);
        }
        else
        {
            // Izquierda a derecha
            int firstResult = ApplyOperation(a, b, op1);
            result = ApplyOperation(firstResult, c, op2);
        }

        return result;
    }

    int ApplyOperation(int a, int b, string operation)
    {
        switch (operation)
        {
            case "+": return a + b;
            case "-": return a - b;
            case "*": return a * b;
            case "/":
                // Asegurar división entera
                while (b == 0) b = Random.Range(1, 5);
                return a / b;
            default: return a + b;
        }
    }

    void GenerateAnswers()
    {
        ClearAnswers();

        List<int> answers = new List<int> { correctAnswer };

        // Generar respuestas incorrectas
        while (answers.Count < 4)
        {
            int variation = Random.Range(1, 6);
            int wrongAnswer = correctAnswer + (Random.Range(0, 2) == 0 ? variation : -variation);

            // Asegurar que no sea la respuesta correcta y sea positiva
            if (wrongAnswer != correctAnswer && !answers.Contains(wrongAnswer) && wrongAnswer > 0)
                answers.Add(wrongAnswer);
        }

        // Mezclar respuestas
        answers = ShuffleList(answers);

        // Crear botones de respuesta
        foreach (int answer in answers)
        {
            GameObject button = Instantiate(answerButtonPrefab, answersPanel);
            button.GetComponentInChildren<TMP_Text>().text = answer.ToString();

            int currentAnswer = answer;
            button.GetComponent<Button>().onClick.AddListener(() => OnAnswerSelected(currentAnswer));

            answerButtons.Add(button);
        }
    }

    void StartTimer()
    {
        currentTime = timeLimit;
        timerText.text = currentTime.ToString("F1");

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(LastChanceTimer());
    }

    IEnumerator LastChanceTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.unscaledDeltaTime;
            timerText.text = currentTime.ToString("F1");
            yield return null;
        }

        // Tiempo agotado
        OnTimeOut();
    }

    void OnAnswerSelected(int selectedAnswer)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        CheckAnswer(selectedAnswer);
    }

    void OnTimeOut()
    {
        // Tiempo agotado - perder
        HidePanel();
        GameManager.instance.GameOver();
    }

    void CheckAnswer(int selectedAnswer)
    {
        HidePanel();

        if (selectedAnswer == correctAnswer)
        {
            // ¡Éxito! Recuperar todas las vidas
            GameManager.instance.ReviveWithFullLives();
        }
        else
        {
            // Respuesta incorrecta - perder
            GameManager.instance.GameOver();
        }
    }

    void HidePanel()
    {
        lastChancePanel.SetActive(false);
        ClearAnswers();
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
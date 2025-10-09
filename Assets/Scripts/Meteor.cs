using UnityEngine;
using TMPro;

public class Meteor : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 4f;
    private float speed;
    public TMP_Text operationText;
    private string operation;
    private int correctAnswer;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
    }

    public void SetOperation(string op, int answer)
    {
        operation = op;
        correctAnswer = answer;
        operationText.text = op + " = ?";
    }

    public int GetCorrectAnswer()
    {
        return correctAnswer;
    }

    // Nuevo método para obtener el texto de la ecuación
    public string GetEquationText()
    {
        return operation + " = ?";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject); // Destruir bala
            Time.timeScale = 0f; // Pausar juego
            AnswerManager.instance.ShowAnswers(this); // Mostrar respuestas
        }

        if (other.CompareTag("BottomBoundary"))
        {
            Destroy(gameObject);
            GameManager.instance.LoseLife();
        }
    }

    public void IncreaseSpeed(float multiplier)
    {
        GetComponent<Rigidbody2D>().linearVelocity *= multiplier;
    }
}
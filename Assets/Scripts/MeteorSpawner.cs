
using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float initialSpawnRate = 2f;
    public float minSpawnRate = 0.5f;
    public float difficultyIncreaseRate = 0.1f;
    private float currentSpawnRate;
    private float nextSpawn = 0f;
    public int difficultyLevel = 1;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(IncreaseDifficulty());
    }

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + currentSpawnRate;
            SpawnMeteor();
        }
    }

    void SpawnMeteor()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-7f, 7f), 6f);
        GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        string operation = GenerateOperation(difficultyLevel);
        int answer = CalculateAnswer(operation);

        meteor.GetComponent<Meteor>().SetOperation(operation, answer);
    }

    string GenerateOperation(int difficulty)
    {
        int num1, num2;
        string operation = "";
        int opType = Random.Range(0, 4);

        switch (opType)
        {
            case 0: // Suma
                num1 = Random.Range(1, 5 + difficulty * 3);
                num2 = Random.Range(1, 5 + difficulty * 3);
                operation = $"{num1} + {num2}";
                break;
            case 1: // Resta
                num1 = Random.Range(5, 10 + difficulty * 4);
                num2 = Random.Range(1, num1);
                operation = $"{num1} - {num2}";
                break;
            case 2: // Multiplicación
                num1 = Random.Range(1, 3 + difficulty);
                num2 = Random.Range(1, 5 + difficulty);
                operation = $"{num1} × {num2}";
                break;
            case 3: // División
                num2 = Random.Range(1, 3 + difficulty);
                int result = Random.Range(1, 5 + difficulty);
                num1 = num2 * result;
                operation = $"{num1} ÷ {num2}";
                break;
        }

        return operation;
    }

    int CalculateAnswer(string operation)
    {
        if (operation.Contains("+"))
        {
            string[] parts = operation.Split('+');
            return int.Parse(parts[0].Trim()) + int.Parse(parts[1].Trim());
        }
        else if (operation.Contains("-"))
        {
            string[] parts = operation.Split('-');
            return int.Parse(parts[0].Trim()) - int.Parse(parts[1].Trim());
        }
        else if (operation.Contains("×"))
        {
            string[] parts = operation.Split('×');
            return int.Parse(parts[0].Trim()) * int.Parse(parts[1].Trim());
        }
        else if (operation.Contains("÷"))
        {
            string[] parts = operation.Split('÷');
            return int.Parse(parts[0].Trim()) / int.Parse(parts[1].Trim());
        }
        return 0;
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            difficultyLevel++;
            currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate - difficultyIncreaseRate);
        }
    }
}
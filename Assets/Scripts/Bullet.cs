using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * speed;
        Destroy(gameObject, 2f); // Auto-destrucci�n despu�s de 2 segundos
    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public AudioClip shootSound; // Agregar esta variable para el sonido de disparo
    private float nextFire = 0f;
    private AudioSource audioSource; // Referencia al AudioSource

    void Start()
    {
        // Obtener o agregar el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * moveX * speed * Time.deltaTime);

        // Limitar movimiento dentro de pantalla
        float clampedX = Mathf.Clamp(transform.position.x, -8f, 8f);
        transform.position = new Vector2(clampedX, transform.position.y);

        // Disparar con barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Reproducir sonido de disparo
            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteor"))
        {
            Destroy(other.gameObject);
            GameManager.instance.LoseLife();
        }
    }
}

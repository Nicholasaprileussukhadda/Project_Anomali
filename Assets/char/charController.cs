using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerak
    public float gravity = -9.81f; // Gravitasi
    public Transform cameraTransform; // Transform dari kamera untuk menentukan arah gerakan
    private CharacterController controller;
    private Vector3 velocity; // Menyimpan kecepatan untuk gravitasi
    private bool isGrounded; // Mengecek apakah karakter di tanah
    private Animator animator; // Animator untuk mengatur animasi
    public float rotationSpeed = 10f; // Kecepatan rotasi tubuh karakter
    public float mouseThreshold = 0.1f; // Ambang batas untuk pergerakan mouse

    void Start()
    {
        // Mendapatkan komponen CharacterController dan Animator
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mengecek apakah karakter berada di tanah
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mengatur sedikit tekanan ke tanah agar karakter tidak melayang
        }

        // Input untuk pergerakan WASD
        float horizontal = Input.GetAxis("Horizontal"); // A/D untuk Kiri/Kanan
        float vertical = Input.GetAxis("Vertical"); // W/S untuk Maju/Mundur
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Set animasi berdasarkan input gerakan
        float speed = direction.magnitude; // Menghitung kecepatan berdasarkan input gerakan
        animator.SetFloat("Speed", speed); // Set parameter animasi "Speed" di Animator

        // Jika ada input gerakan dari WASD
        if (speed >= 0.1f)
        {
            // Arahkan gerakan mengikuti rotasi kamera
            Vector3 forward = cameraTransform.forward; // Dapatkan arah depan kamera
            Vector3 right = cameraTransform.right; // Dapatkan arah kanan kamera

            // Hilangkan komponen vertikal untuk pergerakan horizontal yang benar
            forward.y = 0f;
            right.y = 0f;

            // Normalisasi agar vektor tetap valid
            forward.Normalize();
            right.Normalize();

            // Tentukan arah gerakan akhir: vertikal mengikuti arah depan kamera, horizontal mengikuti arah kanan/kiri
            Vector3 moveDirection = forward * vertical + right * horizontal;

            // Gerakkan karakter
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Input dari mouse untuk rotasi kamera
        float mouseX = Input.GetAxis("Mouse X"); // Input rotasi horizontal dari mouse

        // Hanya rotasi kamera jika pergerakan mouse melebihi ambang batas
        if (Mathf.Abs(mouseX) > mouseThreshold)
        {
            // Putar tubuh karakter agar selalu mengikuti rotasi horizontal kamera
            Quaternion targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Menambahkan gravitasi ke karakter
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); // Terapkan gravitasi
    }
}

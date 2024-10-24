using UnityEngine;

public class charController : MonoBehaviour
{
    public float moveSpeed = 3f; // Kecepatan gerak
    private CharacterController controller; // Komponen CharacterController
    private Vector3 velocity; // Menyimpan kecepatan gravitasi
    public float gravity = -9.81f; // Gravitasi
    private Animator animator; // Animator untuk mengatur animasi
    public Transform cameraTransform; // Tambahkan referensi ke transformasi kamera

    void Start()
    {
        // Mendapatkan komponen CharacterController dan Animator
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Input untuk pergerakan Maju/Mundur dan Kiri/Kanan
        float h = Input.GetAxis("Horizontal"); // A/D untuk Kiri/Kanan
        float v = Input.GetAxis("Vertical"); // W/S untuk Maju/Mundur

        // Mengambil arah gerakan dari input WASD relatif terhadap rotasi kamera
        Vector3 forward = cameraTransform.forward; // Ambil arah depan kamera
        Vector3 right = cameraTransform.right; // Ambil arah kanan kamera

        // Hapus komponen vertikal dari arah kamera agar karakter tidak bergerak ke atas/bawah
        forward.y = 0f;
        right.y = 0f;

        // Normalisasi vektor untuk menghindari pergerakan lebih cepat secara diagonal
        forward.Normalize();
        right.Normalize();

        // Menghitung vektor gerak sesuai arah kamera
        Vector3 move = forward * v + right * h;

        // Gerakkan karakter menggunakan CharacterController
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Terapkan gravitasi
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Jika karakter di tanah, reset gravitasi
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); // Terapkan gravitasi

        // Atur animasi berdasarkan input gerakan
        if (h != 0 || v != 0)
        {
            // Jika ada input gerakan, aktifkan animasi berjalan
            animator.SetBool("stat_jalan", true);
        }
        else
        {
            // Jika tidak ada input, tetap idle
            animator.SetBool("stat_jalan", false);
        }
    }
}

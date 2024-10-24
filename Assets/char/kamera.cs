using UnityEngine;

public class kamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Sensitivitas mouse

    private float xRotation = 0f; // Rotasi vertikal (pitch)

    void Start()
    {
        // Mengunci kursor di tengah layar dan menyembunyikannya
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Ambil input dari mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotasi vertikal kamera (atas-bawah)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Batasi sudut pandang vertikal agar tidak terlalu jauh ke atas/bawah

        // Rotasi horizontal kamera (kiri-kanan)
        transform.Rotate(Vector3.up * mouseX);

        // Terapkan rotasi vertikal
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f);
    }
}

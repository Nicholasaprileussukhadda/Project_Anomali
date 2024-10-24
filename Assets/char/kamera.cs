using UnityEngine;

public class kamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Sensitivitas mouse
    public Transform characterBody; // Referensi ke badan karakter (untuk rotasi horizontal)

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

        // Terapkan rotasi vertikal ke kamera (pitch)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Terapkan rotasi horizontal ke badan karakter (yaw), menggeser sesuai dengan input mouse X
        characterBody.Rotate(Vector3.up * mouseX); // Rotasi horizontal karakter mengikuti gerakan mouse kiri/kanan
    }
}

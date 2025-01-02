using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public GameObject winPanel; // Panel yang berisi UI kemenangan

    private void Start()
    {
        winPanel.SetActive(false); // Sembunyikan panel kemenangan saat game dimulai
        Cursor.visible = false; // Sembunyikan cursor saat game dimulai
        Cursor.lockState = CursorLockMode.Locked; // Kunci cursor di tengah layar
    }

    public void ShowWinUI()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true); // Tampilkan panel kemenangan
            Time.timeScale = 0; // Hentikan semua aktivitas game
            Cursor.visible = true; // Tampilkan cursor
            Cursor.lockState = CursorLockMode.None; // Bebaskan cursor dari kunci tengah layar
            Debug.Log("Panel kemenangan diaktifkan, game dihentikan, dan cursor ditampilkan.");
        }
        else
        {
            Debug.LogError("winPanel tidak diatur di Inspector.");
        }
    }

    public void RestartGame()
    {
        GameController.Instance.ResetScore(); // Reset skor saat game dimulai ulang
        GameController.Instance.ResetMapIndex(); // Reset mapIndex saat game dimulai ulang
        Time.timeScale = 1; // Kembalikan aktivitas game sebelum memuat ulang scene
        Cursor.visible = false; // Sembunyikan cursor saat game kembali
        Cursor.lockState = CursorLockMode.Locked; // Kunci cursor di tengah layar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Muat ulang scene saat ini
    }

    public void GoToMainMenu()
    {
        GameController.Instance.ResetScore(); // Reset skor saat kembali ke menu utama
        GameController.Instance.ResetMapIndex(); // Reset mapIndex saat kembali ke menu utama
        Time.timeScale = 1; // Kembalikan aktivitas game sebelum memuat scene utama
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit(); // Keluar dari aplikasi
    }
}


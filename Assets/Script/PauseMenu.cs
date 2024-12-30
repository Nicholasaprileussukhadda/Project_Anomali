using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;  // UI Pause Menu
    public bool isPaused;         // Status apakah game sedang pause
    public GameController gameController; // Referensi ke GameController untuk memeriksa status game

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);  // Pastikan pause menu disembunyikan saat mulai
        Cursor.visible = false;      // Sembunyikan cursor saat game dimulai
        Cursor.lockState = CursorLockMode.Locked;  // Kunci cursor di tengah layar
    }

    // Update is called once per frame
    void Update()
    {
        // Hanya izinkan pause jika game dalam status Playing dan bukan Won
        if (gameController.currentState == GameController.GameState.Playing && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;  // Hentikan semua aktivitas game
        pauseMenu.SetActive(true);  // Tampilkan pause menu
        isPaused = true;  // Tandai game dalam status pause
        Cursor.visible = true;  // Tampilkan cursor
        Cursor.lockState = CursorLockMode.None;  // Bebaskan cursor
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;  // Lanjutkan waktu
        pauseMenu.SetActive(false);  // Sembunyikan pause menu
        isPaused = false;  // Tandai game dalam status play
        Cursor.visible = false;  // Sembunyikan cursor
        Cursor.lockState = CursorLockMode.Locked;  // Kunci cursor di tengah layar
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1;  // Pastikan waktu berjalan normal sebelum pindah ke main menu
        SceneManager.LoadScene("MainMenu");  // Pindah ke scene MainMenu
    }

    public void QuitGame()
    {
        Application.Quit();  // Keluar dari aplikasi
    }
}

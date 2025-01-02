using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int mapIndex = 1;  // Pastikan mapIndex diatur hanya sekali
    public int scoreCounter = 0;
    public WinMenu winUIController; // Tambahkan referensi ke WinMenu

    // Enum untuk status game
    public enum GameState
    {
        Playing,
        Paused,
        Won
    }
    public GameState currentState = GameState.Playing; // Status permainan saat ini

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Pastikan GameController tidak dihancurkan antara scene
            SceneManager.sceneLoaded += OnSceneLoaded; // Tambahkan event handler untuk sceneLoaded
            Debug.Log("GameController dibuat, mapIndex awal: " + mapIndex);
        }
        else
        {
            Debug.Log("Duplikat GameController ditemukan, instance dihancurkan.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("Game dimulai, mapIndex awal: " + mapIndex);
        ResetScore(); // Reset skor saat game dimulai
        ResetMapIndex(); // Reset mapIndex saat game dimulai
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari WinMenu di scene yang baru dimuat
        winUIController = FindObjectOfType<WinMenu>();
        if (winUIController == null)
        {
            Debug.LogError("winUIController tidak ditemukan di scene.");
        }
        else
        {
            Debug.Log("winUIController berhasil ditemukan dan diatur.");
        }
    }

    public void IncrementMapIndex()
    {
        mapIndex++;
        Debug.Log("mapIndex di-*increment* ke: " + mapIndex);
    }

    public void IncrementScore()
    {
        scoreCounter++;
        Debug.Log("Score di-*increment* ke: " + scoreCounter);
        if (scoreCounter >= 5)
        {
            EndGame(); // Panggil fungsi untuk mengakhiri permainan ketika skor mencapai 5
        }
    }

    private void EndGame()
    {
        currentState = GameState.Won; // Status permainan berubah menjadi "Won"
        Debug.Log("Permainan selesai!");
        if (winUIController != null)
        {
            winUIController.ShowWinUI(); // Tampilkan UI kemenangan
            Debug.Log("UI kemenangan ditampilkan.");
        }
        else
        {
            Debug.LogError("winUIController tidak diatur di Inspector.");
        }
    }

    // Fungsi untuk mengatur status game kembali ke "Playing"
    public void ResumeGame()
    {
        currentState = GameState.Playing;
    }

    // Fungsi untuk mengatur status game ke "Paused"
    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0; // Hentikan semua aktivitas game
        }
    }

    // Fungsi untuk melanjutkan game setelah pause
    public void UnpauseGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1; // Lanjutkan aktivitas game
        }
    }

    // Fungsi untuk mereset skor
    public void ResetScore()
    {
        scoreCounter = 0;
        Debug.Log("Score di-reset ke: " + scoreCounter);
    }

    // Fungsi untuk mereset mapIndex
    public void ResetMapIndex()
    {
        mapIndex = 1;
        Debug.Log("mapIndex di-reset ke: " + mapIndex);
    }
}

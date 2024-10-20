using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Jika kamu ingin menampilkan score dengan UI

public class Sc_Map : MonoBehaviour
{
    [SerializeField] private Transform spawnLocationYes;  // Lokasi di mana map akan di-spawn ketika memilih Yes
    [SerializeField] private Transform spawnLocationNo;   // Lokasi di mana map akan di-spawn ketika memilih No
    [SerializeField] private GameObject mapPrefab;        // Prefab map yang akan di-spawn
    [SerializeField] private List<GameObject> objectsInMap; // Daftar objek di dalam map yang mungkin mengalami anomali
    [SerializeField] private GameObject player;           // Referensi ke pemain, pastikan ini adalah GameObject
    [SerializeField] [Range(0, 100)] private int anomalyChance = 50; // Persentase peluang anomali terjadi
    [SerializeField] private Text scoreText;              // UI Text untuk menampilkan skor, opsional

    private GameObject currentMapInstance;  // Referensi ke map yang di-spawn sebelumnya
    private bool mapSpawned = false;        // Untuk memastikan map hanya di-spawn sekali per trigger
    private bool isAnomalyPresent = false;  // Apakah anomali diterapkan pada map ini
    private int scoreCounter = 0;           // Counter untuk score
    private Transform nextSpawnLocation;    // Lokasi spawn berikutnya, bergantung pada pintu Yes/No

    private void Start()
    {
        UpdateScoreUI(); // Inisialisasi UI score, jika digunakan
        Debug.Log("Game dimulai, siap untuk spawn map.");

        // Debugging untuk memeriksa apakah referensi Player sudah benar
        if (player != null)
        {
            Debug.Log("Player berhasil terhubung dengan script Sc_Map.");
        }
        else
        {
            Debug.LogError("Player tidak terhubung. Pastikan kamu drag objek Player yang benar.");
        }

        // Cek posisi awal dari spawnLocationYes dan spawnLocationNo
        if (spawnLocationYes != null)
        {
            Debug.Log("Posisi spawnLocationYes: " + spawnLocationYes.position + ", Rotasi: " + spawnLocationYes.rotation);
        }
        else
        {
            Debug.LogError("spawnLocationYes belum di-*assign*.");
        }

        if (spawnLocationNo != null)
        {
            Debug.Log("Posisi spawnLocationNo: " + spawnLocationNo.position + ", Rotasi: " + spawnLocationNo.rotation);
        }
        else
        {
            Debug.LogError("spawnLocationNo belum di-*assign*.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player mencoba memasuki area trigger");
        if (other.CompareTag("Player") && !mapSpawned)
        {
            Debug.Log("Player masuk ke trigger area.");

            if (nextSpawnLocation == null)
            {
                Debug.LogError("nextSpawnLocation tidak di-set!");
                return;  // Keluar dari fungsi jika nextSpawnLocation belum di-set
            }

            Debug.Log("Memulai spawn map di lokasi: " + nextSpawnLocation.position);
            Debug.Log("Spawn prefab: " + mapPrefab.name);

            // Spawn map baru di lokasi yang telah ditentukan (nextSpawnLocation)
            currentMapInstance = Instantiate(mapPrefab, nextSpawnLocation.position, nextSpawnLocation.rotation);

            // Verifikasi apakah map benar-benar di-*spawn*
            if (currentMapInstance != null)
            {
                Debug.Log("Map berhasil di-*spawn* di lokasi: " + nextSpawnLocation.position);
                Debug.Log("Posisi map: " + currentMapInstance.transform.position);
                Debug.Log("Ukuran map: " + currentMapInstance.transform.localScale);
            }
            else
            {
                Debug.LogError("Map gagal di-*spawn*.");
            }

            mapSpawned = true;  // Pastikan map hanya di-spawn sekali
        }
    }

    // Panggil ini saat player masuk pintu Yes
    public void PlayerChoseYes()
    {
        nextSpawnLocation = spawnLocationYes; // Tentukan lokasi spawn di belakang pintu Yes
        Debug.Log("Player memilih Yes. Map berikutnya akan muncul di: " + nextSpawnLocation.position);
        Debug.Log("Rotasi spawn lokasi Yes: " + nextSpawnLocation.rotation);
        if (isAnomalyPresent)
        {
            CorrectChoice(); // Pemain benar, karena ada anomali dan memilih Yes
        }
        else
        {
            WrongChoice(); // Pemain salah, tidak ada anomali, tetapi memilih Yes
        }
    }

    // Panggil ini saat player masuk pintu No
    public void PlayerChoseNo()
    {
        nextSpawnLocation = spawnLocationNo; // Tentukan lokasi spawn di belakang pintu No
        Debug.Log("Player memilih No. Map berikutnya akan muncul di: " + nextSpawnLocation.position);
        Debug.Log("Rotasi spawn lokasi No: " + nextSpawnLocation.rotation);
        if (!isAnomalyPresent)
        {
            CorrectChoice(); // Pemain benar, tidak ada anomali dan memilih No
        }
        else
        {
            WrongChoice(); // Pemain salah, ada anomali tetapi memilih No
        }
    }

    private void CorrectChoice()
    {
        scoreCounter++;  // Tambahkan score
        UpdateScoreUI(); // Update tampilan score
        Debug.Log("Pilihan benar! Skor sekarang: " + scoreCounter);
        SpawnNewMap();   // Spawn map baru
    }

    private void WrongChoice()
    {
        scoreCounter = 0; // Reset score ke 0
        UpdateScoreUI();  // Update tampilan score
        Debug.Log("Pilihan salah! Skor di-reset.");
        SpawnNewMap();    // Spawn map baru
    }

    private void SpawnNewMap()
    {
        // Reset kondisi untuk map baru
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance); // Hapus map lama
        }
        mapSpawned = false; // Reset agar map bisa di-*spawn* lagi
        isAnomalyPresent = false;

        Debug.Log("Map baru siap untuk di-*spawn* di lokasi: " + nextSpawnLocation.position);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreCounter;
        }
    }
}

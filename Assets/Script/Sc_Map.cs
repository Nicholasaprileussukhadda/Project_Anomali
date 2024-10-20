using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Jika ingin menampilkan skor dengan UI

public class Sc_Map : MonoBehaviour
{
    [SerializeField] private Transform spawnLocationYes;  // Lokasi untuk spawn jika memilih Yes
    [SerializeField] private Transform spawnLocationNo;   // Lokasi untuk spawn jika memilih No
    [SerializeField] private GameObject mapPrefab;        // Prefab map yang akan di-spawn
    [SerializeField] private List<GameObject> objectsInMap; // Daftar objek di map yang mungkin mengalami anomali
    [SerializeField] private GameObject player;           // Referensi ke pemain
    [SerializeField] [Range(0, 100)] private int anomalyChance = 50; // Persentase peluang anomali terjadi
    [SerializeField] private Text scoreText;              // UI Text untuk menampilkan skor

    private GameObject currentMapInstance;  // Referensi ke map yang di-spawn sebelumnya
    private bool isAnomalyPresent = false;  // Apakah anomali diterapkan pada map ini
    private int scoreCounter = 0;           // Counter untuk skor
    private Transform nextSpawnLocation;    // Lokasi spawn berikutnya, tergantung pilihan Yes/No
    private bool mapSpawned = false;        // Apakah map sudah di-spawn

    private void Start()
    {
        Debug.Log("Game dimulai, siap untuk spawn map.");

        // Cek referensi
        if (spawnLocationYes == null || spawnLocationNo == null)
        {
            Debug.LogError("Spawn location Yes atau No belum di-assign!");
        }

        if (player == null)
        {
            Debug.LogError("Player belum di-assign!");
        }

        UpdateScoreUI(); // Inisialisasi UI skor
        isAnomalyPresent = false; // Map pertama tidak ada anomali
        mapSpawned = false; // Map pertama belum di-spawn
    }

    public void PlayerChoseYes()
    {
        if (!mapSpawned) // Pastikan map belum di-spawn
        {
            nextSpawnLocation = spawnLocationYes; // Tentukan lokasi spawn di pintu Yes
            Debug.Log("Player memilih Yes. Map berikutnya akan muncul di: " + nextSpawnLocation.position);
            ProcessMap(); // Proses map baru dengan kemungkinan anomali
        }
        else
        {
            Debug.LogWarning("Map sudah di-spawn. Menunggu map baru.");
        }
    }

    public void PlayerChoseNo()
    {
        if (!mapSpawned) // Pastikan map belum di-spawn
        {
            nextSpawnLocation = spawnLocationNo; // Tentukan lokasi spawn di pintu No
            Debug.Log("Player memilih No. Map berikutnya akan muncul di: " + nextSpawnLocation.position);
            ProcessMap(); // Proses map baru dengan kemungkinan anomali
        }
        else
        {
            Debug.LogWarning("Map sudah di-spawn. Menunggu map baru.");
        }
    }

    private void ProcessMap()
    {
        // Spawn map baru di lokasi yang ditentukan
        currentMapInstance = Instantiate(mapPrefab, nextSpawnLocation.position, nextSpawnLocation.rotation);

        // Tentukan apakah akan ada anomali di map baru
        isAnomalyPresent = Random.Range(0, 100) < anomalyChance;
        Debug.Log("Anomali hadir: " + isAnomalyPresent);

        // Terapkan anomali jika perlu
        if (isAnomalyPresent)
        {
            ApplyAnomaly();
        }

        mapSpawned = true; // Tandai bahwa map sudah di-spawn
    }

    // Terapkan anomali pada objek di dalam map
    private void ApplyAnomaly()
    {
        if (objectsInMap.Count == 0)
        {
            Debug.LogWarning("Tidak ada objek di dalam map untuk menerima anomali.");
            return;
        }

        int randomIndex = Random.Range(0, objectsInMap.Count);
        GameObject selectedObject = objectsInMap[randomIndex];

        int anomalyType = Random.Range(0, 3); // 0 = Hilang, 1 = Duplikat, 2 = Pindah
        Debug.Log("Anomali diterapkan pada objek: " + selectedObject.name + ", Tipe: " + anomalyType);

        if (anomalyType == 0) // Anomali: Hilangkan objek
        {
            selectedObject.SetActive(false);
        }
        else if (anomalyType == 1) // Anomali: Duplikat objek
        {
            Vector3 duplicatePosition = selectedObject.transform.position + new Vector3(1, 0, 0);
            Instantiate(selectedObject, duplicatePosition, selectedObject.transform.rotation);
        }
        else if (anomalyType == 2) // Anomali: Pindah objek
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            selectedObject.transform.position += randomOffset;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreCounter;
        }
    }

    public void ResetMapSpawn()
    {
        mapSpawned = false; // Digunakan untuk mereset status map setelah pilihan dibuat
    }
}

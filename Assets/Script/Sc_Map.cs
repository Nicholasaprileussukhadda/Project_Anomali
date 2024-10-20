using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Jika ingin menampilkan skor dengan UI

public class Sc_Map : MonoBehaviour
{
    [SerializeField] private Transform spawnLocationYes;  // Lokasi di mana map akan di-spawn ketika memilih Yes
    [SerializeField] private Transform spawnLocationNo;   // Lokasi di mana map akan di-spawn ketika memilih No
    [SerializeField] private GameObject mapPrefab;        // Prefab map yang akan di-spawn
    [SerializeField] private List<GameObject> objectsInMap; // Daftar objek di dalam map yang mungkin mengalami anomali
    [SerializeField] private GameObject player;           // Referensi ke pemain
    [SerializeField] [Range(0, 100)] private int anomalyChance = 50; // Persentase peluang anomali terjadi
    [SerializeField] private Text scoreText;              // UI Text untuk menampilkan skor

    private GameObject currentMapInstance;  // Referensi ke map yang di-spawn sebelumnya
    private bool isAnomalyPresent = false;  // Apakah anomali diterapkan pada map ini
    private int scoreCounter = 0;           // Counter untuk score
    private Transform nextSpawnLocation;    // Lokasi spawn berikutnya, bergantung pada pintu Yes/No

    private void Start()
    {
        Debug.Log("Game dimulai, siap untuk spawn map.");

        // Cek apakah referensi spawnLocation sudah benar
        if (spawnLocationYes != null)
        {
            Debug.Log("Posisi spawnLocationYes: " + spawnLocationYes.position);
        }
        else
        {
            Debug.LogError("spawnLocationYes belum di-assign!");
        }

        if (spawnLocationNo != null)
        {
            Debug.Log("Posisi spawnLocationNo: " + spawnLocationNo.position);
        }
        else
        {
            Debug.LogError("spawnLocationNo belum di-assign!");
        }

        if (player != null)
        {
            Debug.Log("Player berhasil terhubung dengan script Sc_Map.");
        }
        else
        {
            Debug.LogError("Player tidak terhubung. Pastikan objek Player sudah di-drag ke script.");
        }

        UpdateScoreUI(); // Inisialisasi UI skor, jika digunakan
        isAnomalyPresent = false; // Map pertama tidak memiliki anomali
    }

    // Metode yang dipanggil saat player memilih Yes
    public void PlayerChoseYes()
    {
        nextSpawnLocation = spawnLocationYes; // Tentukan lokasi spawn di belakang pintu Yes
        Debug.Log("Player memilih Yes. Map berikutnya akan muncul di: " + nextSpawnLocation.position);
        SpawnNewMap(); // Spawn map baru setelah memilih Yes
    }

    // Metode yang dipanggil saat player memilih No
    public void PlayerChoseNo()
    {
        nextSpawnLocation = spawnLocationNo; // Tentukan lokasi spawn di belakang pintu No
        Debug.Log("Player memilih No. Map berikutnya akan muncul di: " + nextSpawnLocation.position);
        SpawnNewMap(); // Spawn map baru setelah memilih No
    }

    // Spawn map baru di lokasi yang telah ditentukan
    private void SpawnNewMap()
    {
        Debug.Log("Memulai spawn map di lokasi: " + nextSpawnLocation.position);

        // Spawn map baru di lokasi yang telah ditentukan (nextSpawnLocation)
        GameObject newMapInstance = Instantiate(mapPrefab, nextSpawnLocation.position, nextSpawnLocation.rotation);
        currentMapInstance = newMapInstance; // Simpan referensi ke map yang baru

        // Pastikan semua child objects aktif (terutama interior)
        foreach (Transform child in newMapInstance.transform)
        {
            child.gameObject.SetActive(true); // Aktifkan setiap child object
            Debug.Log("Objek: " + child.name + " diaktifkan, posisi: " + child.position + ", skala: " + child.localScale);
        }

        // Tentukan apakah akan ada anomali di map baru
        isAnomalyPresent = Random.Range(0, 100) < anomalyChance;

        // Jika ada anomali, terapkan anomali
        if (isAnomalyPresent)
        {
            ApplyAnomaly();
        }

        Debug.Log("Map berhasil di-*spawn* di lokasi: " + nextSpawnLocation.position);
    }

    // Terapkan anomali pada objek di dalam map
    private void ApplyAnomaly()
    {
        if (objectsInMap.Count == 0)
        {
            Debug.LogWarning("Tidak ada objek di dalam map untuk menerima anomali.");
            return;
        }

        Debug.Log("Jumlah objek dalam objectsInMap: " + objectsInMap.Count);

        int randomIndex = Random.Range(0, objectsInMap.Count);
        GameObject selectedObject = objectsInMap[randomIndex];

        Debug.Log("Objek terpilih untuk anomali: " + selectedObject.name + " pada posisi: " + selectedObject.transform.position);

        int anomalyType = Random.Range(0, 3); // 0 = Hilang, 1 = Duplikat, 2 = Pindah
        Debug.Log("Anomali diterapkan pada objek: " + selectedObject.name + ", Tipe: " + anomalyType);

        if (anomalyType == 0) // Anomali: Hilangkan objek
        {
            selectedObject.SetActive(false);
            Debug.Log("Objek " + selectedObject.name + " dihilangkan.");
        }
        else if (anomalyType == 1) // Anomali: Duplikat objek
        {
            Vector3 duplicatePosition = selectedObject.transform.position + new Vector3(1, 0, 0);
            GameObject duplicate = Instantiate(selectedObject, duplicatePosition, selectedObject.transform.rotation);
            Debug.Log("Objek " + selectedObject.name + " diduplikasi di posisi: " + duplicatePosition);
        }
        else if (anomalyType == 2) // Anomali: Pindah objek
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            selectedObject.transform.position += randomOffset;
            Debug.Log("Objek " + selectedObject.name + " dipindah ke posisi: " + selectedObject.transform.position);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreCounter;
        }
    }
}

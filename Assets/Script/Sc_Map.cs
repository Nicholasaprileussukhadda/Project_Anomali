using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Map : MonoBehaviour
{
    [SerializeField] private Transform spawnLocationYes;
    [SerializeField] private Transform spawnLocationNo;
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private List<GameObject> objectsInMap; // Daftar objek di dalam map
    [SerializeField] private GameObject player;
    [SerializeField][Range(0, 100)] private int anomalyChance = 50;
    [SerializeField] private Vector3 spawnOffset;

    private GameObject currentMapInstance;
    private bool isAnomalyPresent = false; // Apakah ada anomali di map saat ini
    private Transform nextSpawnLocation;

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();  // Menyimpan posisi awal objek
    private Dictionary<GameObject, bool> originalActiveStates = new Dictionary<GameObject, bool>();  // Menyimpan state awal (aktif/tidak aktif)

    public Text scoreText;

    private void Start()
    {
        // Set initial spawn location ke spawnLocationYes atau spawnLocationNo
        nextSpawnLocation = spawnLocationNo != null ? spawnLocationNo : spawnLocationYes;
        Debug.Log("Memulai game, mapIndex diatur ke: " + GameController.Instance.mapIndex);
    }

    private void HandleAnomaly()
    {
        if (isAnomalyPresent && objectsInMap.Count > 0)
        {
            // Pilih objek secara acak dari `objectsInMap`
            int randomIndex = Random.Range(0, objectsInMap.Count);
            GameObject chosenObject = objectsInMap[randomIndex];

            // Pilih jenis anomali secara acak (0 = sembunyikan, 1 = geser, 2 = duplikat)
            int anomalyType = Random.Range(0, 3);

            switch (anomalyType)
            {
                case 0: // Sembunyikan objek alih-alih menghapusnya
                    Debug.Log("Anomali: Menyembunyikan objek " + chosenObject.name);
                    chosenObject.SetActive(false); // Sembunyikan objek
                    break;

                case 1: // Geser objek ke posisi baru
                    Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                    Debug.Log("Anomali: Menggeser objek " + chosenObject.name + " ke lokasi baru.");
                    chosenObject.transform.position += randomOffset;
                    break;

                case 2: // Duplikat objek dan geser duplikat ke posisi baru
                    Vector3 duplicateOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                    GameObject duplicateObject = Instantiate(chosenObject, chosenObject.transform.position + duplicateOffset, chosenObject.transform.rotation);
                    objectsInMap.Add(duplicateObject); // Tambahkan duplikat ke daftar objek
                    Debug.Log("Anomali: Menduplicate objek " + chosenObject.name + " dan memindahkan duplikat.");
                    break;
            }
        }
        else
        {
            Debug.Log("Tidak ada anomali di map atau daftar objectsInMap kosong.");
        }
    }

    private void ResetAnomalies()
    {
        // Mengembalikan semua objek yang terkena anomali ke posisi dan state awal
        foreach (var item in originalPositions)
        {
            item.Key.transform.position = item.Value;  // Kembalikan posisi ke posisi awal
        }

        foreach (var item in originalActiveStates)
        {
            item.Key.SetActive(true);  // Pastikan semua objek aktif kembali
        }

        originalPositions.Clear();  // Kosongkan daftar setelah reset
        originalActiveStates.Clear();
        Debug.Log("Semua anomali telah di-reset.");
    }

    // Spawn map baru setelah player membuat pilihan
    private void SpawnNewMap()
    {
        // Reset anomali sebelum map baru di-*spawn*
        ResetAnomalies();

        // Map hanya di-*spawn* sekali per tebakan
        GameController.Instance.IncrementMapIndex();  // Increment map index untuk setiap map baru
        Debug.Log("Map ke-" + GameController.Instance.mapIndex + " akan di-*spawn* di lokasi: " + nextSpawnLocation.position);

        // Hancurkan map sebelumnya, jika ada
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
            Debug.Log("Map sebelumnya dihancurkan.");
        }

        // Spawn map baru di nextSpawnLocation
        GameObject newMapInstance = Instantiate(mapPrefab, nextSpawnLocation.position + spawnOffset, nextSpawnLocation.rotation);
        currentMapInstance = newMapInstance;
        Debug.Log("Map baru berhasil di-*spawn* di lokasi: " + nextSpawnLocation.position);

        // Aktifkan objek dalam map baru dan isi daftar `objectsInMap` hanya dengan objek dari "anomalyItems"
        objectsInMap.Clear();
        Transform anomalyItems = newMapInstance.transform.Find("anomalyItems");  // Cari child anomalyItems dalam currentMapInstance
        if (anomalyItems != null)
        {
            foreach (Transform child in anomalyItems)
            {
                objectsInMap.Add(child.gameObject); // Tambahkan objek di dalam anomalyItems ke daftar objectsInMap
                originalPositions[child.gameObject] = child.transform.position;  // Simpan posisi awal objek
                originalActiveStates[child.gameObject] = child.gameObject.activeSelf;  // Simpan state awal (aktif/tidak aktif)
            }
            Debug.Log("Daftar objectsInMap diisi dengan " + objectsInMap.Count + " objek dari anomalyItems.");
        }
        else
        {
            Debug.LogWarning("anomalyItems tidak ditemukan di map ini.");
        }

        // Tentukan apakah ada anomali di map baru
        if (GameController.Instance.mapIndex == 1)
        {
            isAnomalyPresent = false;  // Tidak ada anomali di map pertama
            Debug.Log("Map pertama, tidak ada anomali.");
        }
        else
        {
            isAnomalyPresent = Random.Range(0, 100) < anomalyChance;
            Debug.Log("Anomali di-*set* untuk map ke-" + GameController.Instance.mapIndex + ": " + isAnomalyPresent);
        }

        // Jalankan anomali jika ada
        HandleAnomaly();
    }

    public void PlayerChoseYes()
    {
        nextSpawnLocation = spawnLocationYes;  // Tentukan spawn location untuk pilihan Yes

        Debug.Log("Player memilih Yes. Status Anomali sebelum pengecekan: " + isAnomalyPresent);

        // Debugging tambahan untuk melihat apakah isAnomalyPresent benar-benar true
        if (isAnomalyPresent)
        {
            // Jika ada anomali, tebakan benar, tambah poin
            Debug.Log("Tebakan benar. Ada anomali di map ke-" + GameController.Instance.mapIndex + ". Menambah poin.");
            AddScore(1);
        }
        else
        {
            // Jika tidak ada anomali, tebakan salah, reset skor
            Debug.Log("Tebakan salah. Tidak ada anomali di map ke-" + GameController.Instance.mapIndex + ". Mereset poin.");
            ResetScore();
        }

        UpdateScoreText();
        SpawnNewMap();  // Spawn map setelah player memilih
    }

    public void PlayerChoseNo()
    {
        nextSpawnLocation = spawnLocationNo;  // Tentukan spawn location untuk pilihan No

        Debug.Log("Player memilih No. Status Anomali sebelum pengecekan: " + isAnomalyPresent);

        // Debugging tambahan untuk melihat apakah isAnomalyPresent benar-benar false
        if (!isAnomalyPresent)
        {
            // Jika tidak ada anomali, tebakan benar, tambah poin
            Debug.Log("Tebakan benar. Tidak ada anomali di map ke-" + GameController.Instance.mapIndex + ". Menambah poin.");
            AddScore(1);
        }
        else
        {
            // Jika ada anomali, tebakan salah, reset skor
            Debug.Log("Tebakan salah. Ada anomali di map ke-" + GameController.Instance.mapIndex + ". Mereset poin.");
            ResetScore();
        }

        UpdateScoreText();
        SpawnNewMap();  // Spawn map setelah player memilih
    }



    private void AddScore(int points)
    {
        GameController.Instance.scoreCounter += points;
        Debug.Log("Poin ditambahkan: " + points + ", Total skor saat ini: " + GameController.Instance.scoreCounter);
    }

    private void ResetScore()
    {
        GameController.Instance.scoreCounter = 0;
        Debug.Log("Tebakan salah. Skor direset ke 0.");
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + GameController.Instance.scoreCounter;
    }
}

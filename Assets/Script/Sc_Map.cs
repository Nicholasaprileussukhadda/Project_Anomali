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
    [SerializeField] [Range(0, 100)] private int anomalyChance = 50;
    [SerializeField] private Vector3 spawnOffset;
    
    private GameObject currentMapInstance;
    private bool isAnomalyPresent = false; // Apakah ada anomali di map saat ini
    private Transform nextSpawnLocation;

    public Text scoreText;

    private void Start()
    {
        // Set initial spawn location ke spawnLocationYes atau spawnLocationNo
        nextSpawnLocation = spawnLocationNo != null ? spawnLocationNo : spawnLocationYes;
        Debug.Log("Memulai game, mapIndex diatur ke: " + GameController.Instance.mapIndex);
    }

    private void HandleAnomaly()
    {
        if (isAnomalyPresent)
        {
            // Pilih objek secara acak dari daftar `objectsInMap`
            int randomIndex = Random.Range(0, objectsInMap.Count);
            GameObject chosenObject = objectsInMap[randomIndex];

            // Pilih jenis anomali secara acak (0 = hilang, 1 = geser, 2 = duplikat)
            int anomalyType = Random.Range(0, 3);

            switch (anomalyType)
            {
                case 0: // Hapus objek
                    Debug.Log("Anomali: Menghapus objek " + chosenObject.name);
                    Destroy(chosenObject);
                    break;

                case 1: // Geser objek ke posisi baru
                    Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                    Debug.Log("Anomali: Menggeser objek " + chosenObject.name + " ke lokasi baru.");
                    chosenObject.transform.position += randomOffset;
                    break;

                case 2: // Duplikat objek dan geser duplikat ke posisi baru
                    Vector3 duplicateOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                    GameObject duplicateObject = Instantiate(chosenObject, chosenObject.transform.position + duplicateOffset, chosenObject.transform.rotation);
                    Debug.Log("Anomali: Menduplicate objek " + chosenObject.name + " dan memindahkan duplikat.");
                    break;
            }
        }
        else
        {
            Debug.Log("Tidak ada anomali di map.");
        }
    }

    // Spawn map baru setelah player membuat pilihan
    private void SpawnNewMap()
    {
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

        // Aktifkan objek dalam map baru
        foreach (Transform child in newMapInstance.transform)
        {
            child.gameObject.SetActive(true);
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

        // Tambahkan log setelah map di-*spawn* untuk memastikan `isAnomalyPresent` tidak berubah
        HandleAnomaly();
    }

    public void PlayerChoseYes()
    {
        nextSpawnLocation = spawnLocationYes;  // Tentukan spawn location untuk pilihan Yes

        Debug.Log("Player memilih Yes. Status Anomali sebelum pengecekan: " + isAnomalyPresent);

        if (isAnomalyPresent)
        {
            Debug.Log("Tebakan benar. Ada anomali di map ke-" + GameController.Instance.mapIndex + ". Menambah poin.");
            AddScore(1);
        }
        else
        {
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

        if (!isAnomalyPresent)
        {
            Debug.Log("Tebakan benar. Tidak ada anomali di map ke-" + GameController.Instance.mapIndex + ". Menambah poin.");
            AddScore(1);
        }
        else
        {
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

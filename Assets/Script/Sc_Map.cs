using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Map : MonoBehaviour
{
    [SerializeField] private Transform spawnLocationYes;
    [SerializeField] private Transform spawnLocationNo;
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private List<GameObject> objectsInMap;
    [SerializeField] private GameObject player;
    [SerializeField] [Range(0, 100)] private int anomalyChance = 50;
    [SerializeField] private Sc_triggerDoorController doorController;
    [SerializeField] private Sc_triggerDoorYesController doorYesController;
    [SerializeField] private Sc_triggerDoorNoController doorNoController;
    [SerializeField] private Vector3 spawnOffset;

    private GameObject currentMapInstance;
    private bool isAnomalyPresent = false; // Apakah ada anomali di map saat ini
    private int scoreCounter = 0; // Variabel untuk menyimpan score
    private Transform nextSpawnLocation;
    private bool isFirstMap = true; // Variabel untuk menentukan apakah ini map pertama

    public Text scoreText;

    private void Start()
    {
        isAnomalyPresent = false;
    }

    public void PlayerChoseYes()
    {
        nextSpawnLocation = spawnLocationYes;
        
        // Cek apakah tebakan player benar
        if (isAnomalyPresent)
        {
            Debug.Log("Player benar menebak anomali. Menambah poin.");
            AddScore(1); // Benar, ada anomali, jadi tambah 1 poin
        }
        else
        {
            Debug.Log("Player salah menebak anomali. Mereset poin.");
            ResetScore(); // Salah, tidak ada anomali, jadi score di-reset ke 0
        }
        UpdateScoreText();

        SpawnNewMap();
    }

    public void PlayerChoseNo()
    {
        nextSpawnLocation = spawnLocationNo;        
        // Cek apakah tebakan player benar
        if (!isAnomalyPresent)
        {
            Debug.Log("Player benar menebak tidak ada anomali. Menambah poin.");
            AddScore(1); // Benar, tidak ada anomali, jadi tambah 1 poin
        }
        else
        {
            Debug.Log("Player salah menebak tidak ada anomali. Mereset poin.");
            ResetScore(); // Salah, ada anomali, jadi score di-reset ke 0
        }

        SpawnNewMap();
    }private void SpawnNewMap()
{
    Debug.Log("Memulai spawn map di lokasi: " + nextSpawnLocation.position);

    // Spawn map baru dengan offset
    GameObject newMapInstance = Instantiate(mapPrefab, nextSpawnLocation.position + spawnOffset, nextSpawnLocation.rotation);
    currentMapInstance = newMapInstance;

    foreach (Transform child in newMapInstance.transform)
    {
        child.gameObject.SetActive(true);
        Debug.Log("Objek: " + child.name + " diaktifkan.");
    }

    // Aktifkan kembali semua trigger di dalam Map(Clone)
    Sc_triggerDoorController[] doorTriggers = newMapInstance.GetComponentsInChildren<Sc_triggerDoorController>();
    foreach (Sc_triggerDoorController trigger in doorTriggers)
    {
        trigger.ResetTrigger();
        Debug.Log("Trigger diaktifkan untuk: " + trigger.name);
    }

    Sc_triggerDoorYesController[] doorYesTriggers = newMapInstance.GetComponentsInChildren<Sc_triggerDoorYesController>();
    foreach (Sc_triggerDoorYesController trigger in doorYesTriggers)
    {
        trigger.ResetTrigger();
        Debug.Log("Trigger Yes diaktifkan untuk: " + trigger.name);
    }

    Sc_triggerDoorNoController[] doorNoTriggers = newMapInstance.GetComponentsInChildren<Sc_triggerDoorNoController>();
    foreach (Sc_triggerDoorNoController trigger in doorNoTriggers)
    {
        trigger.ResetTrigger(); // Pastikan openTrigger diaktifkan dan closeTrigger di-reset
        Debug.Log("Trigger No diaktifkan untuk: " + trigger.name);
    }

    if (isFirstMap)
    {
        isAnomalyPresent = false;
        isFirstMap = false;
        Debug.Log("Map pertama, tidak ada anomali.");
    }
    else
    {
        isAnomalyPresent = Random.Range(0, 100) < anomalyChance;
        Debug.Log("Apakah ada anomali? " + isAnomalyPresent);
    }
    Debug.Log("Map berhasil di-*spawn* di lokasi: " + nextSpawnLocation.position);
}



    // Fungsi untuk menambahkan poin dan menampilkan di Debug.Log
    private void AddScore(int points)
    {
        scoreCounter += points; // Tambahkan poin ke total score
        Debug.Log("Poin ditambahkan: " + points + ", Total skor saat ini: " + scoreCounter); // Log score ke Debug
    }

    // Fungsi untuk me-*reset* skor ke 0
    private void ResetScore()
    {
        scoreCounter = 0; // Reset score ke 0
        Debug.Log("Tebakan salah. Skor direset ke 0.");
    }

    private void UpdateScoreText(){
        scoreText.text = "Score: " + scoreCounter;
    }
}

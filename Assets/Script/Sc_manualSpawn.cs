using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_manualSpawn : MonoBehaviour
{
    public GameObject mapPrefab; // Prefab yang akan di-spawn
    public Transform spawnLocationYes; // Lokasi spawn untuk Player yang memilih "Yes"
    public Transform spawnLocationNo;  // Lokasi spawn untuk Player yang memilih "No"
    private GameObject manualMapInstance;

    void Start()
    {
        // Map spawn di Start
        GameObject spawnedMap = Instantiate(mapPrefab, spawnLocationYes.position, spawnLocationYes.rotation);
        if (spawnedMap != null)
        {
            Debug.Log("Manual spawn berhasil di lokasi: " + spawnLocationYes.position);
        }
        else
        {
            Debug.LogError("Manual spawn gagal.");
        }
    }

    void Update()
    {
        // Menekan tombol M akan memanggil manual spawn
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            Debug.Log("Manual spawn dijalankan.");

            // Spawn map di lokasi Yes
            manualMapInstance = Instantiate(mapPrefab, spawnLocationYes.position, spawnLocationYes.rotation);
            
            // Debugging untuk memastikan posisi dan skala map
            Debug.Log("Posisi manual spawn: " + manualMapInstance.transform.position);
            Debug.Log("Skala manual spawn: " + manualMapInstance.transform.localScale);
        }
    }
}

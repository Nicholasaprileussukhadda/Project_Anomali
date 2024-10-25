using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int mapIndex = 1;  // Pastikan mapIndex diatur hanya sekali
    public int scoreCounter = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Pastikan GameController tidak dihancurkan antara scene
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
    }

    public void IncrementMapIndex()
    {
        mapIndex++;
        Debug.Log("mapIndex di-*increment* ke: " + mapIndex);
    }
}


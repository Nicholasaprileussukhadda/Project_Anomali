using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_clickToiletDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    private bool isOpen = false; // Menyimpan status pintu apakah terbuka atau tidak
    private bool playerInRange = false; // Menyimpan status apakah player berada di dalam trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Jika player masuk ke area trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Jika player keluar dari area trigger
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetMouseButtonDown(0)) // Jika player berada di trigger dan klik kiri mouse
        {
            if (!isOpen) // Jika pintu sedang tertutup, maka buka pintunya
            {
                myDoor.Play("doorToiletClickOpen", 0, 0.0f);
                isOpen = true;
            }
            else // Jika pintu sedang terbuka, maka tutup pintunya
            {
                myDoor.Play("doorToiletClickClose", 0, 0.0f);
                isOpen = false;
            }
        }
    }
}

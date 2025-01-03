using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_triggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private AudioSource openSound = null; // AudioSource untuk sound effect pintu buka
    [SerializeField] private AudioSource closeSound = null; // AudioSource untuk sound effect pintu tutup
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
                myDoor.Play("doorTriggerOpen", 0, 0.0f);
                openSound.Play(); // Mainkan sound effect pintu buka
                isOpen = true;
            }
            else // Jika pintu sedang terbuka, maka tutup pintunya
            {
                myDoor.Play("doorTriggerClosed", 0, 0.0f);
                StartCoroutine(PlayCloseSoundWithDelay(0.5f)); // Mainkan sound effect pintu tutup dengan delay
                isOpen = false;
            }
        }
    }

    private IEnumerator PlayCloseSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        closeSound.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_triggerDoorNoController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider yang masuk: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player masuk trigger untuk pintu NO.");
            if (openTrigger)
            {
                myDoor.Play("doorNoTriggerOpen", 0, 0.0f);
                Debug.Log("Animasi doorNoTriggerOpen dijalankan.");
                gameObject.SetActive(false); // Disable trigger setelah animasi berjalan
            }
            if (closeTrigger)
            {
                myDoor.Play("doorNoTriggerClose", 0, 0.0f);
                Debug.Log("Animasi doorNoTriggerClose dijalankan.");
                gameObject.SetActive(false); // Disable trigger setelah animasi berjalan
            }
        }
    }

    // Fungsi reset trigger seperti di Sc_triggerDoorController
    public void ResetTrigger()
    {
        gameObject.SetActive(true); // Aktifkan kembali trigger pintu
        openTrigger = true; // Atur openTrigger ke true untuk membuka pintu
        closeTrigger = false; // Pastikan closeTrigger di-reset ke false

        // Pastikan Box Collider diaktifkan kembali
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = true; // Aktifkan kembali Box Collider
            Debug.Log("Box Collider diaktifkan untuk " + gameObject.name);
        }
    }
}

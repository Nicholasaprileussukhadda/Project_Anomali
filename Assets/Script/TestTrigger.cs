using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player masuk ke area trigger.");
        }
        else
        {
            Debug.Log("Objek lain masuk ke area trigger: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player keluar dari area trigger.");
        }
        else
        {
            Debug.Log("Objek lain keluar dari area trigger: " + other.name);
        }
    }
}

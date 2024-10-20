using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_doorDecision : MonoBehaviour
{
    public Sc_Map mapAnomalyGame; // Drag and drop the Sc_Map script object here
    public bool isYesDoor; // Set to true for Yes door, false for No door

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isYesDoor)
            {
                Debug.Log("Player memilih Yes.");
                mapAnomalyGame.PlayerChoseYes(); // Memilih Yes, map akan di-spawn di lokasi Yes
            }
            else
            {
                Debug.Log("Player memilih No.");
                mapAnomalyGame.PlayerChoseNo(); // Memilih No, map akan di-spawn di lokasi No
            }
        }
    }
}

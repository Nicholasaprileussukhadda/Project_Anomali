using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_doorDecision : MonoBehaviour
{
    public Sc_Map mapAnomalyGame; // Drag and drop the MapAnomalyGame script object here
    public bool isYesDoor; // Set to true for Yes door, false for No door

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isYesDoor)
            {
                mapAnomalyGame.PlayerChoseYes(); // Memilih Yes
            }
            else
            {
                mapAnomalyGame.PlayerChoseNo(); // Memilih No
            }
        }
    }
}

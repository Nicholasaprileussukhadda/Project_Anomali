using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepsc : MonoBehaviour
{
    public AudioSource footstepSound; // AudioSource untuk footstep
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            footstepSound.Play(); // Mainkan suara footstep
        }
        else
        {
            footstepSound.enabled = false; // Matikan suara footstep
        }
    }
}

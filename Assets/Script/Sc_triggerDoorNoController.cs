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
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.Play("doorNoTriggerOpen", 0, 0.0f);
                gameObject.SetActive(false);
            }
            if (closeTrigger)
            {
                myDoor.Play("doorNoTriggerClose", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}

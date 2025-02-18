using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CheckpointManager manager = FindObjectOfType<CheckpointManager>();
            if (manager != null)
            {
                manager.RegistrarPaso(gameObject);
            }
        }
    }
}

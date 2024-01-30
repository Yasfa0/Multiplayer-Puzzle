using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatExtras : MonoBehaviour
{
    List<PlacementSlot> placementSlots = new List<PlacementSlot>();

    private void Awake()
    {
        foreach (PlacementSlot slot in FindObjectsOfType<PlacementSlot>())
        {
            placementSlots.Add(slot);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<PlayerMovement>().AdjustSpeed(10);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<PlayerMovement>().AdjustSpeed(-10);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach(PlacementSlot slot in placementSlots) 
            {
                slot.ToggleMesh();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

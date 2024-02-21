using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNotes : MonoBehaviour
{
    [SerializeField] GameObject noteObj;

    private void Awake()
    {
        noteObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            noteObj.SetActive(!noteObj.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Swap to player mode
        }
    }
}

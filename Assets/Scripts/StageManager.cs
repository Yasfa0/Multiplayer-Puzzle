using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[SerializeField] GameObject placementSlotParent;
    [SerializeField] GameObject gridMat;
    [SerializeField] List<GameObject> itemPlacements = new List<GameObject>();
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<GameObject> CraneControls = new List<GameObject>();

    private void Start()
    {
        ChangeMode(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ChangeMode(false);
        }
    }

    public void ChangeMode(bool modeBool)
    {
        //placementSlotParent.SetActive(!modeBool);
        gridMat.SetActive(modeBool);

        foreach (GameObject item in itemPlacements) { 
            item.SetActive(!modeBool);
        }

        foreach (GameObject player in players)
        {
            player.SetActive(!modeBool);
        }

        foreach (GameObject crane in CraneControls)
        {
            crane.SetActive(modeBool);
        }

        ScoreManager.Instance.RefreshPlacementSlots();
    }

}

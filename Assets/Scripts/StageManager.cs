using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    //[SerializeField] GameObject placementSlotParent;
    [SerializeField] GameObject gridMat;
    //[SerializeField] List<GameObject> itemPlacements = new List<GameObject>();
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<GameObject> CraneControls = new List<GameObject>();

    [SerializeField] GameObject deleteGroup;
    [SerializeField] GameObject deleteBoxPrefab;

    bool currentMode = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != null)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ChangeMode(currentMode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeMode(currentMode);
        }
    }

    public void ChangeMode(bool modeBool)
    {
        //placementSlotParent.SetActive(!modeBool);
        gridMat.SetActive(modeBool);

        /*foreach (GameObject item in itemPlacements) { 
            item.SetActive(!modeBool);
        }*/

        foreach (GameObject player in players)
        {
            player.SetActive(!modeBool);
        }

        foreach (GameObject crane in CraneControls)
        {
            crane.SetActive(modeBool);
        }

        currentMode = !currentMode;
        ScoreManager.Instance.RefreshPlacementSlots();
    }

    public void AddDeleteBox(GameObject targetRoom)
    {
        GameObject temp = Instantiate(deleteBoxPrefab, deleteGroup.transform);
        temp.transform.SetParent(deleteGroup.transform,false);
        temp.GetComponent<PlacedRoomBox>().SetData(targetRoom,targetRoom.name);
    }

}

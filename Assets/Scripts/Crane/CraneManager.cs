using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneManager : MonoBehaviour
{
    public static CraneManager instance;

    [SerializeField] GameObject defaultBlock;
    [SerializeField] GameObject crane;
    GameObject currentBlock = null;
    CranePlacementSystem placementSystem;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if (instance != null)
        {
            Destroy(this);
        }

        placementSystem = GetComponent<CranePlacementSystem>();
        currentBlock = Instantiate(defaultBlock,crane.transform,false);
        currentBlock.transform.localPosition = new Vector3(0,-5,0);
    }

    public void ChangeCurrentBox(GameObject newBox)
    {
        if(currentBlock != null)
        {
            Destroy(currentBlock);
            currentBlock = null;
            currentBlock = Instantiate(newBox, crane.transform, false);
            currentBlock.transform.localPosition = new Vector3(0, -5, 0);

        }
        else
        {
            currentBlock = Instantiate(newBox, crane.transform, false);
            currentBlock.transform.localPosition = new Vector3(0, -5, 0);
        }
    }

    public void PlaceBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.transform.parent = null;
            currentBlock.transform.position = placementSystem.GetLastIndicatorPos();
            currentBlock = null;

        }
    }
}

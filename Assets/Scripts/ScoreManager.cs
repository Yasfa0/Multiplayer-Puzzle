using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    private float currentScore = 0;
    List<PlacementSlot> placementSlots = new List<PlacementSlot>();
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else if(Instance != null)
        {
            Destroy(this);
        }

        RefreshPlacementSlots();
    }

    private void Update()
    {
        CalculateScore();
    }

    public float CalculateScore()
    {
        Debug.Log("UPDATE");
        if (placementSlots.Count > 0)
        {
            Debug.Log("CALCULATING SCORE");
            float tempScore = 0;
            foreach (PlacementSlot slot in placementSlots)
            {
                if (slot.GetOccupied())
                {
                    tempScore += slot.GetScore();
                }
            }

            scoreText.text = "SCORE: " + tempScore;
            currentScore = tempScore;
            return tempScore;
        }
        else
        {
            return 0;
        }
    }

    public void RefreshPlacementSlots()
    {
        placementSlots.Clear();
        foreach (PlacementSlot slot in FindObjectsOfType<PlacementSlot>())
        {
            placementSlots.Add(slot);
        }
    }
       

}

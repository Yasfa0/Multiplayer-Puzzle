using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private float currentScore = 0;
    List<PlacementSlot> placementSlots = new List<PlacementSlot>();
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        foreach (PlacementSlot slot in FindObjectsOfType<PlacementSlot>()) {
            placementSlots.Add(slot);
        }
    }

    private void Update()
    {
        CalculateScore();
    }

    public float CalculateScore()
    {
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    private float currentScore = 0;
    List<PlacementSlot> placementSlots = new List<PlacementSlot>();
    List<RoomPlacementSlot> roomPlacementSlots = new List<RoomPlacementSlot>();
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] GameObject finalScoreCanvas;
    [SerializeField] TextMeshProUGUI finalScoreText;

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

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ShowFinalScore();
        }
    }

    public float CalculateScore()
    {

        if (roomPlacementSlots.Count > 0)
        {
            int occupied = 0;

            foreach (RoomPlacementSlot room in roomPlacementSlots)
            {
                if (room.GetOccupied())
                {
                    occupied += 1;
                }
            }

            if (occupied >= roomPlacementSlots.Count && CraneManager.instance.GetCurrentBlock() == null)
            {
                StageManager.Instance.ChangeMode(false);
            }

        }

        //Debug.Log("UPDATE");
        if (placementSlots.Count > 0)
        {
            int occupied = 0;
            //Debug.Log("CALCULATING SCORE");
            float tempScore = 0;
            foreach (PlacementSlot slot in placementSlots)
            {
                if (slot.GetOccupied())
                {
                    occupied++;
                    tempScore += slot.GetScore();
                }
            }

            scoreText.text = "SCORE: " + tempScore;
            currentScore = tempScore;

            if (occupied >= placementSlots.Count)
            {
                ShowFinalScore();
            }

            return tempScore;
        }

        return 0;
    }

    public void RefreshPlacementSlots()
    {
        placementSlots.Clear();
        foreach (PlacementSlot slot in FindObjectsOfType<PlacementSlot>())
        {
            placementSlots.Add(slot);
        }

        roomPlacementSlots.Clear();
        foreach (RoomPlacementSlot roomSlot in FindObjectsOfType<RoomPlacementSlot>())
        {
            roomPlacementSlots.Add(roomSlot);   
        }

    }
       
    public void ShowFinalScore()
    {
        finalScoreCanvas.SetActive(true);
        finalScoreText.text = "Final Score: " + currentScore;
    }

}

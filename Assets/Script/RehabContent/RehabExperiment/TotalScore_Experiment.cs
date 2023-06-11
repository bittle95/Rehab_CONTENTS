using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalScore_Experiment : MonoBehaviour
{
    public TextMeshProUGUI TotalScoreText;
    int scores = 0;

    void Update()
    {
        TotalScoreText.text = "Success: "+ Contents1_Manager.TotalScore.ToString();
    }

}

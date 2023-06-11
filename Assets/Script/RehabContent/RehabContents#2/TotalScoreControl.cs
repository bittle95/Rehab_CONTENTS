using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreControl : MonoBehaviour
{
    public TextMeshProUGUI TotalScoreText;
    int Animatedscores = 0;

    void Update()
    {
        TotalScoreText.text = Animatedscores.ToString();
        AnimatedScore();
    }
    void AnimatedScore()
    {
        if (Animatedscores != Contents1_Manager.TotalScore && Contents1_Manager.TotalScore > Animatedscores)
        {
            Animatedscores += 1;
        }
    }
}

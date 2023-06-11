using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalScoreControl2 : MonoBehaviour
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
        if (Animatedscores != Contents3_GameController.TotalScore && Contents3_GameController.TotalScore > Animatedscores)
        {
            Animatedscores += 1;
        }
        else if (Animatedscores != Contents3_GameController.TotalScore && Contents3_GameController.TotalScore < Animatedscores)
        {
            Animatedscores -= 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using System;

public class TodayScoreManager : MonoBehaviour
{
    public TextMeshProUGUI Content1_Score_Text;
    public TextMeshProUGUI Content2_Score_Text;
    public TextMeshProUGUI Content3_Score_Text;
    public TextMeshProUGUI Content1_Star;
    public TextMeshProUGUI Content2_Star;
    public TextMeshProUGUI Content3_Star;
    public static List<Tuple<string, int>> Content1_Score;
    public static List<Tuple<string, int>> Content2_Score;
    public static List<Tuple<string, int>> Content3_Score;
    public static bool OnlyExecuteOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        if(OnlyExecuteOnce == false)
        {
            Content1_Score = new List<Tuple<string, int>>();
            Content2_Score = new List<Tuple<string, int>>();
            Content3_Score = new List<Tuple<string, int>>();
            OnlyExecuteOnce = true;
        }
        print(Content1_Score.Count);

        if (Content1_Score.Count == 0)
        {
            Content1_Score_Text.text = "".ToString();
        }
        else
        {
            Content1_Star.text = "★";

            //정렬후
            var sortedList = Content1_Score.OrderByDescending(x => x.Item2).ToList();
            //텍스트에 이어 붙이기
            foreach(var sortedlist in sortedList)
            {
                Content1_Score_Text.text += sortedlist.Item1 + " = "+ sortedlist.Item2 + "점" + "\n";
                //print("콘텐츠1: " + sortedlist);
            }

        }

        if (Content2_Score.Count == 0)
        {
            Content2_Score_Text.text = "".ToString();
        }
        else
        {
            Content2_Star.text = "★";

            //정렬후
            var sortedList = Content2_Score.OrderByDescending(x => x.Item2).ToList();
            //텍스트에 이어 붙이기
            foreach (var sortedlist in sortedList)
            {
                Content2_Score_Text.text += sortedlist.Item1 + " = " + sortedlist.Item2 + "점" + "\n";
                //print("콘텐츠2: " + sortedlist);
            }
        }

        if (Content3_Score.Count == 0)
        {
            Content3_Score_Text.text = "".ToString();
        }
        else
        {
            Content3_Star.text = "★";

            //정렬후
            var sortedList = Content3_Score.OrderByDescending(x => x.Item2).ToList();
            //텍스트에 이어 붙이기
            foreach (var sortedlist in sortedList)
            {
                Content3_Score_Text.text += sortedlist.Item1 + " = " + sortedlist.Item2 + "점" + "\n";
                //print("콘텐츠3: " + sortedlist);
            }
        }
    }

}

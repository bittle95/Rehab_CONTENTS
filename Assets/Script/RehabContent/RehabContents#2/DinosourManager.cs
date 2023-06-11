using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DinosourManager : MonoBehaviour
{

    [Header("인터액션 관련")]
    public GameObject[] ApearEffects;
    GameObject TempApearEffect;

    GameObject[] Dinos;

    [Header("알림창 관련")]
    public GameObject GradePanel;
    public Sprite[] GradeSprites;


    static int randInt = 0;

    public DinosourManager(GameObject[] dinos)
    { 
        Dinos = dinos;
    }
    public int DinoRandomInt()
    {
        System.Random randomOBJ = new System.Random();
        randInt = randomOBJ.Next(0, Dinos.Length);
        return randInt;
    }

}

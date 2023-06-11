using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Content3_ImproveFinal : Content3_Final
{
    // Start is called before the first frame update
    void Start()
    {
        TotalScore = 0;
        ContentConfiguration.TrainingTime = "0";
        ContentIsRepeating = false;

        MeteorFallSource = this.GetComponent<AudioSource>();
        ForceShield.SetActive(false);
        TimeCountDownPanel.SetActive(false);
        DiscriptionPanel.SetActive(false);
        VideoPanel.SetActive(false);
        StartCoroutine(PhaseControl());
    }
    IEnumerator PhaseControl()
    {  
        yield return StartCoroutine(Phase1()); //Phase1이 끝나면     
        yield return StartCoroutine(Phase2()); //Phase2가 시작됨
        yield return StartCoroutine(Rest(3.0f)); //5초 정도 쉼
    }
    IEnumerator Phase1()
    {
        TimeCountDownPanel.transform.gameObject.SetActive(false);

        if (First_Phase1 == true)
        {
            VideoPanel.transform.gameObject.SetActive(true);
            VideoPanel.GetComponent<VideoPlayer>().clip = Video1;
            VideoPanel.GetComponent<VideoPlayer>().Play();
            CountDownNum = (float)Video1.length;

            yield return new WaitForSecondsRealtime(CountDownNum); //카운트 후, 다음 줄 시작

            VideoPanel.transform.gameObject.SetActive(false);
            First_Phase1 = false;
        }
        else
        {
            DiscriptionPanel.transform.gameObject.SetActive(true);
            DiscriptionPanel.transform.GetChild(0).GetComponent<Text>().text = "운석 피하기";
            DiscriptionPanel.transform.GetChild(1).GetComponent<Image>().sprite = meteorSprite;
            CountDownNum = 5; //카운트 설정
            yield return new WaitForSecondsRealtime(CountDownNum); //카운트 후, 다음 줄 시작
            DiscriptionPanel.transform.gameObject.SetActive(false);
        }

        TimeCountDownPanel.transform.gameObject.SetActive(true);

        float objRepeatRate = 0;
        float PhaseCountDown = 145;

        if (float.Parse(ContentConfiguration.Difficulty) == 1) //난이도가 상이면
        {
            objRepeatRate = 2f;
        }
        else //난이도가 하이면
        {
            objRepeatRate = 1f;
        }
        InvokeRepeating("MeteorInstantiate", 1, objRepeatRate);

        CountDownNum = PhaseCountDown; //카운트 설정 ==> 카운트 동안 운석/선물 수행
        yield return new WaitForSecondsRealtime(CountDownNum); //카운트 후, 다음 줄 시작
        CancelInvoke("MeteorInstantiate");

        //print("생성된 총 메테오 : " + ContentConfiguration.TotalMetour + ", " + "난이도 : " + ContentConfiguration.Difficulty);

        yield return new WaitForSecondsRealtime(7); //약간의 여유를 줌 (바로 phase 2 시작 안하게끔)
        TimeCountDownPanel.transform.gameObject.SetActive(false);
    }
    IEnumerator Phase2()
    {
        if (First_Phase2 == true)
        {
            VideoPanel.transform.gameObject.SetActive(true);
            VideoPanel.GetComponent<VideoPlayer>().clip = Video2;
            VideoPanel.GetComponent<VideoPlayer>().Play();
            CountDownNum = (float)Video2.length;

            yield return new WaitForSecondsRealtime(CountDownNum); //카운트 후, 다음 줄 시작

            VideoPanel.transform.gameObject.SetActive(false);
            First_Phase2 = false;
        }
        else
        {
            DiscriptionPanel.transform.gameObject.SetActive(true);
            DiscriptionPanel.transform.GetChild(0).GetComponent<Text>().text = "선물 받기";
            DiscriptionPanel.transform.GetChild(1).GetComponent<Image>().sprite = giftSprite;
            CountDownNum = 5; //카운트 설정
            yield return new WaitForSecondsRealtime(CountDownNum); //카운트 후, 다음 줄 시작
            DiscriptionPanel.transform.gameObject.SetActive(false);
        }
        TimeCountDownPanel.transform.gameObject.SetActive(true);

        float objRepeatRate = 0;
        float PhaseCountDown = 145;

        if (float.Parse(ContentConfiguration.Difficulty) == 1) //난이도가 상이면
        {
            objRepeatRate = 1f;
        }
        else //난이도가 하이면
        {
            objRepeatRate = 1f;
        }
        InvokeRepeating("GiftInstantiate", 1, objRepeatRate);

        CountDownNum = PhaseCountDown; //카운트 설정 ==> 카운트 동안 운석/선물 수행
        yield return new WaitForSecondsRealtime(CountDownNum); //카운트 후, 다음 줄 시작
        CancelInvoke("GiftInstantiate");

        //print("생성된 총 선물 : " + ContentConfiguration.TotalGift + ", " + "난이도 : " + ContentConfiguration.Difficulty);

        yield return new WaitForSecondsRealtime(4);
        TimeCountDownPanel.transform.gameObject.SetActive(false);
    }
    IEnumerator Rest(float restTime)
    {
        yield return new WaitForSecondsRealtime(restTime);
    }
    void Update()
    {
        if (CountDownNum + 5 > 0) //+5를 통해 혼란 방지
        {
            CountDownNum -= Time.deltaTime;
            TimeCountDownPanel.transform.GetChild(0).GetComponent<Text>().text = (CountDownNum + 5).ToString("N0");  //+3를 통해 혼란 방지
        }
    }

}

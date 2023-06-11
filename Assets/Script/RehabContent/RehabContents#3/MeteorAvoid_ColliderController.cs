using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeteorAvoid_ColliderController : MonoBehaviour
{
    public DateTime StartTime, EndTime, MeasureTime;

    [Header("판넬 관련")]
    public TextMeshProUGUI MeteorCnt_Text;
    public TextMeshProUGUI GiftCnt_Text;
    public Text Totaltime_Text;

    [Header("프리펩 관련")]
    public GameObject MeteorEffectPrefab;
    public GameObject MeteorEffectPrefab_Giant;
    public GameObject GiftEffectPreafab;

    [Header("스프라이트 관련")]
    public GameObject ScoreSprite;
    public Sprite MeteorSprite;
    public Sprite MeteorSprite_Giant;
    public Sprite GiftSprite;

    [Header("사운드 관련")]
    public AudioClip MeteorExplode;
    public AudioClip MeteorExplode_Giant;
    public AudioClip GiftGet;
    AudioSource MeteorSource;
    AudioSource MeteorSource_Giant;
    AudioSource GiftSource;


    private void Start()
    {
        ContentConfiguration.TotalMetour = 0;
        ContentConfiguration.TotalGift = 0;
        ContentConfiguration.AvoidMeteor_cnt = 0;
        ContentConfiguration.GainedGift_cnt = 0;

        StartTime = DateTime.Now;
        Finished = false;

        MeteorSource = this.GetComponent<AudioSource>();
        GiftSource = this.GetComponent<AudioSource>();
        MeteorSource_Giant = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Meteor")
        {
            ContentConfiguration.AvoidMeteor_cnt += 1;
            MeteorCnt_Text.text = ContentConfiguration.AvoidMeteor_cnt.ToString();

            GameObject MeteorEffect;
            if (Contents3_GameController.TotalScore > 0)
            {
                GameObject scoreSprite;
                scoreSprite = Instantiate(ScoreSprite, collision.transform.position, Quaternion.Euler(new Vector3(0,0,90)));
                scoreSprite.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite.GetComponent<SpriteRenderer>().sprite = MeteorSprite;
                Contents3_GameController.TotalScore -= 10;
                Destroy(scoreSprite, 1f);
            }
            if (Contents3_GameController.TotalScore < 0) Contents3_GameController.TotalScore = 0;
            //print(Contents2_GameController.TotalScore);
            MeteorEffect = Instantiate(MeteorEffectPrefab, collision.gameObject.transform.position, Quaternion.identity);
            MeteorEffect.transform.SetParent(GameObject.Find("Components").transform, true);

            //사운드
            MeteorSource.clip = MeteorExplode;
            MeteorSource.PlayOneShot(MeteorSource.clip); // 이펙트 사운드
            MeteorSource.loop = false;

            Destroy(collision.gameObject); //해당 운석 제거
        }
        if (collision.tag == "Gift")
        {
            ContentConfiguration.GainedGift_cnt += 1;
            GiftCnt_Text.text = ContentConfiguration.GainedGift_cnt.ToString();

            GameObject scoreSprite;
            scoreSprite = Instantiate(ScoreSprite, collision.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            scoreSprite.transform.SetParent(GameObject.Find("Components").transform, true);
            scoreSprite.GetComponent<SpriteRenderer>().sprite = GiftSprite;
            Contents3_GameController.TotalScore += 50;
            Destroy(scoreSprite, 1f);

            GameObject GiftEffect;
            GiftEffect = Instantiate(GiftEffectPreafab, collision.gameObject.transform.position, Quaternion.identity);
            GiftEffect.transform.SetParent(GameObject.Find("Components").transform, true);

            //사운드
            GiftSource.clip = GiftGet;
            GiftSource.PlayOneShot(GiftSource.clip); // 이펙트 사운드
            GiftSource.loop = false;

            Destroy(collision.gameObject); //해당 아이템 제거
        }
        if (collision.tag == "Meteor_Giant")
        {
            GameObject MeteorEffect_Giant;
            if (Contents3_GameController.TotalScore > 0)
            {
                GameObject scoreSprite;
                scoreSprite = Instantiate(ScoreSprite, collision.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                scoreSprite.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite.GetComponent<SpriteRenderer>().sprite = MeteorSprite_Giant;
                Contents3_GameController.TotalScore -= 200;
                Destroy(scoreSprite, 1f);
            }
            if (Contents3_GameController.TotalScore < 0) Contents3_GameController.TotalScore = 0;

            MeteorEffect_Giant = Instantiate(MeteorEffectPrefab_Giant, collision.gameObject.transform.position, Quaternion.identity);
            MeteorEffect_Giant.transform.SetParent(GameObject.Find("Components").transform, true);

            //사운드
            MeteorSource_Giant.clip = MeteorExplode_Giant;
            MeteorSource_Giant.PlayOneShot(MeteorSource_Giant.clip); // 이펙트 사운드
            MeteorSource_Giant.loop = false;


            Destroy(collision.gameObject); //거대 운석 제거
        }
        if (collision.tag == "SuccessLine")
        {
            var colliderOff = this.transform.GetComponent<CapsuleCollider>();
            colliderOff.enabled = false;
            FinishProcess();
        }
    }
    public void FinishProcess(string ment = "정상종료")
    {
        var info1 = (100 * ((float)ContentConfiguration.AvoidMeteor_cnt / (float)ContentConfiguration.TotalMetour));
        if (float.IsNaN(info1)) ContentConfiguration.Info1 = "0";
        else ContentConfiguration.Info1 = info1.ToString("N1");

        var info2 = (100 * ((float)ContentConfiguration.GainedGift_cnt / (float)ContentConfiguration.TotalGift));
        if (float.IsNaN(info2)) ContentConfiguration.Info2 = "0";
        else ContentConfiguration.Info2 = info2.ToString("N1");

        Contents3_GameController.ContentIsRepeating = true;
        EndTime = DateTime.Now;

        #region 전방 보행이 사라지면서 생긴 코드
        //FootInformation.SaveToToalData(); 전방 보행이 사라지면서 주석처리
        FootInformation.SaveToToalData2();
        #endregion

        var networkManager = FindObjectOfType<ContentCommunication>();
        ContentConfiguration.TrainingTime = (EndTime - StartTime).TotalSeconds.ToString("N2");
        ContentConfiguration.Info3 = ment;
        TodayScoreManager.Content3_Score.Add(new Tuple<string, int> 
            (DateTime.Now.ToString("yy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + " " + DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm"),
            Contents3_GameController.TotalScore));
        networkManager.SendContentFinish();
    }

    bool Finished = false;
    private void Update()
    {
        MeasureTime = DateTime.Now;
        Totaltime_Text.text = (MeasureTime - StartTime).ToString("mm") + "분" + (MeasureTime - StartTime).ToString("ss") + "초";

        //print("끝남테스트1 : " + float.Parse((MeasureTime - StartTime).ToString("mm")));
        //print("끝남테스트2 : " + float.Parse((MeasureTime - StartTime).ToString("ss")));

        if ( float.Parse((MeasureTime - StartTime).ToString("mm")) == 5 && float.Parse((MeasureTime - StartTime).ToString("ss")) == 20 && Finished == false)
        {
            Finished = true;
            //Invoke("FinishProcess", 1.5f);
            FinishProcess();
        }
    }

}

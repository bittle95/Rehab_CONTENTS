using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class Contents3_GameController : MonoBehaviour
{
    public static int TotalScore = 0;

    protected bool First_Phase1 = true;
    protected bool First_Phase2 = true;

    [Header("사운드 관련")]
    public AudioClip MeteorFall_Giant;
    protected AudioSource MeteorFallSource;

    [Header("판넬 관련")]
    public GameObject TimeCountDownPanel;
    public GameObject GuidePanel;
    public GameObject VideoPanel;
    public GameObject DiscriptionPanel;
    public GameObject GuideCountPanel;
    public VideoClip Video1;
    public VideoClip Video2;

    [Header("아이템 매니저")]
    public ItemManager_content3 ItemManager;

    [Header("발 관련")]
    public GameObject LeftFoot;
    public GameObject RightFoot;
    public GameObject Target;

    [Header("보호막")]
    public GameObject ForceShield;
    protected float CountDownNum = 0;

    [Header("프리펩")]
    public GameObject[] MeteorPrefab;
    public GameObject[] MeteorPrefab_Giant;
    public GameObject GiftPrefab;
    public Transform[] InstantiatePosition;
    // Start is called before the first frame update

    public static bool ContentIsRepeating = false;
     
    void Start()
    {
        var TargetCC = Target.transform.GetComponent<CapsuleCollider>();
        TargetCC.enabled = false;
        var TargetSR = Target.transform.GetChild(0).GetComponent<SpriteRenderer>();
        TargetSR.enabled = false;

        MeteorFallSource = this.GetComponent<AudioSource>();
        ForceShield.SetActive(false);
        GuidePanel.SetActive(false);
        TimeCountDownPanel.SetActive(false);
        DiscriptionPanel.SetActive(false);
        VideoPanel.SetActive(false);
        GuideCountPanel.SetActive(false);
        StartCoroutine(PhaseControl());
    }

    void GuidePannel_Func(bool SetPanel)
    {
        GuidePanel.SetActive(SetPanel);
        Vector3 temp =  Vector3.zero;
        temp.x = (LeftFoot.transform.position.x + RightFoot.transform.position.x) / 2;
        temp.z = 122;
        GuidePanel.transform.position = temp;
    }

    IEnumerator PhaseControl()
    {
        while(ContentIsRepeating == false)
        {
            yield return StartCoroutine(Phase1()); //Phase1이 끝나면
            yield return StartCoroutine(Phase2()); //Phase2가 시작됨
            yield return StartCoroutine(Rest(5.0f)); //5초 정도 쉼
        }
    }
    IEnumerator Phase1()
    {
        TimeCountDownPanel.transform.gameObject.SetActive(false);

        var TargetCC = Target.transform.GetComponent<CapsuleCollider>();
        TargetCC.enabled = true;
        var TargetSR = Target.transform.GetChild(0).GetComponent<SpriteRenderer>();
        TargetSR.enabled = true;

        var LeftFootCC = LeftFoot.transform.GetComponent<CapsuleCollider>();
        LeftFootCC.enabled = false;
        var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        LeftFootSR.enabled = false;

        var RightFootCC = RightFoot.transform.GetComponent<CapsuleCollider>();
        RightFootCC.enabled = false;
        var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        RightFootSR.enabled = false;


        if (First_Phase1 == true)
        {
            VideoPanel.transform.gameObject.SetActive(true);
            VideoPanel.GetComponent<VideoPlayer>().clip = Video1;
            VideoPanel.GetComponent<VideoPlayer>().Play();
            CountDownNum = (float)Video1.length;

            yield return new WaitForSeconds(CountDownNum); //카운트 후, 다음 줄 시작
            
            VideoPanel.transform.gameObject.SetActive(false);
            First_Phase1 = false;
        }
        else
        {
            DiscriptionPanel.transform.gameObject.SetActive(true);
            DiscriptionPanel.transform.GetChild(0).GetComponent<Text>().text = "Phase1이 시작됩니다. \n 날아오는 운석을 피하고 선물을 획득하세요";
            CountDownNum = 5; //카운트 설정
            yield return new WaitForSeconds(CountDownNum); //카운트 후, 다음 줄 시작
            DiscriptionPanel.transform.gameObject.SetActive(false);
        }

        TimeCountDownPanel.transform.gameObject.SetActive(true);
        GuidePannel_Func(true);

        float objRepeatRate = 0;
        float PhaseCountDown = 0;

        if (float.Parse(ContentConfiguration.Difficulty) == 1) //난이도가 상이면
        {
            objRepeatRate = 2f;
            PhaseCountDown = 20;
        }
        else //난이도가 하이면
        {
            objRepeatRate = 3f;
            PhaseCountDown = 15;
        }
        InvokeRepeating("MeteorInstantiate", 1, objRepeatRate);
        InvokeRepeating("GiftInstantiate", 1, objRepeatRate);
        
        CountDownNum = PhaseCountDown; //카운트 설정 ==> 카운트 동안 운석/선물 수행
        yield return new WaitForSeconds(CountDownNum); //카운트 후, 다음 줄 시작
        CancelInvoke("MeteorInstantiate");
        CancelInvoke("GiftInstantiate");

        print("생성된 총 메테오 : " + ContentConfiguration.TotalMetour);
        print("생성된 총 선물 : " + ContentConfiguration.TotalGift);

        TimeCountDownPanel.transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(6); //약간의 여유를 줌 (바로 phase 2 시작 안하게끔)
        GuidePannel_Func(false);
    }

    public static bool IsPunishing = false;
    IEnumerator Phase2()
    {
        var TargetCC = Target.transform.GetComponent<CapsuleCollider>();
        TargetCC.enabled = false;
        var TargetSR = Target.transform.GetChild(0).GetComponent<SpriteRenderer>();
        TargetSR.enabled = false;

        var LeftFootCC = LeftFoot.transform.GetComponent<CapsuleCollider>();
        LeftFootCC.enabled = true;
        var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        LeftFootSR.enabled = true;

        var RightFootCC = RightFoot.transform.GetComponent<CapsuleCollider>();
        RightFootCC.enabled = true;
        var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        RightFootSR.enabled = true;

        //print("생성될 총 발자국 수: " + ItemManager.ItemCount);
        if (First_Phase2 == true)
        {
            VideoPanel.transform.gameObject.SetActive(true);
            VideoPanel.GetComponent<VideoPlayer>().clip = Video2;
            VideoPanel.GetComponent<VideoPlayer>().Play();
            CountDownNum = (float)Video2.length;

            yield return new WaitForSeconds(CountDownNum); //카운트 후, 다음 줄 시작

            VideoPanel.transform.gameObject.SetActive(false);

            First_Phase2 = false;
        }
        else
        {
            DiscriptionPanel.transform.gameObject.SetActive(true); //영상 재생 할 것
            DiscriptionPanel.transform.GetChild(0).GetComponent<Text>().text = "Phase2가 시작됩니다. \n 빨간색 발판을 정확하게 밟으세요";
            yield return new WaitForSeconds(5); //판넬용 대기 시간
        }

        DiscriptionPanel.transform.gameObject.SetActive(false);
        TimeCountDownPanel.transform.gameObject.SetActive(true); //
        GuideCountPanel.transform.gameObject.SetActive(true);

        IsPunishing = false; 
        Vector3 tempItM = ItemManager.transform.position;
        tempItM.x = (LeftFoot.transform.position.x + RightFoot.transform.position.x) / 2;
        ItemManager.ItemEvent.Invoke(); //화살표 가이드 생성 시작

        CountDownNum = 30; //카운트 설정 ==> 카운트 동안 GaitGuide 수행

        yield return new WaitForSeconds(1); //시간 조금만 더 주기 (에러 야기함)
        yield return new WaitUntil(() => CountDownNum < 0 || ItemManager.transform.childCount == 0);

        CountDownNum = 0;

        yield return new WaitForSeconds(3); //카운트 후, 다음 줄 시작
        TimeCountDownPanel.transform.gameObject.SetActive(false);
        GuideCountPanel.transform.gameObject.SetActive(false);
        GiantMeteorInstantiate();
        if (ItemManager.transform.childCount == 0 && CountDownNum <= 0)
        {
            ForceShield.SetActive(true);
            ItemManager.CurIndex = 0;
            ItemManager.ItemCount += 1;
        }
        else if (ItemManager.transform.childCount != 0 && CountDownNum <= 0)
        {
            IsPunishing = true;
            ItemManager.CurIndex = 0;
            Destroy(ItemManager.Target.gameObject);
            ItemManager_content3.ItemLists.Remove(ItemManager.Target);
        }
    }
    IEnumerator Rest(float restTime)
    {
        yield return new WaitForSeconds(restTime);
    }

    void MeteorInstantiate()
    {
        ContentConfiguration.TotalMetour += 1;

        GameObject Meteor;
        int randPos = Random.Range(0, 10);  //날아올 위치 랜덤으로 선정
        int randPlanet = Random.Range(0, MeteorPrefab.Length);
        Meteor = Instantiate(MeteorPrefab[randPlanet], InstantiatePosition[randPos].position, Quaternion.identity);
        Meteor.transform.SetParent(GameObject.Find("Components").transform, true);
        Destroy(Meteor, 12f);
    }
    void GiantMeteorInstantiate()
    {
        //사운드
        MeteorFallSource.clip = MeteorFall_Giant;
        MeteorFallSource.PlayOneShot(MeteorFallSource.clip); // 이펙트 사운드
        MeteorFallSource.loop = false;


        GameObject Meteor;
        Vector3 middlePos = (InstantiatePosition[2].position + InstantiatePosition[3].position) / 2;
        int randGiant = Random.Range(0, MeteorPrefab_Giant.Length);
        Meteor = Instantiate(MeteorPrefab_Giant[randGiant], middlePos, Quaternion.identity);
        Meteor.transform.SetParent(GameObject.Find("Components").transform, true);
        Destroy(Meteor, 12f);
    }
    void GiftInstantiate()
    {
        ContentConfiguration.TotalGift += 1;

        Quaternion InitRot = Quaternion.Euler(new Vector3(0, 0, 90f));//Quaternion.identity;
        GameObject Gift;
        int randPos = Random.Range(1, 5);
        Gift = Instantiate(GiftPrefab, InstantiatePosition[randPos].position, InitRot);
        Gift.transform.SetParent(GameObject.Find("Components").transform, true);
        Destroy(Gift, 12f);
    }
    // Update is called once per frame
    void Update()
    {
        if (CountDownNum > 0)
        {
            CountDownNum -= Time.deltaTime;
            TimeCountDownPanel.transform.GetChild(0).GetComponent<Text>().text = CountDownNum.ToString("N0");
        }
        if(ForceShield.activeSelf == true)
        {
            ForceShield.transform.position = (LeftFoot.transform.position + RightFoot.transform.position) / 2;
        }
        if(GuideCountPanel.activeSelf == true)
        {
            GuideCountPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (ItemManager.ItemCount - ItemManager.CurIndex + 1).ToString();
        }
        Target.transform.position = RightFoot.transform.position; //

        //print("HitedMeteor_cnt: " + ContentConfiguration.HitedMeteor_cnt);
        //print("TotalMetour: " + ContentConfiguration.TotalMetour);
        //print("GainedGift_cnt: " + ContentConfiguration.GainedGift_cnt);
        //print("TotalGift: " + ContentConfiguration.TotalGift);
    }
}

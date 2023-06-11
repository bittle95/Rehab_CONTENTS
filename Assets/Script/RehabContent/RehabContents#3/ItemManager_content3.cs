using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using cakeslice;

public class ItemManager_content3 : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent ItemEvent;

    [Header("도착 지점")]
    public Transform[] FinishLine;

    [Header("보폭")]
    [HideInInspector]
    public float StrideLength = float.Parse(ContentConfiguration.Stride); //180

    [Header("양발 사이")]
    [HideInInspector]
    public float SideLength = float.Parse(ContentConfiguration.Width); //150

    [Header("오브젝트")]
    public GameObject Item;

    [Header("크기")]
    [HideInInspector]
    public float ItemSize = 100f; //어째선지 6f였다

    [Header("갯수")]
    [HideInInspector]
    public int ItemCount = 5;

    public AudioClip CookieEatSound;

    [HideInInspector]
    public static List<Transform> ItemLists;
    [HideInInspector]
    public Transform Target = null;

    public static List<Transform> DrawItemGizmo;

    public Transform LeftFoot, RightFoot;

    private AreaCalculateModule AreaCalculator;
    private float StayTime = 0;
    private bool IsStay = false;
    private float[] ares;
    private void Awake() => ares = new float[2] { 0, 0 };

    float Overlapping_area = 15;
    private void Start()
    {
        Contents3_GameController.TotalScore = 0;

        FootInformation.Initialize();

        StrideLength = float.Parse(ContentConfiguration.Stride); //180
        SideLength = float.Parse(ContentConfiguration.Width); //150

        #region 난이도 설정
        if (float.Parse(ContentConfiguration.Difficulty) == 1) //난이도 상
        {
            Overlapping_area = 30;
            ContentConfiguration.Info1 = "0";
            ContentConfiguration.Info2 = "0";
        }
        else //난이도 하
        {
            Overlapping_area = 15;
            ContentConfiguration.Info1 = "0";
            ContentConfiguration.Info2 = "0";
        }
        #endregion

        #region FinishLine 위치 설정
        for (int i = 0; i < FinishLine.Length; i++) FinishLine[i].gameObject.SetActive(false);
        switch (ContentConfiguration.Distance)
        {
            case "5":
                FinishLine[0].gameObject.SetActive(true);
                break;
            case "10":
                FinishLine[1].gameObject.SetActive(true);
                break;
            case "20":
                FinishLine[2].gameObject.SetActive(true);
                break;
        }
        #endregion

        ItemLists = new List<Transform>();
        ItemEvent = new UnityEvent();

        int ItemIndex = 0;

        ItemEvent.AddListener(() =>
        {
            //각 쿠키의 x 위치 설정
            Vector3 TempPosition = transform.position;
            TempPosition.x -= ItemIndex * StrideLength;
            ItemIndex += 1;

            //랜덤생성모드
            //float SideValue = Random.Range(-SideLength, SideLength);
            //TempPosition.y = SideValue;

            //균일생성모드
            if (ItemIndex % 2 == 0)TempPosition.y += SideLength; 
            else TempPosition.y -= SideLength; 

            //쿠키 방향 설정
            Quaternion TempRotation = Quaternion.Euler(0, 0, 90);

            //쿠키 생성
            GameObject ItemClone = Instantiate(Item, TempPosition, TempRotation);

            if (ItemIndex % 2 == 0)
            {
                ItemClone.GetComponent<FootInformation>().Direction = FootDirection.RIGHT;
                FootInformation.TotalRightCount++;
            }
            else
            {
                ItemClone.GetComponent<FootInformation>().Direction = FootDirection.LEFT;
                FootInformation.TotalLeftCount++;
            }

            //쿠키 생성 후, 크기 설정
            ItemClone.transform.localScale = new Vector3(ItemSize, ItemSize, ItemSize);
            ItemClone.transform.SetParent(transform, true);

            //쿠키 태그 설정
            ItemClone.tag = "Cookie";
            ItemLists.Add(ItemClone.transform);
        });


        AreaCalculator = new AreaCalculateModule(LeftFoot, RightFoot, 80);
        AreaCalculator.OnFootStartEventHandler = (foot, position) =>
        {
            IsStay = true;
        };
        AreaCalculator.OnFootEndEventHandler = (foot, position) =>
        {
            IsStay = false;
        };
    }
    private bool ConditionCheck()
    {
        var s = Mathf.PI * ItemSize * ItemSize;
        var max = Mathf.Max(ares[0], ares[1]);
        //Debug.Log(string.Format("겹침 영역: {0}, 전체 크기: {1}, 퍼센트: {2}", max, s, max / s * 100));
        if (max / s > Overlapping_area / 100) return true; //0.15f
        else return false;
    }

    [HideInInspector]
    public int CurIndex = 0;
    private void Update()
    {
        if (IsStay && ConditionCheck()) StayTime += Time.deltaTime;
        else StayTime = 0;
        if (StayTime > 0.5f) //겹친지 3초뒤, 쿠키 삭제
        {
            Debug.Log("겹치는 중");
            StayTime = 0;
            IsStay = false;

            //사운드
            AudioSource audiosource = this.gameObject.GetComponent<AudioSource>();
            audiosource.clip = CookieEatSound;
            audiosource.Play(); 
            audiosource.loop = false;
            Contents3_GameController.TotalScore += 10;

            if (Target.GetComponent<FootInformation>().Direction == FootDirection.LEFT) FootInformation.SuccessLeftCount++;
            else FootInformation.SuccessRightCount++;

            Destroy(Target.gameObject);
            ItemLists.Remove(Target);
            Target = null;

            if (CurIndex < ItemCount) //반복 생성
            {
                ItemEvent?.Invoke();
                CurIndex++;
            }
        }
        if (ItemLists.Count != 0 && Contents3_GameController.IsPunishing == false) //아이템 리스트가 없거나, 거대운석이 떨어지지 않는 동안만 계산
        {
            Transform targetFromLeftFoot, targetFromRightFoot;
            targetFromLeftFoot = ItemLists.Aggregate((minItem, nextItem) => AreaCalculator.CalculateDistance(LeftFoot, minItem) < AreaCalculator.CalculateDistance(LeftFoot, nextItem) ? minItem : nextItem);
            targetFromRightFoot = ItemLists.Aggregate((minItem, nextItem) => AreaCalculator.CalculateDistance(RightFoot, minItem) < AreaCalculator.CalculateDistance(RightFoot, nextItem) ? minItem : nextItem);
            var target = AreaCalculator.CalculateDistance(LeftFoot, targetFromLeftFoot) > AreaCalculator.CalculateDistance(RightFoot, targetFromRightFoot) ? targetFromRightFoot : targetFromLeftFoot;
            if (target != Target && Target != null)
            {
                //Destroy(Target.gameObject.GetComponent<Outline>());
                IsStay = false;
            }
            Target = target;

            //쿠키 강조 효과
            //if (target.GetComponent<Outline>() == null) target.gameObject.AddComponent<Outline>();
            ares = AreaCalculator.GetAreas(target, ItemSize);
        }
    }

    private void OnDrawGizmos()
    {
        if (AreaCalculator != null) AreaCalculator.DrawGizmo();
    }
}

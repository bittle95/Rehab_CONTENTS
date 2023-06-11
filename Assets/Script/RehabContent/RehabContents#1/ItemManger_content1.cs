using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using cakeslice;

public class ItemManger_content1 : MonoBehaviour
{
    [Header("도착 지점")]
    public Transform FinishLine;

    [Header("거리 기준")]
    public Transform RoadOrigin;

    [Header("기본 거리 오브젝트")]
    public GameObject DefaultRoad;

    [Header("보폭")]
    [HideInInspector]
    public float StrideLength = 180;


    [Header("양발 사이")]
    [HideInInspector]
    public float SideLength = 150;

    [Header("오브젝트")]
    public GameObject FootPrint;

    [Header("크기")]
    [HideInInspector]
    public float ItemSize = 80; //80

    [Header("갯수")]
    public int ItemCount = 100;

    public AudioClip SuccessSound;
    public AudioClip FailSound;

    [HideInInspector]
    public static List<Transform> ItemLists;
    public static List<Transform> DrawItemGizmo;

    public Transform LeftFoot, RightFoot;

    private AreaCalculateModule AreaCalculator;
    private Transform Target = null;
    private float StayTime = 0;
    private bool IsStay = false;
    private float[] ares;

    public Sprite LeftSprite, RightSprite; 
    public Sprite LeftSprite_Wrong, RightSprite_Wrong;

    private void Awake() => ares = new float[2] { 0, 0 };

    float Overlapping_area = 15;
    private void Start()
    {
        Contents1_Manager.TotalScore = 0;
        ContentConfiguration.TrainingTime = "0";

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
        var RoadCount = float.Parse(ContentConfiguration.Distance) / 5;
        //var RoadCount = RoadLength / 5;
        for(int i = 1; i < RoadCount; ++i)
        {
            var roadInstance = Instantiate(DefaultRoad, RoadOrigin).transform;
            var roadPosition = roadInstance.localPosition;
            roadPosition.y = DefaultRoad.transform.localPosition.y + 572 * i;
            roadInstance.localPosition = roadPosition;
        }

        var finishLinePosition = FinishLine.localPosition;
        finishLinePosition.y = transform.localPosition.y + RoadCount * 430;
        FinishLine.localPosition = finishLinePosition;
        #endregion

        CreateDynamicItem();

        AreaCalculator = new AreaCalculateModule(LeftFoot, RightFoot, 80);
        AreaCalculator.OnFootStartEventHandler = (foot, position) =>
        {
            IsStay = true;
        };
        AreaCalculator.OnFootEndEventHandler = (foot, position) =>
        {
            IsStay = false;
        };
        ItemLists = new List<Transform>();
        foreach (Transform child in transform)
        {
            ItemLists.Add(child);
        }

    }
    private bool ConditionCheck()
    {
        var s = Mathf.PI * (ItemSize * 1.4f) * (ItemSize * 1.4f);
        var max = Mathf.Max(ares[0], ares[1]);
        //Debug.Log(string.Format("겹침 영역: {0}, 전체 크기: {1}, 퍼센트: {2}", max, s, max/s*100));
        if (max / s > Overlapping_area / 100) return true; //30
        else return false;
    }
    bool FootWorkGood = false;
    private void Update()
    {
        if (IsStay == true && ConditionCheck() == true) //
        {
            FootWorkGood = true;
            StayTime += Time.deltaTime;
        }

        else if (IsStay == true && ConditionCheck() == false)
        {
            FootWorkGood = false;
            StayTime += Time.deltaTime;
        }

        else StayTime = 0;

        if (StayTime > 0.5f) //교차영역 만족해야 하는 기준 시간
        {
            if(FootWorkGood == true)
            {
                StayTime = 0;
                IsStay = false;
                //쿠키 사운드
                AudioSource audiosource = this.gameObject.GetComponent<AudioSource>();
                audiosource.clip = SuccessSound;
                audiosource.Play(); //쿠키 사운드
                audiosource.loop = false;
                Contents1_Manager.TotalScore += 10;
                if (Target.GetComponent<FootInformation>().Direction == FootDirection.LEFT) FootInformation.SuccessLeftCount++;
                else FootInformation.SuccessRightCount++;
                Destroy(Target.gameObject);
                ItemLists.Remove(Target);
                Target = null;
            }
            else
            {
                StayTime = 0;
                IsStay = false;

                if (Target.GetComponent<FootInformation>().Direction == FootDirection.LEFT)
                {
                    Target.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = LeftSprite_Wrong;
                }
                else
                {
                    Target.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RightSprite_Wrong;
                }

            }
            
        }
        if (ItemLists.Count != 0)
        {
            //Transform targetFromLeftFoot, targetFromRightFoot;
            var mPosition = (LeftFoot.position + RightFoot.position) / 2;
            Transform[] targetCandidates = ItemLists.Where(item => mPosition.x > item.position.x).OrderByDescending(frontItem => frontItem.position.x).ToArray();
            if (targetCandidates.Length == 0)
            {
                Outline outline;
                if (Target != null && (outline = Target.GetComponent<Outline>()))
                {
                    Destroy(outline);
                    IsStay = false;
                }
                return;
            }
            Transform target = targetCandidates.First();
            //targetFromLeftFoot = ItemLists.Aggregate((minItem, nextItem) => AreaCalculator.CalculateDistance(LeftFoot, minItem) < AreaCalculator.CalculateDistance(LeftFoot, nextItem) ? minItem : nextItem);
            //targetFromRightFoot = ItemLists.Aggregate((minItem, nextItem) => AreaCalculator.CalculateDistance(RightFoot, minItem) < AreaCalculator.CalculateDistance(RightFoot, nextItem) ? minItem : nextItem);
            //var target = AreaCalculator.CalculateDistance(LeftFoot, targetFromLeftFoot) > AreaCalculator.CalculateDistance(RightFoot, targetFromRightFoot) ? targetFromRightFoot : targetFromLeftFoot;
            if (target != Target && Target != null)//타겟 바뀌면 제거하는 코드
            {
                Destroy(Target.gameObject.GetComponent<Outline>());
                IsStay = false;
            }
            Target = target;

            //쿠키 강조 효과
            if (target.GetComponent<Outline>() == null) target.gameObject.AddComponent<Outline>();
            ares = AreaCalculator.GetAreas(target, ItemSize * 1.4f);
            //Debug.Log(string.Format("{0}, {1}", ares[0], ares[1]));
        }
    }
    public void CreateDynamicItem()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        int itemIndex = 0;
        while (true)
        {
            Vector3 TempPosition = transform.position;

            //각 쿠키의 x 위치 설정
            TempPosition.x -= itemIndex * StrideLength;

            //각 쿠키의 y 위치 설정
            if (itemIndex % 2 == 0) TempPosition.y += SideLength;
            else TempPosition.y -= SideLength;

            var positionBasedComponents = transform.parent.InverseTransformPoint(TempPosition);
            Vector3 TempFinishPos = FinishLine.localPosition;
            TempFinishPos.y -= 25;
            if (positionBasedComponents.y > TempFinishPos.y) break;

            //쿠키 방향 설정
            Quaternion TempRotation = Quaternion.Euler(0, 180, 0);

            //쿠키 생성
            GameObject ItemClone = Instantiate(FootPrint, TempPosition, TempRotation);

            if (itemIndex % 2 == 0)
            {
                ItemClone.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = LeftSprite;
                ItemClone.GetComponent<FootInformation>().Direction = FootDirection.RIGHT;
                FootInformation.TotalRightCount++;
            }
            else
            {
                ItemClone.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RightSprite;
                ItemClone.GetComponent<FootInformation>().Direction = FootDirection.LEFT;
                FootInformation.TotalLeftCount++;
            }

            //쿠키 생성 후, 크기 설정
            ItemClone.transform.localScale = new Vector3(ItemSize * 1.4f, ItemSize * 1.4f, ItemSize * 1.4f); //크기 조금 늘림
            ItemClone.transform.SetParent(transform, true);

            //쿠키 태그 설정
            ItemClone.tag = "Cookie";
            itemIndex++;
        }
    }
    private void OnDrawGizmos()
    {
        if (AreaCalculator != null) AreaCalculator.DrawGizmo();
    }
}

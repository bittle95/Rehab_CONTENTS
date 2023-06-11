using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using cakeslice;


public class ItemManager_Experiment : MonoBehaviour
{
    [Header("보폭")]
    public float StrideLength = 500f; //
    [Header("양발 사이")]
    public float SideLength = 70f; //

    [Header("오브젝트")]
    public GameObject Fossile;

    [Header("크기")]
    public float ItemSize = 50f;

    [Header("갯수")]
    public int ItemCount = 20;

    public AudioClip CookieEatSound;

    [HideInInspector]
    public static List<Transform> ItemLists;


    public static List<Transform> DrawItemGizmo;

    public Transform LeftFoot, RightFoot;

    private AreaCalculateModule AreaCalculator;
    private Transform Target = null;
    private float StayTime = 0;
    private bool IsStay = false;
    private float[] ares;
    private void Awake() => ares = new float[2] { 0, 0 };
    private void Start()
    {
        AreaCalculator = new AreaCalculateModule(LeftFoot, RightFoot, 110);
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
        var s = Mathf.PI * ItemSize * ItemSize;
        var max = Mathf.Max(ares[0], ares[1]);
        Debug.Log(string.Format("겹침 영역: {0}, 전체 크기: {1}, 퍼센트: {2}", max, s, max / s * 100));
        if (max / s > 0.5f) return true;
        else return false;
    }
    private void Update()
    {
        if (IsStay && ConditionCheck()) StayTime += Time.deltaTime;
        else StayTime = 0;
        if (StayTime > 1) //겹친지 3초뒤, 쿠키 삭제
        {
            StayTime = 0;
            IsStay = false;

            //쿠키 사운드
            AudioSource audiosource = this.gameObject.GetComponent<AudioSource>();
            audiosource.clip = CookieEatSound;
            audiosource.Play(); //쿠키 사운드
            audiosource.loop = false;
            Contents1_Manager.TotalScore += 1;

            Destroy(Target.gameObject);
            ItemLists.Remove(Target);
            Target = null;
        }
        if (ItemLists.Count != 0)
        {
            Transform targetFromLeftFoot, targetFromRightFoot;
            targetFromLeftFoot = ItemLists.Aggregate((minItem, nextItem) => AreaCalculator.CalculateDistance(LeftFoot, minItem) < AreaCalculator.CalculateDistance(LeftFoot, nextItem) ? minItem : nextItem);
            targetFromRightFoot = ItemLists.Aggregate((minItem, nextItem) => AreaCalculator.CalculateDistance(RightFoot, minItem) < AreaCalculator.CalculateDistance(RightFoot, nextItem) ? minItem : nextItem);
            var target = AreaCalculator.CalculateDistance(LeftFoot, targetFromLeftFoot) > AreaCalculator.CalculateDistance(RightFoot, targetFromRightFoot) ? targetFromRightFoot : targetFromLeftFoot;
            if (target != Target && Target != null)
            {
                Destroy(Target.gameObject.GetComponent<Outline>());
                IsStay = false;
            }
            Target = target;

            //쿠키 강조 효과
            if (target.GetComponent<Outline>() == null) target.gameObject.AddComponent<Outline>();
            ares = AreaCalculator.GetAreas(target, ItemSize);
            //Debug.Log(string.Format("{0}, {1}", ares[0], ares[1]));
        }
    }
    public void CreateDynamicItem()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        for (int cookiePos = 0; cookiePos < ItemCount; cookiePos++)
        {
            //각 쿠키의 x 위치 설정
            Vector3 TempPosition = transform.position;
            TempPosition.x -= cookiePos * StrideLength;

            //각 쿠키의 y 위치 설정
            if (cookiePos % 2 == 0) TempPosition.y += SideLength;
            else TempPosition.y -= SideLength;

            //쿠키 방향 설정
            Quaternion TempRotation = Quaternion.Euler(0, 180, 0);

            //쿠키 생성
            GameObject ItemClone = Instantiate(Fossile, TempPosition, TempRotation);

            //쿠키 생성 후, 크기 설정
            ItemClone.transform.localScale = new Vector3(ItemSize, ItemSize, ItemSize);
            ItemClone.transform.SetParent(transform, true);

            //쿠키 태그 설정
            ItemClone.tag = "Cookie";
        }
    }
    private void OnDrawGizmos()
    {
        if (AreaCalculator != null) AreaCalculator.DrawGizmo();
    }

}

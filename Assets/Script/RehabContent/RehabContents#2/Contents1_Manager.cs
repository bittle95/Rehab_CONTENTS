using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contents1_Manager : MonoBehaviour
{
    public static int TotalScore = 0;

    [Header("이펙트 관련")]
    public GameObject firstArrow;


    [Header("사운드 관련")]
    public AudioClip GuideMent;
    public EffectManager effectManager;
    protected bool Trigged = false;
    protected AudioSource audioSource;

    public static bool CurveIsDone = false;


    private void Start()
    {

        //사운드 플레이
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = GuideMent;
        audioSource.Play(); // 시작 멘트 재생
        audioSource.loop = false;

        //effectManager = new EffectManager(/*Effect*/);
        //effectManager.OffEffect();
        firstArrow.SetActive(false);
    }

    void Update()
    {
        if (audioSource.isPlaying == false && Trigged == false)  //이펙트 효과 재생
        {
            print("멘트 끝");
            //effectManager.OnEffect();
            firstArrow.SetActive(true);

            Trigged = true;
        }
    }
}

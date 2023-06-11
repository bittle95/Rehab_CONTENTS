using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Contents2_ColliderControl : MonoBehaviour
{
    public GameObject Panel;

    [Header("오브젝트 관련")]
    public Transform[] DinoSours;
    public Transform[] Arrows;
    public Transform[] Effects;
    
    [Header("인터액션 관련")]
    public EffectManager InterActionManager;
    public Transform Root; //카메라 바로 앞 위치

    [Header("사운드 관련")]
    public AudioClip CookieEatSound;
    public AudioClip EffectSound;
    public AudioClip MagicSound;
    public AudioClip[] Reactions;
    public AudioSource BGSound;
    AudioSource audiosource;

    public static int TotalIndex = 0;

    private void Start()
    {
        InterActionManager = new EffectManager();
        audiosource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Cookie")
        {
            //사운드 플레이
            audiosource.clip = CookieEatSound;
            audiosource.Play(); //쿠키 사운드
            audiosource.loop = false;

            Destroy(collision.gameObject);
        }
        if (collision.tag == "Effect")
        {
            //사운드 플레이
            audiosource.clip = EffectSound;
            audiosource.Play(); // 이펙트 사운드
            
            audiosource.loop = false;

            print(TotalIndex);
            DinoSours[TotalIndex].transform.position = Root.position;
            DinoSours[TotalIndex].transform.rotation = Root.rotation;
            DinoSours[TotalIndex].GetComponent<Animator>().SetBool("IsRoaring", true);
            StartCoroutine("DinosourCollecter"); //공룡 수집 

            Destroy(Arrows[TotalIndex].gameObject);
            Destroy(collision.gameObject); //해당 이펙트 Off
        }

        if (collision.tag == "SuccessLine" && SceneManager.GetActiveScene().name == "curves1")
            {
                Panel.SetActive(true);
                StartCoroutine("ChangScene");
            }
    }

    IEnumerator ChangScene()
    {
        yield return new WaitForSeconds(9f);
        Contents1_Manager.CurveIsDone = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))            Contents1_Manager.CurveIsDone = true;
        if (Contents1_Manager.CurveIsDone == true)
        {
            Panel.SetActive(false);
            SceneManager.LoadScene("curves2");
        }
    }

    IEnumerator DinosourCollecter()
    {
        //해당 공룡 관련 사운드 플레이
        audiosource.clip = Reactions[TotalIndex];
        audiosource.Play();

        //8초뒤 플레이
        yield return new WaitForSeconds(8f); 
        print("공룡" + TotalIndex + " 수집");
        //DinoSours[TotalIndex].GetComponent<Animator>().SetBool("IsRoaring", false);
        Destroy(DinoSours[TotalIndex].gameObject);
        TotalIndex += 1;
    }
}

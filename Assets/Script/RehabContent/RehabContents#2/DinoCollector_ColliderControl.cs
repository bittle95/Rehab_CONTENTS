using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DinoCollector_ColliderControl : MonoBehaviour
{
    public DateTime StartTime, EndTime;

    public GameObject[] ApearEffects;
    public Sprite[] GradeSprites;
    public AudioClip[] DinoRoarSounds;

    [Header("공룡 관련")]
    public DinosourManager dinosourManager;
    public GameObject[] Dinosours;
    public Transform[] InstaiateDinoPos;


    [Header("인터액션 관련")]
    public EffectManager effectManager;
    public Transform CamRoot; //카메라 바로 앞 위치
    public Transform EffectRoot; //카메라 바로 앞 위치한 이펙트 좌표

    [Header("사운드 관련")]
    public AudioClip[] Reactions;
    public AudioClip CookieEatSound;
    public AudioClip EffectSound;
    public AudioClip MagicSound;
    public AudioClip IncreaseScoreSound;
    AudioSource audiosource;
    AudioSource n_AudioSource;
    AudioSource RoarAudioSource;

    [Header("알림창 관련")]
    public GameObject EndPanel;
    public GameObject CautionPanel;
    public GameObject GradePanel;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI Discription;
    bool TryCountdownOnce = false;

    [Header("스프라이트 관련")]
    public GameObject ScoreSprite;
    public Sprite Hundred;
    public Sprite Eighty;
    public Sprite Sixty;
    public Sprite Forty;
    public Sprite Twenty;


    GameObject RandomDino;
    bool IsContact = false;

    public static int TotalIndex = 0; //Effect, Arrow를 관리하는 인덱스
    int DinoIndex = 0;  //공룡을 관리하는 인덱스`
    private void Start()
    {
        ContentConfiguration.TrainingTime = "0";
        Contents1_Manager.TotalScore = 0;

        StartTime = DateTime.Now;
        TotalIndex = 0;

        dinosourManager = new DinosourManager(Dinosours);
        effectManager = FindObjectOfType<EffectManager>();
        audiosource = this.GetComponent<AudioSource>();
        n_AudioSource = this.GetComponent<AudioSource>();
        RoarAudioSource = this.GetComponent<AudioSource>();
    }

    void ColliderEvent(Collider collision)
    {
        IsContact = true;
        if (collision.tag == "Cookie")
        {
            //사운드 플레이
            audiosource.clip = CookieEatSound;
            audiosource.Play(); //쿠키 사운드
            audiosource.loop = false;
            Contents1_Manager.TotalScore += 10;

            if (ItemManager_content2.ItemLists.Count != 0)
                ItemManager_content2.ItemLists.RemoveAt(0); //쿠키 매니저를 통해 첫번째 쿠키 관리
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Effect")
        {
            print("TotalIndex: " + TotalIndex);

            //사운드 플레이
            audiosource.clip = EffectSound;
            audiosource.PlayOneShot(audiosource.clip); // 이펙트 사운드
            audiosource.loop = false;


            StartCoroutine(DinosourAlert()); //알림. 공룡이 도망가요!~
            Destroy(collision.gameObject); //해당 이펙트 제거
        }

        if (collision.tag == "Dinosour")
        {
            StartCoroutine("DinosourIntroduce");
        }

        if (collision.tag == "SuccessLine")
        {
            var colliderOff = this.transform.GetComponent<CapsuleCollider>();
            colliderOff.enabled = false;
            FinishProcess();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        ColliderEvent(collision);
    }
    private void OnTriggerStay(Collider collision)
    {
        if (IsContact == false)
        {
            ColliderEvent(collision);
        }
    }

    IEnumerator DinosourAlert()
    {
        CautionPanel.SetActive(true);
        Discription.text = "공룡이 도망치려고 해요!" + "\n" + "도망가는 공룡을 쫓아가세요!";
        if(effectManager.Arrow_poses[TotalIndex] != null)
            Destroy(effectManager.Arrow_poses[TotalIndex].gameObject); //해당 화살표 제거

        yield return new WaitForSeconds(2f); // N 초뒤, 공룡 무빙 시작

        CautionPanel.SetActive(false);
        StartCoroutine(DinosourChase());
    }

    //공룡이 도망가는 애니메이션 및 기능
    IEnumerator DinosourChase()
    {
        //랜덤으로 공룡 배치하는 함수
        DinoIndex = dinosourManager.DinoRandomInt();

        //공룡 울음소리 활성화 (Update문 활용)
        DinoIsRoaring = true; 

        //공룡 클론 만들어서 배치하기
        RandomDino = Instantiate(Dinosours[DinoIndex], Dinosours[DinoIndex].transform.position, Dinosours[DinoIndex].transform.rotation);
        RandomDino.transform.position = InstaiateDinoPos[TotalIndex].position;
        RandomDino.transform.rotation = InstaiateDinoPos[TotalIndex].rotation;

        RandomDino.transform.SetParent(GameObject.Find("Components").transform, true);

        //공룡 콜리더 잠시 없애기
        GameObject[] DinoCollider = GameObject.FindGameObjectsWithTag("Dinosour");
        foreach (GameObject collider in DinoCollider)
        {
            collider.GetComponent<CapsuleCollider>().enabled = false;
        }

        dinoMove = true; //DinoMovingFoward() 함수 실행

        yield return null;
    }
    bool dinoMove = false;
    Coroutine coroutine; //"서두르세요! 공룡이 곧 사라집니다" 코루틴
    Coroutine Timercoroutine;

    void DinoMovingFoward() //Update 함수 안에
    {
        if (dinoMove == true)
        {
            RandomDino.transform.position = Vector3.MoveTowards(RandomDino.transform.position, InstaiateDinoPos[TotalIndex].transform.GetChild(0).position, 800f * Time.deltaTime);

            //공룡이 일정 거리 다 주행함, 한번만 실행
            if (Vector3.Distance(RandomDino.transform.position, InstaiateDinoPos[TotalIndex].transform.GetChild(0).position) <= 0.05f)
            {
                //print("완주");

                //공룡 콜리더 켜기
                GameObject[] DinoCollider = GameObject.FindGameObjectsWithTag("Dinosour");
                foreach (GameObject collider in DinoCollider)
                {
                    collider.GetComponent<CapsuleCollider>().enabled = true;
                }
                coroutine = StartCoroutine("DinoWillDisappear");
                if (InstaiateDinoPos.Length > TotalIndex) TotalIndex += 1; //out of Index 방지  //인덱스 추가
                dinoMove = false;
            }
        }
    }

    void Update()
    {
        if (DinoIsRoaring == true)
        {
            if (RoarAudioSource.isPlaying == false)  //플레이 안되고 있으면
            {
                DinoClassification_Roar(Dinosours[DinoIndex].name); //플레이 (텀을 주고 오디오 플레이하는 함수)
            }
        }

        if(Discription.text == "서두르세요!" + "\n" + "공룡이 곧 사라집니다") //새로 추가
        {
            if(TryCountdownOnce == false) //카운트다운 코루틴 한번만 실행되게끔
            {
                Timercoroutine = StartCoroutine(StartCountdown(float.Parse(ContentConfiguration.Info1)));
                TryCountdownOnce = true;
            }
        }
        else
        {
            if(Timercoroutine != null)
                StopCoroutine(Timercoroutine);
        }

        DinoMovingFoward();
    }
    IEnumerator StartCountdown(float time)
    {
        var currCountdownValue = time;
        while(currCountdownValue > 0)
        {
            TimerText.text = currCountdownValue.ToString("N0");
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            if (Discription.text == "") yield break;
        }
    }


    IEnumerator DinoWillDisappear()
    {
        CautionPanel.SetActive(true);
        TryCountdownOnce = false;
        Discription.text = "서두르세요!" + "\n" + "공룡이 곧 사라집니다";

        yield return new WaitForSeconds(float.Parse(ContentConfiguration.Info1)); // 수 초뒤, 공룡 클론 삭제 //원래 27f

        DinoIsRoaring = false; //공룡 울음소리 끄기

        Discription.text = "";
        TimerText.text = ""; StopCoroutine(Timercoroutine); Timercoroutine = null;

        RoarAudioSource.Stop();
        Destroy(RandomDino); //주행을 완주한 공룡 파괴되다.
        CautionPanel.SetActive(false);
        yield return null;
    }

    GameObject TempApearEffect;

    IEnumerator DinosourIntroduce()
    {
        DinoIsRoaring = false; //공룡 울음소리 끄기
        RoarAudioSource.Stop();

        StopCoroutine(coroutine); //없애려던 코루틴 멈춤

        Discription.text = "";
        TimerText.text = ""; StopCoroutine(Timercoroutine); Timercoroutine = null;

        CautionPanel.SetActive(false);
        TryCountdownOnce = false;

        //공룡 소개 
        if (RandomDino != null)
        {
            //잠시 발 콜리더 끄기
            effectManager.FootscolliderSetTemporary(false);

            RandomDino.transform.position = CamRoot.position;
            RandomDino.transform.rotation = CamRoot.rotation;
            RandomDino.transform.SetParent(CamRoot, true); //카메라 루트로 설정
            RandomDino.GetComponent<Animator>().SetBool("IsRoaring", true);

            DinoClassification_Grade(Dinosours[DinoIndex].name); //등급 시각화
            DinoClassification_Sound(Dinosours[DinoIndex].name);//빵빠레
            DinoClassification_ApearsEffects(Dinosours[DinoIndex].name, EffectRoot, TempApearEffect); //이펙트 효과
        }

        yield return new WaitForSeconds(3.3f); // 수 초뒤, 공룡 클론 삭제
        GradePanel.SetActive(false);
        Destroy(RandomDino);

        //발 콜리더 켜기
        effectManager.FootscolliderSetTemporary(true);

        DinoClassification_DestroyEffects(); //이펙트 효과 삭제
        DinoClassification_Score(Dinosours[DinoIndex].name);  //점수 책정
    }
    public void FinishProcess(string ment = "정상종료")
    {
        //yield return new WaitForSeconds(1.5f);
        EndTime = DateTime.Now;
        FootInformation.SaveToToalData();
        var networkManager = FindObjectOfType<ContentCommunication>();
        ContentConfiguration.TrainingTime = (EndTime - StartTime).TotalSeconds.ToString("N2");
        ContentConfiguration.Info3 = ment;
        TodayScoreManager.Content2_Score.Add(new Tuple<string, int>
            (DateTime.Now.ToString("yy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + " " + DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm"),
            Contents1_Manager.TotalScore));
        networkManager.SendContentFinish();
    }

    bool DinoIsRoaring = false;

    #region 
    public void DinoClassification_Score(String DinoName)
    {

        switch (DinoName)
        {

            case "T-Rex":
                //print("1등급");
                Contents1_Manager.TotalScore += 100;

                GameObject scoreSprite;
                scoreSprite = Instantiate(ScoreSprite, EffectRoot.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                scoreSprite.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite.GetComponent<SpriteRenderer>().sprite = Hundred;
                Destroy(scoreSprite, 3f);

                break;

            case "Triceratops":
            case "Brachiosaurus":
                //print("2등급");
                Contents1_Manager.TotalScore += 80;

                GameObject scoreSprite1;
                scoreSprite1 = Instantiate(ScoreSprite, EffectRoot.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                scoreSprite1.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite1.GetComponent<SpriteRenderer>().sprite = Eighty;
                Destroy(scoreSprite1, 3f);

                break;

            case "Stegosaurus_Red":
            case "Parasaurolophus":
                //print("3등급");
                Contents1_Manager.TotalScore += 60;

                GameObject scoreSprite2;
                scoreSprite2 = Instantiate(ScoreSprite, EffectRoot.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                scoreSprite2.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite2.GetComponent<SpriteRenderer>().sprite = Sixty;
                Destroy(scoreSprite2, 3f);

                break;

            case "Iguanodon":
            case "Pteranodon":
                //print("4등급");
                Contents1_Manager.TotalScore += 40;

                GameObject scoreSprite3;
                scoreSprite3 = Instantiate(ScoreSprite, EffectRoot.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                scoreSprite3.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite3.GetComponent<SpriteRenderer>().sprite = Forty;
                Destroy(scoreSprite3, 3f);

                break;

            case "Velociraptor":
            case "Pachycephalosaurus":
                //print("5등급");
                Contents1_Manager.TotalScore += 20;

                GameObject scoreSprite4;
                scoreSprite4 = Instantiate(ScoreSprite, EffectRoot.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                scoreSprite4.transform.SetParent(GameObject.Find("Components").transform, true);
                scoreSprite4.GetComponent<SpriteRenderer>().sprite = Twenty;
                Destroy(scoreSprite4, 3f);

                break;
        }
        n_AudioSource.clip = IncreaseScoreSound;
        n_AudioSource.PlayOneShot(n_AudioSource.clip);
        n_AudioSource.loop = false;
    }

    public void DinoClassification_Roar(String DinoName)
    {
        switch (DinoName)
        {
            case "T-Rex":
                RoarAudioSource.clip = DinoRoarSounds[0];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;

            case "Triceratops":
                RoarAudioSource.clip = DinoRoarSounds[2];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;
            case "Brachiosaurus":
                //print("2등급");
                RoarAudioSource.clip = DinoRoarSounds[1];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;

            case "Stegosaurus_Red":
                RoarAudioSource.clip = DinoRoarSounds[3];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;
            case "Parasaurolophus":
                //print("3등급");
                RoarAudioSource.clip = DinoRoarSounds[4];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;

            case "Iguanodon":
                RoarAudioSource.clip = DinoRoarSounds[5];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;
            case "Pteranodon":
                //print("4등급");
                RoarAudioSource.clip = DinoRoarSounds[6];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;

            case "Velociraptor":
                RoarAudioSource.clip = DinoRoarSounds[7];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);

                break;
            case "Pachycephalosaurus":
                RoarAudioSource.clip = DinoRoarSounds[7];
                RoarAudioSource.loop = false;
                //RoarAudioSource.PlayDelayed(3f);
                break;
        }
        RoarAudioSource.PlayDelayed(3f);
    }


    public void DinoClassification_Sound(String DinoName)
    {
        switch (DinoName)
        {
            case "T-Rex":
                //print("1등급 빵빠레");
                n_AudioSource.clip = Reactions[0];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[0];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;


            case "Triceratops":
                n_AudioSource.clip = Reactions[1];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[2];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;
            case "Brachiosaurus":
                n_AudioSource.clip = Reactions[1];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[1];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;


            case "Stegosaurus_Red":
                n_AudioSource.clip = Reactions[2];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[3];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;
            case "Parasaurolophus":
                n_AudioSource.clip = Reactions[2];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[4];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;


            case "Iguanodon":
                n_AudioSource.clip = Reactions[3];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[5];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;

            case "Pteranodon":
                n_AudioSource.clip = Reactions[3];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[6];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;


            case "Velociraptor":
                n_AudioSource.clip = Reactions[4];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[7];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;

            case "Pachycephalosaurus":
                n_AudioSource.clip = Reactions[4];
                n_AudioSource.PlayOneShot(n_AudioSource.clip);
                n_AudioSource.loop = false;

                RoarAudioSource.clip = DinoRoarSounds[7];
                RoarAudioSource.loop = false;
                RoarAudioSource.PlayDelayed(2f);
                break;
        }
    }
    public void DinoClassification_ApearsEffects(String DinoName, Transform EffectRoot, GameObject tmpAppearEffect)
    {
        switch (DinoName)
        {
            case "T-Rex":
                //print("1등급");
                TempApearEffect = Instantiate(ApearEffects[0], EffectRoot.position, EffectRoot.rotation);
                break;

            case "Triceratops":
            case "Brachiosaurus":
                //print("2등급");
                TempApearEffect = Instantiate(ApearEffects[1], EffectRoot.position, EffectRoot.rotation);
                break;

            case "Stegosaurus_Red":
            case "Parasaurolophus":
                //print("3등급");
                TempApearEffect = Instantiate(ApearEffects[2], EffectRoot.position, EffectRoot.rotation);
                break;

            case "Iguanodon":
            case "Pteranodon":
                //print("4등급");
                TempApearEffect = Instantiate(ApearEffects[3], EffectRoot.position, EffectRoot.rotation);
                break;

            case "Velociraptor":
            case "Pachycephalosaurus":
                //print("5등급");
                TempApearEffect = Instantiate(ApearEffects[4], EffectRoot.position, EffectRoot.rotation);
                break;
        }
        TempApearEffect.transform.SetParent(EffectRoot, true);
    }

    public void DinoClassification_Grade(String DinoName)
    {
        GradePanel.SetActive(true);
        switch (DinoName)
        {
            case "T-Rex":
                //print("1등급");
                GradePanel.GetComponent<RawImage>().texture = GradeSprites[0].texture;
                break;

            case "Triceratops":
            case "Brachiosaurus":
                GradePanel.GetComponent<RawImage>().texture = GradeSprites[1].texture;
                //print("2등급");
                break;

            case "Stegosaurus_Red":
            case "Parasaurolophus":
                GradePanel.GetComponent<RawImage>().texture = GradeSprites[2].texture;
                //print("3등급");
                break;

            case "Iguanodon":
            case "Pteranodon":
                GradePanel.GetComponent<RawImage>().texture = GradeSprites[3].texture;
                //print("4등급");

                break;

            case "Velociraptor":
            case "Pachycephalosaurus":
                GradePanel.GetComponent<RawImage>().texture = GradeSprites[4].texture;

                //print("5등급");
                break;
        }
    }

    public void DinoClassification_DestroyEffects()
    {
        Destroy(TempApearEffect);
    }
    #endregion
}

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

    #region 결국 하드 코딩
    //public void DinoClassification_Score(String DinoName)
    //{
    //    switch (DinoName)
    //    {
    //        case "T-Rex":
    //            //print("1등급");
    //            Contents1_Manager.TotalScore += 100;
    //            break;

    //        case "Triceratops":
    //        case "Brachiosaurus":
    //            //print("2등급");
    //            Contents1_Manager.TotalScore += 80;
    //            break;

    //        case "Stegosaurus_Red":
    //        case "Parasaurolophus":
    //            //print("3등급");
    //            Contents1_Manager.TotalScore += 60;
    //            break;

    //        case "Iguanodon":
    //        case "Pteranodon":
    //            //print("4등급");
    //            Contents1_Manager.TotalScore += 40;
    //            break;

    //        case "Velociraptor":
    //        case "Pachycephalosaurus":
    //            //print("5등급");
    //            Contents1_Manager.TotalScore += 20;
    //            break;
    //    }
    //    n_AudioSource.clip = IncreaseScoreSound;
    //    n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //    n_AudioSource.loop = false;
    //}

    //public void DinoClassification_Roar(String DinoName)
    //{
    //    switch (DinoName)
    //    {
    //        case "T-Rex":
    //            roarAudioSource.clip = DinoRoarSounds[0];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;

    //        case "Triceratops":
    //            roarAudioSource.clip = DinoRoarSounds[2];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;
    //        case "Brachiosaurus":
    //            //print("2등급");
    //            roarAudioSource.clip = DinoRoarSounds[1];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;

    //        case "Stegosaurus_Red":
    //            roarAudioSource.clip = DinoRoarSounds[3];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;
    //        case "Parasaurolophus":
    //            //print("3등급");
    //            roarAudioSource.clip = DinoRoarSounds[4];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;

    //        case "Iguanodon":
    //            roarAudioSource.clip = DinoRoarSounds[5];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;
    //        case "Pteranodon":
    //            //print("4등급");
    //            roarAudioSource.clip = DinoRoarSounds[6];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;

    //        case "Velociraptor":
    //            roarAudioSource.clip = DinoRoarSounds[7];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);

    //            break;
    //        case "Pachycephalosaurus":
    //            roarAudioSource.clip = DinoRoarSounds[7];
    //            roarAudioSource.loop = false;
    //            //RoarAudioSource.PlayDelayed(3f);
    //            break;
    //    }
    //    roarAudioSource.PlayDelayed(3f);
    //}


    //public void DinoClassification_Sound(String DinoName)
    //{
    //    switch (DinoName)
    //    {
    //        case "T-Rex":
    //            //print("1등급 빵빠레");
    //            n_AudioSource.clip = Reactions[0];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[0];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;


    //        case "Triceratops":
    //            n_AudioSource.clip = Reactions[1];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[2];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;
    //        case "Brachiosaurus":
    //            n_AudioSource.clip = Reactions[1];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[1];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;


    //        case "Stegosaurus_Red":
    //            n_AudioSource.clip = Reactions[2];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[3];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;
    //        case "Parasaurolophus":
    //            n_AudioSource.clip = Reactions[2];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[4];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;


    //        case "Iguanodon":
    //            n_AudioSource.clip = Reactions[3];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[5];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;

    //        case "Pteranodon":
    //            n_AudioSource.clip = Reactions[3];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[6];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;


    //        case "Velociraptor":
    //            n_AudioSource.clip = Reactions[4];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[7];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;

    //        case "Pachycephalosaurus":
    //            n_AudioSource.clip = Reactions[4];
    //            n_AudioSource.PlayOneShot(n_AudioSource.clip);
    //            n_AudioSource.loop = false;

    //            roarAudioSource.clip = DinoRoarSounds[7];
    //            roarAudioSource.loop = false;
    //            roarAudioSource.PlayDelayed(2f);
    //            break;
    //    }
    //}
    //public void DinoClassification_ApearsEffects(String DinoName, Transform EffectRoot, GameObject tmpAppearEffect)
    //{
    //    switch (DinoName)
    //    {
    //        case "T-Rex":
    //            //print("1등급");
    //            TempApearEffect = Instantiate(ApearEffects[0], EffectRoot.position, EffectRoot.rotation);
    //            break;

    //        case "Triceratops":
    //        case "Brachiosaurus":
    //            //print("2등급");
    //            TempApearEffect = Instantiate(ApearEffects[1], EffectRoot.position, EffectRoot.rotation);
    //            break;

    //        case "Stegosaurus_Red":
    //        case "Parasaurolophus":
    //            //print("3등급");
    //            TempApearEffect = Instantiate(ApearEffects[2], EffectRoot.position, EffectRoot.rotation);
    //            break;

    //        case "Iguanodon":
    //        case "Pteranodon":
    //            //print("4등급");
    //            TempApearEffect = Instantiate(ApearEffects[3], EffectRoot.position, EffectRoot.rotation);
    //            break;

    //        case "Velociraptor":
    //        case "Pachycephalosaurus":
    //            //print("5등급");
    //            TempApearEffect = Instantiate(ApearEffects[4], EffectRoot.position, EffectRoot.rotation);
    //            break;
    //    }
    //    TempApearEffect.transform.SetParent(EffectRoot, true);
    //}

    //public void DinoClassification_Grade(String DinoName)
    //{
    //    GradePanel.SetActive(true);
    //    switch (DinoName)
    //    {
    //        case "T-Rex":
    //            //print("1등급");
    //            GradePanel.GetComponent<RawImage>().texture = GradeSprites[0].texture;
    //            break;

    //        case "Triceratops":
    //        case "Brachiosaurus":
    //            GradePanel.GetComponent<RawImage>().texture = GradeSprites[1].texture;
    //            //print("2등급");
    //            break;

    //        case "Stegosaurus_Red":
    //        case "Parasaurolophus":
    //            GradePanel.GetComponent<RawImage>().texture = GradeSprites[2].texture;
    //            //print("3등급");
    //            break;

    //        case "Iguanodon":
    //        case "Pteranodon":
    //            GradePanel.GetComponent<RawImage>().texture = GradeSprites[3].texture;
    //            //print("4등급");

    //            break;

    //        case "Velociraptor":
    //        case "Pachycephalosaurus":
    //            GradePanel.GetComponent<RawImage>().texture = GradeSprites[4].texture;

    //            //print("5등급");
    //            break;
    //    }
    //}

    //public void DinoClassification_DestroyEffects()
    //{
    //    Destroy(TempApearEffect);
    //}
    #endregion
}

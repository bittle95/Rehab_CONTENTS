using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static int randEffectInt = 0;
    public Transform LeftFoot, RightFoot;

    [Header("이펙트 프리펩")]
    public GameObject[] Effects_Prefabs;

    [Header("포탈 Transform")]
    public Transform[] Effects_poses;

    [Header("화살표 Transform")]
    public Transform[] Arrow_poses;

    private void Start()
    {
        
    }
    private void Update()
    {
        //포탈을 그냥 지나치면 TotalIndex +=1 인것으로
        //var mPosition = (LeftFoot.position + RightFoot.position) / 2;
        //for(int t = 0; t < Effects_poses.Length; t++)
        //{
        //    if (Effects_poses[t] != null && mPosition.x < Effects_poses[t].position.x)
        //    {
        //        Destroy(Effects_poses[t].gameObject);
        //        DinoCollector_ColliderControl.TotalIndex ++;
        //        print("TotalIndex : " + DinoCollector_ColliderControl.TotalIndex);
        //    }
        //}
    }

    public void OffEffect()
    {
        for (int i = 0; i< Effects_poses.Length; i++)
        {
            Effects_poses[i].gameObject.SetActive(false);
        }
    }
    public void FootscolliderSetTemporary(bool Temp)
    {
        GameObject[] Foots = GameObject.FindGameObjectsWithTag("FootPrint");
        foreach (GameObject foots in Foots)
        {
            foots.GetComponent<CapsuleCollider>().enabled = Temp;
        }
    }

    public void OnEffect(float Info1)
    {
        System.Random randomOBJ = new System.Random();
        for (int i = 0; i < Effects_poses.Length; i++)
        {
            randEffectInt = randomOBJ.Next(0, Effects_Prefabs.Length);
            if(i % 2 == 0)
            {
                Vector3 posTemp = Effects_poses[i].position;
                posTemp.y += Info1;
                Effects_poses[i].position = posTemp;

                Vector3 posTemp2 = Arrow_poses[i].position;
                posTemp2.y += Info1;
                Arrow_poses[i].position = posTemp2;
            }
            else
            {
                Vector3 posTemp = Effects_poses[i].position;
                posTemp.y -= Info1;
                Effects_poses[i].position = posTemp;

                Vector3 posTemp2 = Arrow_poses[i].position;
                posTemp2.y -= Info1;
                Arrow_poses[i].position = posTemp2;
            }
            GameObject AppearObJ = Instantiate(Effects_Prefabs[randEffectInt], Effects_poses[i].position, Effects_poses[i].rotation);
            //AppearObJ.transform.SetParent(GameObject.Find("Components").transform, true);
            AppearObJ.transform.SetParent(Effects_poses[i].transform, true);
        }
    }

}

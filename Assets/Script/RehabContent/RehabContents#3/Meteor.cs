using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    //GameObject Rightfoot;
    //GameObject Leftfoot;
    GameObject Target;

    Vector3 targetPos;
    Vector3 myPos;

    Vector3 newPos;

    void Start()
    {
       // print(this.gameObject.name);

        Target = GameObject.Find("Target");

        targetPos = Target.transform.position; //두발 사이 위치 갱신
        myPos = transform.position;

        StartCoroutine(MoveMeteour());

    }
    IEnumerator MoveMeteour()
    {
        while(true) //혹은 그냥 true로
        {
            newPos = (targetPos - myPos) * 0.0025f; //목표 위치 갱신 //0.003
            transform.position = transform.position + newPos; //목표로 이동
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void OnDestroy()
    {
        
    }
}

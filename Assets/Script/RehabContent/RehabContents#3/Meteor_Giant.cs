using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Giant : MonoBehaviour
{


    Vector3 targetPos;
    Vector3 myPos;

    Vector3 newPos;

    void Start()
    {
        targetPos = Vector3.right;
        myPos = transform.position;

        StartCoroutine(MoveMeteour());
    }

    IEnumerator MoveMeteour()
    {
        while (true) //혹은 그냥 true로
        {
            newPos = (targetPos - myPos) * 0.0028f; //목표 위치 갱신
            transform.position = transform.position + newPos; //목표로 이동

            yield return new WaitForSeconds(0.01f);
        }
    }


    //// Update is called once per frame
    //void Update()
    //{
    //    newPos = (targetPos - myPos) * 0.002f; //목표 위치 갱신

    //    transform.position = transform.position + newPos; //목표로 이동
    //}

}

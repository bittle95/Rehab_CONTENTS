using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using UnityEngine.UI;
using System.IO;

public class RehabConent_Inital_OFFSET : MonoBehaviour
{
    public static Vector3 LeftOffset = Vector3.zero;
    public static Vector3 RightOffset = Vector3.zero;
    public static bool ConentStart = false;

    private Vector3 left_foot;
    private Vector3 right_foot;
    public GameObject left_foot_print;
    public GameObject right_foot_print;
    public GameObject LeftSample;
    public GameObject RightSample;

    public float end_flag = 0.0f;
    private float pre_left_x = 0;
    private float pre_left_y = 0;
    private float pre_right_x = 0;
    private float pre_right_y = 0;
    private int start_count = 0;
    public float timer = 0;
    private int init_flag = 0;

    public float[,] tMatrix1 = new float[4, 4] {
                { 1, 0, 0, 0.2581f },
                { 0, 0, 1, -0.1524f },
                { 0, -1, 0, 1.3361f },
                { 0, 0, 0, 1 }
     };

    private void Start()
    {
        LeftOffset = Vector3.zero;
        RightOffset = Vector3.zero;
    }
    private void Update()
    {
        if (RehabConent_Inital_OFFSET.ConentStart == true)
        {
            //1. 오프셋을 구한다
            LeftOffset = LeftSample.transform.position - left_foot_print.transform.position;
            RightOffset = RightSample.transform.position - right_foot_print.transform.position;

            //2. 구한 오프셋을 ROOT 씬에서 파란발에 적용해본다
            left_foot_print.transform.position += LeftOffset;
            right_foot_print.transform.position += RightOffset;

            //3. 잘된다면, transform_kinect_coordinat.cs에 오프셋 적용 코드를 추가한다
        }
    }

    void FixedUpdate()
    {
        // float width = data.GetComponent<checkfootprint>().stepwidth;

        //left_foot = GameObject.Find("RosConnector").GetComponent<TwistSubscriber>().linear;
        //right_foot = GameObject.Find("RosConnector").GetComponent<TwistSubscriber>().angular;
        end_flag = left_foot.z;


        if (left_foot.y == 0.0f && right_foot.y == 0.0f)
        {
            start_count = 0;
            init_flag = 0;
        }
        else
        {
            if (init_flag == 0)
            {
                pre_left_x = left_foot.x; pre_left_y = left_foot.y;
                pre_right_x = right_foot.x; pre_right_y = right_foot.y;
                init_flag = 1;
            }

            if (left_foot.y < 0.0f)
            {
                left_foot_print.SetActive(false);
                right_foot_print.SetActive(true);
                //left_foot.x = pre_left_x; left_foot.y =  pre_left_y;
            }
            else if (right_foot.y < 0.0f)
            {
                right_foot_print.SetActive(false);
                left_foot_print.SetActive(true);
                //right_foot.x = pre_right_x; right_foot.y = pre_right_y; 
            }
            else
            {
                right_foot_print.SetActive(true);
                left_foot_print.SetActive(true);
            }

            if (pre_left_y == -2f && left_foot.y != -2f) { pre_left_x = left_foot.x; pre_left_y = left_foot.y; }
            if (pre_right_y == -2f && right_foot.y != -2f) { pre_right_x = right_foot.x; pre_right_y = right_foot.y; }
            timer += Time.deltaTime;

            left_foot_print.transform.localPosition = new Vector3(
                (left_foot.x + (-0.7f)) * 1200.3f,
                (left_foot.y) * 1400f,
                left_foot_print.transform.localPosition.z);
            LeftOffset = LeftSample.transform.position - left_foot_print.transform.position;

            right_foot_print.transform.localPosition = new Vector3(
                    (right_foot.x + (-0.7f)) * 1200.3f,
                    (right_foot.y) * 1400f,
                    right_foot_print.transform.localPosition.z);
            RightOffset = RightSample.transform.position - right_foot_print.transform.position;
        }
    }

}

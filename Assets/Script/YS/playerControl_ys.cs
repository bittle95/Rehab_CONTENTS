using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl_ys : MonoBehaviour
{
    public int PlayerSpeed = 10;
    [SerializeField]
    private GameObject Player_forTest;

    void Update()
    {
#if UNITY_EDITOR
        #region 발 두개 동시
        if (Input.GetKey(KeyCode.RightArrow)) Player_forTest.transform.position = 
                new Vector3(
                    Player_forTest.transform.position.x + PlayerSpeed, 
                    Player_forTest.transform.position.y, 
                    Player_forTest.transform.position.z);

        if (Input.GetKey(KeyCode.LeftArrow)) Player_forTest.transform.position = 
                new Vector3(
                    Player_forTest.transform.position.x - PlayerSpeed, 
                    Player_forTest.transform.position.y, 
                    Player_forTest.transform.position.z);

        if (Input.GetKey(KeyCode.UpArrow)) Player_forTest.transform.position = 
                new Vector3(
                    Player_forTest.transform.position.x, 
                    Player_forTest.transform.position.y + PlayerSpeed, 
                    Player_forTest.transform.position.z);

        if (Input.GetKey(KeyCode.DownArrow)) Player_forTest.transform.position = 
                new Vector3(
                    Player_forTest.transform.position.x, 
                    Player_forTest.transform.position.y - PlayerSpeed, 
                    Player_forTest.transform.position.z);

        if (Input.GetKey(KeyCode.Space))//콘텐츠 끝
        {
        }
        if (Input.GetKey(KeyCode.Escape)) //콘텐츠 시작
        {
        }
        #endregion
#endif
    }
}

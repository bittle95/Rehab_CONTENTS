using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseShutDown : MonoBehaviour
{
    //마우스 커서를 Off 해주는 스크립트
    void Start()
    {
        if(Cursor.visible != false)
        {
            Cursor.visible = false;
        }
        var obj = FindObjectsOfType<MouseShutDown>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

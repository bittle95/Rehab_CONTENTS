using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsFunctionTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        try
        {
            if (collision.tag == "SuccessLine")
            {
                Destroy(collision.gameObject);
                print("끝");
            }
        }

        catch (Exception e)
        {
            print(e + ": 과 같은 오류뜸");
        }
    }
}
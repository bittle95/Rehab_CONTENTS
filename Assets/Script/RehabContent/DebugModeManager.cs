using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeManager : MonoBehaviour
{
    GameObject LeftFoot;
     GameObject RightFoot;

    string ServerAddress = null;
    // Start is called before the first frame update
    void Start()
    {
        ServerAddress = PlayerPrefs.GetString("Address");
        StartCoroutine(DebugMode());
    }
    IEnumerator DebugMode()
    {
        while (true)
        {
            var form1 = new WWWForm();
            var url1 = ServerAddress + "/debugmode_fromUnity";
            UnityEngine.Networking.UnityWebRequest www1 = UnityEngine.Networking.UnityWebRequest.Post(url1, form1);
            www1.redirectLimit = 10;
            yield return www1.SendWebRequest();
            if (www1.isHttpError || www1.isNetworkError)
            {
                Debug.Log(www1.error);
                Debug.Log(url1);
                www1.Dispose();
            }
            else
            {
                string debugMode_is = www1.downloadHandler.text;

                if (debugMode_is == "True") //0
                {
                    LeftFoot = GameObject.Find("tracking_foot_left_ys");
                    RightFoot = GameObject.Find("tracking_foot_right_ys");
                    //print("디버그 모드 온");
                    var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    LeftFootSR.enabled = true;
                    var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    RightFootSR.enabled = true;
                }

                else
                {
                    LeftFoot = GameObject.Find("tracking_foot_left_ys");
                    RightFoot = GameObject.Find("tracking_foot_right_ys");
                    //print("디버그 모드 오프");
                    var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    LeftFootSR.enabled = false;
                    var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    RightFootSR.enabled = false;
                }

                www1.Dispose();
                
                yield return new WaitForSeconds(2);
            }
            if (this.gameObject.name == "false")
            { break; }

        }
    }


        // Update is called once per frame
        void Update()
    {
        
    }
}

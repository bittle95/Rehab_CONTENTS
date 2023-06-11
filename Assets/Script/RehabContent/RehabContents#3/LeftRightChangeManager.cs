using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightChangeManager : MonoBehaviour
{
    [Header("발 관련")]
    public GameObject LeftFoot;
    public GameObject RightFoot;

    string ServerAddress = null;
    // Start is called before the first frame update
    void Start()
    {
        LeftFoot.gameObject.name = "None";
        RightFoot.gameObject.name = "Target";
        var LeftFootCC = LeftFoot.transform.GetComponent<CapsuleCollider>();
        LeftFootCC.enabled = false;
        var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        LeftFootSR.enabled = false;
        var RightFootCC = RightFoot.transform.GetComponent<CapsuleCollider>();
        RightFootCC.enabled = true;
        var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        RightFootSR.enabled = true;

        ServerAddress = PlayerPrefs.GetString("Address");
        StartCoroutine(LeftRightChange());
    }

    IEnumerator LeftRightChange()
    {
        while (true)
        {
            var form1 = new WWWForm();
            var url1 = ServerAddress + "/content/changebtn_fromUnity";
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
                string BtnHasPushed = www1.downloadHandler.text;
                www1.Dispose();
                if (BtnHasPushed == "True") //0
                {
                    LeftFoot.gameObject.name = "Target";
                    RightFoot.gameObject.name = "None";

                    var LeftFootCC = LeftFoot.transform.GetComponent<CapsuleCollider>();
                    LeftFootCC.enabled = true;
                    var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    LeftFootSR.enabled = true;
                    var LeftFootScript = LeftFoot.transform.GetComponent<MeteorAvoid_ColliderController>();
                    LeftFootScript.enabled = true;

                    var RightFootCC = RightFoot.transform.GetComponent<CapsuleCollider>();
                    RightFootCC.enabled = false;
                    var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    RightFootSR.enabled = false;
                    var RightFootScript = RightFoot.transform.GetComponent<MeteorAvoid_ColliderController>();
                    RightFootScript.enabled = false;
                }

                else
                {
                    LeftFoot.gameObject.name = "None";
                    RightFoot.gameObject.name = "Target";

                    var LeftFootCC = LeftFoot.transform.GetComponent<CapsuleCollider>();
                    LeftFootCC.enabled = false;
                    var LeftFootSR = LeftFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    LeftFootSR.enabled = false;
                    var LeftFootScript = LeftFoot.transform.GetComponent<MeteorAvoid_ColliderController>();
                    LeftFootScript.enabled = false;

                    var RightFootCC = RightFoot.transform.GetComponent<CapsuleCollider>();
                    RightFootCC.enabled = true;
                    var RightFootSR = RightFoot.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    RightFootSR.enabled = true;
                    var RightFootScript = RightFoot.transform.GetComponent<MeteorAvoid_ColliderController>();
                    RightFootScript.enabled = true;
                }
                yield return new WaitForSeconds(2);
            }
            if (this.gameObject.name == "false")
            { break; }

        }
    }

}

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RehabROOT : MonoBehaviour
{
    public GameObject Test;
    // Update is called once per frame
    void Update()
    {
        Test.transform.Rotate(0, 0, 0.1f, Space.Self);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("CrossWalk_Final");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("DinoCollector_Final");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("MeteorAvoid_Final");
        }
        else
        {
            print("DoNothing");
        }
    }
    string MainPort = ":9021";
    string ServerAddress = "";
    void Start()
    {
        string IP = File.ReadAllText(Path.Combine(Application.persistentDataPath, "serverIP.txt"));
        ServerAddress = IP + MainPort;
        SendMessageToServer();
    }
    void SendMessageToServer()
    {
        StartCoroutine(GetMessageProcess());
    }
    IEnumerator GetMessageProcess()
    {

        var form = new WWWForm();

        form.AddField("patient_id", "patient_id");
        form.AddField("patient_stride", "patient_stride");
        form.AddField("patient_width", "발너비");
        form.AddField("overlapping_area", "교차영역");
        form.AddField("foot_size", "발크기");
        form.AddField("chk_info", "f");

        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(ServerAddress + "/config/fin", form);
        www.redirectLimit = 10;
        yield return www.SendWebRequest();
        if (!www.isDone)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text); //이걸로 찍는다
            switch (www.downloadHandler.text)
            {
                case "content1":
                    SceneManager.LoadScene("CrossWalk_Final");
                    break;
                case "content2":
                    SceneManager.LoadScene("DinoCollector_Final");
                    break;
                case "content3":
                    SceneManager.LoadScene("MeteorAvoid_Final");
                    break;
            }
        }
        www.Dispose();
    }

}


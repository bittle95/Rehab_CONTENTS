using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContentCommunication : MonoBehaviour
{
    string ServerAddress = null;
    void Start()
    {
        ServerAddress = PlayerPrefs.GetString("Address");
        StartCoroutine(ReceiveContentStatus());
    }
    IEnumerator ReceiveContentStatus()
    {
        while (true)
        {
            var form = new WWWForm();
            var url = ServerAddress + "/content/order";
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(url, form);
            www.redirectLimit = 10;
            yield return www.SendWebRequest();
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
                Debug.Log(url);
                www.Dispose();
            }
            else
            {
                string contentType = www.downloadHandler.text;
                www.Dispose();
                if (contentType == "order") //중간 저장을 위한 명령
                {
                    //print("리셋 버튼 테스트 로그");
                    print("contentType : " + contentType);
                    string intermediateMent = "중간저장"; 
                    switch (int.Parse(ContentConfiguration.Type))
                    {
                        case 1:
                            var obj1 = FindObjectOfType<Content1_ColliderController>();
                            obj1.FinishProcess(intermediateMent);
                            break;
                        case 2:
                            var obj2 = FindObjectOfType<DinoCollector_ColliderControl>();
                            obj2.FinishProcess(intermediateMent);
                            break;
                        case 3:
                            var obj3 = FindObjectOfType<MeteorAvoid_ColliderController>();
                            obj3.FinishProcess(intermediateMent);
                            break;
                    }

                    break;
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
    public void SendContentFinish()
    {
        StartCoroutine(SendContentFinishProcess());
    }

    IEnumerator SendContentFinishProcess()
    {
        var ServerAddress = PlayerPrefs.GetString("Address");
        var form = new WWWForm();
        var url = ServerAddress + "/content/finish";

        form.AddField("L_Success_Cnt", FootInformation.SuccessLeftCount);
        form.AddField("L_Total_Cnt", FootInformation.TotalLeftCount);
        form.AddField("R_Success_Cnt", FootInformation.SuccessRightCount);
        form.AddField("R_Total_Cnt", FootInformation.TotalRightCount);
        form.AddField("Training_Time", ContentConfiguration.TrainingTime);

        //추가 항목
        form.AddField("Info1", ContentConfiguration.Info1);
        form.AddField("Info2", ContentConfiguration.Info2);
        form.AddField("Info3", ContentConfiguration.Info3);

        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(url, form);
        www.redirectLimit = 10;
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
            Debug.Log(url);
            www.Dispose();
            var fileName = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".csv";
            var path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            var data = GenerateCSVdata();
            System.IO.File.WriteAllText(path, data);
        }
        else
        {
            string isSucess = www.downloadHandler.text;
            www.Dispose();
            if (isSucess == "True")
            {
                #region 콘텐츠 정보들을 기록한 후, 초기화하는 작업
                var form1 = new WWWForm();
                var url1 = ServerAddress + "/content/resetfinish";
                UnityEngine.Networking.UnityWebRequest www1 = UnityEngine.Networking.UnityWebRequest.Post(url1, form1);
                www1.redirectLimit = 10;

                yield return www1.SendWebRequest();
                if (www1.isHttpError || www1.isNetworkError)
                {
                    Debug.Log(www1.error);
                    Debug.Log(url1);
                    www1.Dispose();
                }
                #endregion
                SceneManager.LoadScene("ROOT");
            }
            else
            {
                var fileName = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".csv";
                var path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
                var data = GenerateCSVdata();
                System.IO.File.WriteAllText(path, data);
            }
        }
    }
    public string GenerateCSVdata()
    {
        string data = "";
        data += string.Format("Patient ID,{0}\n", ContentConfiguration.Patient_id);
        data += string.Format("Stride,{0}\n", ContentConfiguration.Stride);
        data += string.Format("Width,{0}\n", ContentConfiguration.Width);
        data += string.Format("Overlapped Area,{0}\n", ContentConfiguration.Difficulty);
        data += string.Format("Foot Size,{0}\n", ContentConfiguration.Foot_size);
        data += string.Format("Content Type,{0}\n", ContentConfiguration.Type);
        data += string.Format("Total Second,{0}\n", ContentConfiguration.TrainingTime);
        data += string.Format("Left Info,{0}\n", ContentConfiguration.LeftFootInformation);
        data += string.Format("Right Info,{0}\n", ContentConfiguration.RightFootInformation);
        return data;
    }
    public IEnumerator RefreshData()
    {
        #region 콘텐츠 정보들을 기록한 후, 초기화하는 작업
        print("contentResetFresh");
        var form1 = new WWWForm();
        var url1 = ServerAddress + "/content/resetfinish";
        UnityEngine.Networking.UnityWebRequest www1 = UnityEngine.Networking.UnityWebRequest.Post(url1, form1);
        www1.redirectLimit = 10;
        yield return www1.SendWebRequest();
        if (www1.isHttpError || www1.isNetworkError)
        {
            Debug.Log(www1.error);
            Debug.Log(url1);
            www1.Dispose();
        }
        #endregion
    }
}

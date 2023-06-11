using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class IntializeCommunication : MonoBehaviour
{
    string MainPort = ":9021";
    string ServerAddress = "";
    private void Awake()
    {
        string IP = File.ReadAllText(Path.Combine(Application.persistentDataPath, "serverIP.txt"));
        ServerAddress = IP + MainPort;
        PlayerPrefs.SetString("Address", ServerAddress);
        //Debug.Log(ServerAddress);
    }
    void Start()
    {
        RehabConent_Inital_OFFSET.ConentStart = false;
        StartCoroutine(ReceiveContentInformation());
    }
    IEnumerator ReceiveContentInformation()
    {
        while (true)
        {
            var form = new WWWForm();
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.
                Post(ServerAddress + "/content/information", form);
            www.redirectLimit = 10;
            yield return www.SendWebRequest();
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
                www.Dispose();
            }
            else
            {
                string contentInformation = www.downloadHandler.text;
                Debug.Log(contentInformation);
                www.Dispose();
                if (contentInformation != "Empty")
                {
                    var jsonObject = JObject.Parse(contentInformation);
                    ContentConfiguration.Type = jsonObject.GetValue("type").ToString();
                    ContentConfiguration.Stride = jsonObject.GetValue("stride").ToString();
                    ContentConfiguration.Width = jsonObject.GetValue("width").ToString();
                    ContentConfiguration.Difficulty = jsonObject.GetValue("difficulty").ToString(); //원래 area
                    ContentConfiguration.Distance = jsonObject.GetValue("distance").ToString();

                    switch (int.Parse(ContentConfiguration.Type))
                    {
                        case 1:
                            SceneManager.LoadScene("CrossWalk_Final");
                            break;
                        case 2:
                            SceneManager.LoadScene("DinoCollector_Final");
                            break;
                        case 3:
                            SceneManager.LoadScene("MeteorAvoid_Final");
                            break;
                    }
                    RehabConent_Inital_OFFSET.ConentStart = true;
                    break;
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
}

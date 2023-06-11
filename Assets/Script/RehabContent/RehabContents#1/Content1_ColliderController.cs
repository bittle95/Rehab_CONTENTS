using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Content1_ColliderController : MonoBehaviour
{
    public DateTime StartTime, EndTime;

    public void Start()
    {
        StartTime = DateTime.Now;
    }
    public void FinishProcess(string ment = "정상종료")
    {
        EndTime = DateTime.Now;
        FootInformation.SaveToToalData();
        var networkManager = FindObjectOfType<ContentCommunication>();
        ContentConfiguration.TrainingTime = (EndTime - StartTime).TotalSeconds.ToString("N2");
        ContentConfiguration.Info3 = ment;
        TodayScoreManager.Content1_Score.Add(new Tuple<string, int>
            (DateTime.Now.ToString("yy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + " " + DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm"),
            Contents1_Manager.TotalScore));
        networkManager.SendContentFinish();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "SuccessLine")
        {
            var colliderOff = this.transform.GetComponent<CapsuleCollider>();
            colliderOff.enabled = false;
            FinishProcess();
        }
    }
}
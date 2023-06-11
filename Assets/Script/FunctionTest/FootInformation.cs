using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FootDirection
{
    LEFT, RIGHT, NONE
}

public class FootInformation : MonoBehaviour
{
    public static int TotalLeftCount = 0;
    public static int TotalRightCount = 0;
    public static int SuccessLeftCount = 0;
    public static int SuccessRightCount = 0;

    public static void Initialize()
    {
        TotalLeftCount = 0;
        TotalRightCount = 0;
        SuccessLeftCount = 0;
        SuccessRightCount = 0;
    }
    public static void SaveToToalData()
    {
        ContentConfiguration.LeftFootInformation = string.Format("{0},{1},{2}",SuccessLeftCount, TotalLeftCount, SuccessLeftCount /(float) TotalLeftCount);
        ContentConfiguration.RightFootInformation = string.Format("{0},{1},{2}", SuccessRightCount, TotalRightCount, SuccessRightCount / (float)TotalRightCount);
    }
    public static void SaveToToalData2()
    {
        TotalLeftCount = 0;
        TotalRightCount = 0;
        SuccessLeftCount = 0;
        SuccessRightCount = 0;
        ContentConfiguration.LeftFootInformation = string.Format("{0},{1},{2}", SuccessLeftCount, TotalLeftCount, SuccessLeftCount / (float)TotalLeftCount);
        ContentConfiguration.RightFootInformation = string.Format("{0},{1},{2}", SuccessRightCount, TotalRightCount, SuccessRightCount / (float)TotalRightCount);
    }


    public FootDirection Direction = FootDirection.NONE;
}

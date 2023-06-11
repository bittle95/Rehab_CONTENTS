using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawCircle : MonoBehaviour
{
    //public int segments;
    //public float xradius;
    //public float yradius;

    //LineRenderer line;

    //private void Start()
    //{
    //    line = gameObject.AddComponent<LineRenderer>();
    //    line.SetVertexCount(segments + 0);
    //    line.useWorldSpace = false;
    //    CreatePoints();
    //}

    // void CreatePoints()
    //{
    //    float x;
    //    float y;
    //    float z = 0f;

    //    float angle = 20f;

    //    for (int i = 0; i < (segments + 1); i++) 
    //    {
    //        x = Mathf.Cos(Mathf.Deg2Rad * angle) * xradius;
    //        y = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius; //?

    //        line.SetPosition(i, new Vector3(x, y, z));
    //        angle += (360f / segments);
    //    }
    //}

    #region 기존 방법
    public static Vector3 DrawGizmosCircleXY(Vector3 pos, float radius, int circleStep = 10, float ratioLastPt = 1f)
    {
        float theta, step = (2f * Mathf.PI) / (float)circleStep;
        Vector3 p0 = pos;
        Vector3 p1 = pos;
        for (int i = 0; i < circleStep; ++i)
        {
            theta = step * (float)i;
            p0.x = pos.x + radius * Mathf.Sin(theta);
            p0.y = pos.y + radius * Mathf.Cos(theta);

            theta = step * (float)(i + 1);
            p1.x = pos.x + radius * Mathf.Sin(theta);
            p1.y = pos.y + radius * Mathf.Cos(theta);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(p0, p1);
        }

        theta = step * ((float)circleStep * ratioLastPt);
        p0.x = pos.x + radius * Mathf.Sin(theta);
        p0.y = pos.y + radius * Mathf.Cos(theta);

        return p0;
    }

    private void OnDrawGizmos()
    {
        DrawGizmosCircleXY(this.transform.position, 0.5f);
    }
    #endregion
}

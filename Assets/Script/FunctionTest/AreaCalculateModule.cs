using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCalculateModule
{
    public enum Foot { Left, Right};

    public delegate void OnFootEventHandler(Foot foot, Vector3 Position);
    public OnFootEventHandler OnFootStartEventHandler;
    public OnFootEventHandler OnFootEndEventHandler;

    private Transform leftFoot, rightFoot;
    private float radiusFoot = 0;
    private float radiusItem = 0;
    private bool IsFootStay = false;

    private Transform DebugTarget = null;

    public AreaCalculateModule(Transform LeftFoot, Transform RightFoot, float footRadious)
    {
        leftFoot = LeftFoot;
        rightFoot = RightFoot;
        radiusFoot = footRadious;
    }
    public float[] GetAreas(Transform target, float targetRadious)
    {
        DebugTarget = target;
        float[] result = new float[2] { 0, 0 };
        if(leftFoot == null || rightFoot == null || target == null)
        {
            IsFootStay = false;
            return result;
        }
        var distanceL = CalculateDistance(leftFoot, target);
        var distanceR = CalculateDistance(rightFoot, target);
        radiusItem = targetRadious;
        if (radiusFoot + radiusItem >= distanceL)
        {
            result[0] = CalculateArea(radiusFoot, radiusItem, distanceL);
            if (IsFootStay == false) OnFootStartEventHandler?.Invoke(Foot.Left, leftFoot.position);
        }
        else
        {
            if (radiusFoot + radiusItem < distanceR && IsFootStay == true)
            {
                OnFootEndEventHandler?.Invoke(Foot.Left, leftFoot.position);
                IsFootStay = false;
            }
        }
        if (radiusFoot + radiusItem >= distanceR)
        {
            result[1] = CalculateArea(radiusFoot, radiusItem, distanceR);
            if (IsFootStay == false) OnFootStartEventHandler?.Invoke(Foot.Right, rightFoot.position);
        }
        else
        {
            if (radiusFoot + radiusItem < distanceL && IsFootStay == true)
            {
                OnFootEndEventHandler?.Invoke(Foot.Right, rightFoot.position);
                IsFootStay = false;
            }
        }
        return result;
    }
    public float CalculateDistance(Transform foot, Transform item)
    {
        Vector2 FootPosition = new Vector2(foot.position.x, foot.position.y);
        Vector2 ItemPosition = new Vector2(item.position.x, item.position.y);
        return Vector2.Distance(FootPosition, ItemPosition);
    }
    public float CalculateDistance(Vector3 footPosition, Vector3 itemPosition) => Vector2.Distance(new Vector2(footPosition.x, footPosition.y), new Vector2(itemPosition.x, itemPosition.y));
    float CalculateArea(float r1, float r2, float distance)
    {
        if (Mathf.Abs(r2-r1) >= distance) return Mathf.Pow(Mathf.Min(r1, r2), 2) * Mathf.PI;
        var PowR1 = Mathf.Pow(r1, 2);
        var PowR2 = Mathf.Pow(r2, 2);
        var x = Mathf.Abs((PowR2 - PowR1 - Mathf.Pow(distance, 2)) / (2 * distance));
        var y1 = Mathf.Sqrt(Mathf.Abs(PowR1 - Mathf.Pow(x, 2)));
        var y2 = -y1;
        var thetaOfR1 = Mathf.Acos(x / r1);
        var theta1 = Mathf.Abs(thetaOfR1) * 2;
        var S1 = Mathf.PI * PowR1 * theta1 / (2 * Mathf.PI);
        var S2 = y1 * x;
        var halfSphere1 = S1 - S2;
        var thetaOfR2 = Mathf.Acos(-(x-distance) / r2);
        var theta2 = Mathf.Abs(thetaOfR2) * 2;
        var S3 = Mathf.PI * PowR2 * theta2 / (2 * Mathf.PI);
        var S4 = Mathf.Abs(distance - x) * y1;
        var halfSphere2 = S3 - S4;
        return halfSphere1 + halfSphere2;
    }
    public void DrawGizmo()
    {
        if (leftFoot == null || rightFoot == null || DebugTarget == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftFoot.position, radiusFoot);
        Gizmos.DrawSphere(rightFoot.position, radiusFoot);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(DebugTarget.position, radiusItem);
    }
}

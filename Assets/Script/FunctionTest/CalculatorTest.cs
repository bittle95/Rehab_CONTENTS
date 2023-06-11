using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorTest : MonoBehaviour
{
    public Transform L, R, T;
    public AreaCalculateModule calculator;
    void Start()
    {
        calculator = new AreaCalculateModule(L, R, 1);
    }

    // Update is called once per frame
    void Update()
    {
        var resultf = calculator.GetAreas(T, 1);
        var s = Mathf.PI;
        var m = Mathf.Max(resultf[0], resultf[1]);
        Debug.Log(string.Format("{0}, {1}, {2}", m, s, m / s * 100));
    }
    private void OnDrawGizmos()
    {
        calculator?.DrawGizmo();
    }
}

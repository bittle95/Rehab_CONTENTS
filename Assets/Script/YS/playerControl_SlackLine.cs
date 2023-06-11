using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl_SlackLine : MonoBehaviour
{
    public float playerSpeed = 200;
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * playerSpeed * Time.deltaTime;
        transform.position = curPos + nextPos;

    }
}

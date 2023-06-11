using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{

    float forceGravity = 170f; //기존 300f
    private float rotSpeed = 300f; //동전: 700f;

    // Update is called once per frame
    void Update()
    {
        //this.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * forceGravity);
    }
}

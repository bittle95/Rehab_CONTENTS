using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShieldScript : MonoBehaviour
{
    [Header("사운드 관련")]
    public AudioClip MeteorExplode;
    AudioSource MeteorSource;

    [Header("프리펩 관련")]
    public GameObject Giant_ShieldEffect_Prefab;
    

    private void Start()
    {
        MeteorSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Meteor_Giant")
        {
            GameObject MeteorEffect_Giant;
            MeteorEffect_Giant = Instantiate(Giant_ShieldEffect_Prefab, collision.gameObject.transform.position, Quaternion.identity);
            MeteorEffect_Giant.transform.SetParent(GameObject.Find("Components").transform, true);

            //사운드
            MeteorSource.clip = MeteorExplode;
            MeteorSource.PlayOneShot(MeteorSource.clip); // 이펙트 사운드
            MeteorSource.loop = false;


            Destroy(collision.gameObject); //거대 운석 제거
            StartCoroutine(SetActiveFalseForceShield());
        }
    }
    IEnumerator SetActiveFalseForceShield()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }
}

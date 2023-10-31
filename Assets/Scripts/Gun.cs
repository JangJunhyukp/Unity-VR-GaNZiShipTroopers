using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    //public PhotonView pv;
    public Transform gunTransform;
    public GameObject hiteffect;
    public GameObject shotffect;

    public float recoilForce = 0.05f;
    public float recoilDuration = 0.1f;

    private Vector3 initialPosition;

    int attackdmg = 1;
    private bool isattack = false;

    float attackTime = 0f;
    float coolTime = 0.1f;

    public AudioClip gunSound;

    //int guncount = 30;

    public XRController controller;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gunTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime > coolTime)
        {
            updateAttack(controller.inputDevice);
            attackTime = 0f;
        }

    }

    IEnumerator Recoil()
    {
        float elapsedTime = 0;

        while (elapsedTime < recoilDuration)
        {
            float recoilAmount = Mathf.Lerp(0, recoilForce, elapsedTime / recoilDuration);
            Vector3 recoilVector = new Vector3(Random.Range(-recoilAmount, recoilAmount), 0, -recoilAmount);
            gunTransform.localPosition = initialPosition + recoilVector;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gunTransform.localPosition = initialPosition;
    }

    void updateAttack(InputDevice Device)
    {
        if (Device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {  //컨트롤러에 트리거버튼이 눌렸는지 안눌렸는지 체크 (눌리면 1, 안눌리면 0)
            if (triggerValue > 0.5f && !isattack) //&& guncount > 0)
            {

                GetComponent<AudioSource>().PlayOneShot(gunSound);
                Vector3 effet = transform.position + transform.forward * 1f;
                PhotonNetwork.Instantiate("shoteffect", effet, Quaternion.identity);
                isattack = true;
                StartCoroutine("Recoil");

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 500f))
                {
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            PhotonNetwork.Instantiate("hiteffect", hit.transform.position + new Vector3(0f, 0f, -2f), Quaternion.identity);//Instantiate(hiteffect, hit.transform.position + new Vector3(0f,0f,-2f), Quaternion.identity);//photonnetwork.ins
                            enemy.pv.RPC("hurt", RpcTarget.All, attackdmg);
                            enemy.pv.RPC("hitani", RpcTarget.All);
                            enemy.pv.RPC("hithit", RpcTarget.All);
                        }
                    }
                }
            }
            else if (triggerValue <= 0.5f && isattack)
            {
                isattack = false;
            }
        }
    }
}

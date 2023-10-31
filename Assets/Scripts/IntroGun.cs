using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class IntroGun : MonoBehaviour
{
    // Start is called before the first frame update
    //public PhotonView pv;
    public Transform gunTransform;
    public GameObject shotffect;

    public float recoilForce = 0.05f;
    public float recoilDuration = 0.1f;

    private Vector3 initialPosition;

    public AudioClip gunSound;

    public XRController controller;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gunTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        updateAttack(controller.inputDevice);
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
            if (triggerValue > 0.5f)
            {

                GetComponent<AudioSource>().PlayOneShot(gunSound);
                Vector3 effet = transform.position + transform.forward * 1f;
                Instantiate(shotffect, effet, Quaternion.identity);
                StartCoroutine("Recoil");
            }
        }
    }
}

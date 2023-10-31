using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramoving : MonoBehaviour
{
    public Transform cameraTransform;
    public float recoilForce = 0.05f;
    public float recoilDuration = 0.1f;

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Recoil()
    {
        float elapsedTime = 0;

        while (elapsedTime < recoilDuration)
        {
            float recoilAmount = Mathf.Lerp(0, recoilForce, elapsedTime / recoilDuration);
            Vector3 recoilVector = new Vector3(Random.Range(-recoilAmount, recoilAmount), 0, -recoilAmount);
            cameraTransform.localPosition = initialPosition + recoilVector;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = initialPosition;
    }

    public void koRecoil()
    {
        StartCoroutine(Recoil());
    }
}

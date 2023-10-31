using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Hands : MonoBehaviour
{
    public InputDeviceCharacteristics deviceCharacteristics;
    public List<GameObject> prefabController;
    public GameObject prefabHand;
    
    InputDevice Device;
    GameObject Controller;
    GameObject HandModel;
    Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        if (devices.Count > 0)
        {
            Device = devices[0];
            GameObject prefab = prefabController.Find(controller => controller.name == Device.name);
            if (prefab)
            {
                Controller = Instantiate(prefab, transform);
            }

            HandModel = Instantiate(prefabHand, transform);
            Animator = HandModel.GetComponent<Animator>();
        }
    }

    void UpdateAnimation()
    {
        if(Device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            Animator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            Animator.SetFloat("Trigger", 0);
        }

        if (Device.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            Animator.SetFloat("Grip", gripValue);
        }
        else
        {
            Animator.SetFloat("Grip", 0);
        }
    }

    void Update()
    {
        if(!Device.isValid)
        {
            Init();
        }
        else
        {
            if (HandModel)
                HandModel.SetActive(true);
            if (Controller)
                Controller.SetActive(false);
            UpdateAnimation();
        }
    }
}

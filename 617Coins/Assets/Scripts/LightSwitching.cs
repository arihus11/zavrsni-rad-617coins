using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitching : MonoBehaviour
{
    public static bool startSwitching = false;
    // Start is called before the first frame update
    public GameObject lightOn;
    void Start()
    {
        startSwitching = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSwitching)
        {
            Debug.Log("Animation on!");
            lightOn.gameObject.GetComponent<Animator>().Play("LightFlickering");
            Invoke("disableSwitch", 9f);
        }
        else
        {
            lightOn.gameObject.GetComponent<Animator>().Play("BaseLight");
        }
    }

    void disableSwitch()
    {
        startSwitching = false;
    }

}

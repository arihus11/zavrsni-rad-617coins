using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVision : MonoBehaviour
{
    private GameObject highestParent;
    // Start is called before the first frame update
    void Start()
    {
        highestParent = this.gameObject.transform.parent.gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Debug.Log("PLAYER IN SIGHT");
            highestParent.GetComponent<PatrolMovement>().chase = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Invoke("laterStopChase", 2f);
        }
    }

    void laterStopChase()
    {
        highestParent.GetComponent<PatrolMovement>().chase = false;
        //    Debug.Log("STOP CHASE");
    }

}

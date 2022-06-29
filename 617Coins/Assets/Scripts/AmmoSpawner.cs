using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{

    public GameObject ammoPrefab;
    private bool spawnSwitch = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnSwitch = false;
        ammoInstantiation();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.childCount == 0)
        {

            if (spawnSwitch == false)
            {
                Invoke("ammoInstantiation", 20f);
                spawnSwitch = true;
                Invoke("changeSwitch", 20.5f);
            }

        }
    }

    void ammoInstantiation()
    {
        GameObject newAmmo = Instantiate(ammoPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
    }

    void changeSwitch()
    {
        spawnSwitch = false;
    }

}

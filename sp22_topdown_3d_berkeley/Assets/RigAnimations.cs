using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigAnimations : MonoBehaviour
{
    public GameObject magazineInGun;
    public GameObject magazineInHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void mag_drop()
    {
        magazineInGun.SetActive(false);
        GameObject droppedMag = Instantiate(magazineInGun,magazineInGun.transform.position,magazineInGun.transform.rotation);
        droppedMag.AddComponent<Rigidbody>();
        droppedMag.GetComponent<BoxCollider>().enabled = true;
        droppedMag.SetActive(true);
        Destroy(droppedMag, 2f);
    }

    public void mag_pickup()
    {
        magazineInHand.SetActive(true);
    }

    public void mag_fit()
    {
        magazineInHand.SetActive(false);
        magazineInGun.SetActive(true);
    }
}

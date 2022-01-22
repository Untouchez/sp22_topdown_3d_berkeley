    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer LR;

    // Start is called before the first frame update
    void Start()
    {
    }

    void LateUpdate()
    {
        LR.SetPosition(0, transform.position);
        RaycastHit hit;
        if(Physics.Raycast(transform.position,-transform.up, out hit))
        {
            LR.SetPosition(1, hit.point);
        }
        else
        {
            LR.SetPosition(1, transform.forward*300f);
        }
    }
}

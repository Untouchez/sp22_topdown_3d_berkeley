using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public float yOffset;
    // Start is called before the first frame update
    void Start()
    {
        yOffset = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTarget.position.x, yOffset, followTarget.position.z);
    }
}

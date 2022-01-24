using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset;
    public float yOffset;
    // Start is called before the first frame update
    void Start()
    {
        yOffset = transform.position.y;
        offset = new Vector3(transform.position.x, yOffset, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTarget.position.x, 0, followTarget.position.z) + offset;
    }
}

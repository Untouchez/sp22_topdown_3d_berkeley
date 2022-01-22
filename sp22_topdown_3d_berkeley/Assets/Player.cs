using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Vector2 inputRaw;
    public Vector2 inputCalculated;

    public Rigidbody rb;

    public float acceleration;
    public float decceleration;

    public float movementClamp;

    public Vector3 rotDir;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

        HandleMovement();
        rotDir = new Vector3(inputCalculated.x, 0, inputCalculated.y);
        transform.rotation = Quaternion.LookRotation(  rotDir );
    }

    void HandleInputs()
    {
        inputRaw.x = Input.GetAxisRaw("Horizontal");
        inputRaw.y = Input.GetAxisRaw("Vertical");

        inputCalculated.x += inputRaw.x * acceleration * Time.deltaTime;
        inputCalculated.y += inputRaw.y * acceleration * Time.deltaTime;

        inputCalculated.x = Mathf.Clamp(inputCalculated.x, -movementClamp, movementClamp);
        inputCalculated.y = Mathf.Clamp(inputCalculated.y, -movementClamp, movementClamp);


        if(inputRaw.x == 0) 
            inputCalculated.x = Mathf.MoveTowards(inputCalculated.x, 0, decceleration*Time.deltaTime);
        if(inputRaw.y == 0)
            inputCalculated.y = Mathf.MoveTowards(inputCalculated.y, 0, decceleration*Time.deltaTime);
        
        if(inputCalculated != Vector2.zero)
        {
            anim.SetFloat("Vertical", inputCalculated.magnitude / 5);
        }
        else
        {
            anim.SetFloat("Vertical", 0);
        }
        moveSpeed = inputCalculated.magnitude / 5; 
    }

    void HandleMovement()
    {
        rb.velocity = new Vector3(inputCalculated.x, 0, inputCalculated.y);
    }

    void FootR()
    {

    }

    void FootL()
    {

    }
}

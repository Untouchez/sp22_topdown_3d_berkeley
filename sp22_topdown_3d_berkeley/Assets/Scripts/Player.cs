using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cameraLook;
    public Camera mainCamera;
    public Animator anim;
    public Rigidbody rb;

    public Vector2 inputRaw;
    public Vector2 inputCalculated;
    public Vector3 rotDir;

    public float decceleration;

    public float acceleration;
    public float lookAtAcceleration;

    public float movementClamp;
    public float lookAtMovementClamp;   

    public bool lookAt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            lookAt = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 300f))
            {
                cameraLook.position = hit.point;
            }
        }
        else
        {
            lookAt = false;
        }

        HandleInputs();

        anim.SetBool("lookAt", lookAt);

        HandleMovement();
    }

    void HandleInputs()
    {
        inputRaw.x = Input.GetAxisRaw("Horizontal");
        inputRaw.y = Input.GetAxisRaw("Vertical");

        float calculatedX;
        float calculatedY;
        if (lookAt)
        {
            calculatedX = inputRaw.x * lookAtAcceleration * Time.deltaTime;
            calculatedY = inputRaw.y * lookAtAcceleration * Time.deltaTime;

            inputCalculated.x = Mathf.Clamp(inputCalculated.x, -lookAtMovementClamp, lookAtMovementClamp);
            inputCalculated.y = Mathf.Clamp(inputCalculated.y, -lookAtMovementClamp, lookAtMovementClamp);
        }
        else
        {
            calculatedX = inputRaw.x * acceleration * Time.deltaTime;
            calculatedY = inputRaw.y * acceleration * Time.deltaTime;

            inputCalculated.x = Mathf.Clamp(inputCalculated.x, -movementClamp, movementClamp);
            inputCalculated.y = Mathf.Clamp(inputCalculated.y, -movementClamp, movementClamp);

        }

        inputCalculated.x += calculatedX;
        inputCalculated.y += calculatedY;
    }

    void HandleRotation()
    {
        if (lookAt)
        {
            transform.LookAt(cameraLook);
        } else {
            rotDir = new Vector3(inputCalculated.x, 0, inputCalculated.y);
            transform.rotation = Quaternion.LookRotation(rotDir);
        }
    }

    void HandleMovement()
    {
        if (inputRaw.x == 0)
            inputCalculated.x = Mathf.MoveTowards(inputCalculated.x, 0, decceleration * Time.deltaTime);
        else
            HandleRotation();

        if (inputRaw.y == 0)
            inputCalculated.y = Mathf.MoveTowards(inputCalculated.y, 0, decceleration * Time.deltaTime);
        else
            HandleRotation();

        if (lookAt)
        {
            if (inputCalculated != Vector2.zero)
            {
                anim.SetFloat("Vertical", inputCalculated.y);
                anim.SetFloat("Horizontal", inputCalculated.x);
            }
            else
            {
                anim.SetFloat("Vertical", 0);
                anim.SetFloat("Horizontal", 0);
            }
        }
        else
        {
            if (inputCalculated != Vector2.zero)
            {
                anim.SetFloat("Vertical", inputCalculated.magnitude / 5);
            }
            else
            {
                anim.SetFloat("Vertical", 0);
            }
        }

        rb.velocity = new Vector3(inputCalculated.x, 0, inputCalculated.y);
    }

    void FootR()
    {

    }

    void FootL()
    {

    }
}

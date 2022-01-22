using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    public Animator rigAnim;
    public MultiAimConstraint topHalfAim;
    public MultiAimConstraint weaponAim;


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

    public float rotSpeed;
    public bool lookAt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //camera look at
        HandleLookAt();

        //sets inputRaw and inputCalculated
        HandleInputs();

        //moves player and plays animation
        HandleMovement();
    }

    void HandleLookAt()
    {
        if (Input.GetMouseButton(1))
        {
            topHalfAim.weight = 1;
            weaponAim.weight = 1;
            lookAt = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 300f))
            {
                Vector3 newPos = hit.point;
                newPos.y = 1;
                cameraLook.position = newPos;
            }
        }
        else
        {

            topHalfAim.weight = 0;
            lookAt = false;
            weaponAim.weight = 0;
        }
        anim.SetBool("lookAt", lookAt);
        rigAnim.SetBool("lookAt", lookAt);
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
            HandleRotation();
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

    void HandleRotation()
    {
        if (lookAt)
        {
            RotateTowards(cameraLook);
        }
        else
        {
            rotDir = new Vector3(inputCalculated.x, 0, inputCalculated.y);
            transform.rotation = Quaternion.LookRotation(rotDir);
        }

        //https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
        void RotateTowards(Transform target)
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0;
            float angleDifference = Vector3.Angle(transform.forward, targetDirection);
            // The step size is equal to speed times frame time.
            float singleStep = rotSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);
            if (angleDifference < 30)
                return;
            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    void FootR()
    {

    }

    void FootL()
    {

    }
}

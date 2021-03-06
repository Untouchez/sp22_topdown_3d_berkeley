using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    public Animator rigAnim;
    public MultiAimConstraint topHalfAim;
    public MultiAimConstraint weaponAim;
    public WeaponHandler weaponHandler;

    public Transform mousePos;
    public Camera mainCamera;
    public Animator anim;
    public Rigidbody rb;

    public Vector2 inputRaw;
    public Vector2 inputCalculated;
    private Vector2 animVector;
    public Vector3 rotDir;

    public float decceleration;

    public float acceleration;
    public float lookAtAcceleration;

    public float movementClamp;
    public float lookAtMovementClamp;
    public float angleDifferenceMovementClamp;
    public float differenceAngle;
    public float rotSpeed;
    public bool lookAt;

    public bool isRolling;
    public float rollForce;
    public bool canRotate;
    public bool canMove;

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

        HandleRoll();
    }

    void HandleLookAt()
    {
        if (Input.GetMouseButton(1)) {
            //GUN UP
            topHalfAim.weight = 1;
            weaponAim.weight = 1;
            lookAt = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(origin: ray.origin,direction: ray.direction,out RaycastHit hit,maxDistance: 300f)) {
                Vector3 newPos = hit.point;
                mousePos.position = newPos;
            }
        } else { 
            //GUN DOWN
            topHalfAim.weight = 0;
            weaponAim.weight = 0;
            lookAt = false;
        }
        if(weaponHandler.isReloading) {
            lookAt = false;
            topHalfAim.weight = 0;
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

        if (lookAt) {
            calculatedX = inputRaw.normalized.x * lookAtAcceleration * Time.deltaTime;
            calculatedY = inputRaw.normalized.y * lookAtAcceleration * Time.deltaTime;

            inputCalculated.x = Mathf.Clamp(inputCalculated.x, -lookAtMovementClamp, lookAtMovementClamp);
            inputCalculated.y = Mathf.Clamp(inputCalculated.y, -lookAtMovementClamp, lookAtMovementClamp);
        } else {
            calculatedX = inputRaw.x * acceleration * Time.deltaTime;
            calculatedY = inputRaw.y * acceleration * Time.deltaTime;

            inputCalculated.x = Mathf.Clamp(inputCalculated.x, -movementClamp, movementClamp);
            inputCalculated.y = Mathf.Clamp(inputCalculated.y, -movementClamp, movementClamp);
        }

        if(differenceAngle > 90) {
            calculatedX /= 2;
            calculatedY /= 2;
        }
        inputCalculated.x += calculatedX;
        inputCalculated.y += calculatedY;

        rotDir = new Vector3(inputCalculated.x, 0, inputCalculated.y);

        //if im moving in the opposite direction im moving then slow down
        Vector3 temp = new Vector3(transform.forward.x, 0, transform.forward.z);
        differenceAngle = Vector3.Angle(rotDir, temp);
        if(differenceAngle > 90)
        {
            inputCalculated = Vector3.ClampMagnitude(inputCalculated, angleDifferenceMovementClamp);
        }
        animVector = inputCalculated;
        if(lookAt)
            animVector = Vector3.ClampMagnitude(inputCalculated, 1).normalized;
    }

    void HandleMovement()
    {
        if (!canMove)
            return;
        //IF NO INPUT FROM WS THEN SLOW DOWN
        if (inputRaw.x == 0)
            inputCalculated.x = Mathf.MoveTowards(inputCalculated.x, 0, decceleration * Time.deltaTime);
        else
            HandleRotation();

        //IF NO INPUT FROM AD THEN SLOW DOWN
        if (inputRaw.y == 0)
            inputCalculated.y = Mathf.MoveTowards(inputCalculated.y, 0, decceleration * Time.deltaTime);
        else
            HandleRotation();

        //IF GUN UP THEN I WANT TO PLAY MOVING ANIMATION
        if (lookAt) {
            if (inputCalculated != Vector2.zero)
            {
                anim.SetFloat("Vertical", animVector.y);
                anim.SetFloat("Horizontal", animVector.x);
            } else {
                anim.SetFloat("Vertical", 0);
                anim.SetFloat("Horizontal", 0);
            }
            HandleRotation();
        } else {
            if (inputCalculated != Vector2.zero)            
                anim.SetFloat("Vertical", animVector.y);            
            else         
                anim.SetFloat("Vertical", Mathf.MoveTowards(anim.GetFloat("Vertical"),0,0.2f));
            
        }      
        //MOVE PLAYER USING RIGIDBODY PHYSICS
        rb.velocity = new Vector3(inputCalculated.x, 0, inputCalculated.y);
    }

    void HandleRotation()
    {
        if (!canRotate)
            return;

        if (lookAt)
        {
            RotateTowards(mousePos); 
        } else {
            //ROTATE TO MOVEMENT DIRECTION
            RotateTowardsDir(rotDir);
        }
    }

    Vector3 tempDir;
    void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isRolling && inputRaw.magnitude != 0)
                StartCoroutine(StartRoll());
        }

        if (isRolling)
        {
            rb.velocity += tempDir * rollForce;
            RotateTowardsDir(tempDir);
        }
    }

    IEnumerator StartRoll()
    {
        weaponHandler.weapon.SetActive(false);
        isRolling = true;
        weaponHandler.canShoot = false;
        topHalfAim.weight = 0;
        lookAt = false;
        weaponAim.weight = 0;
        canRotate = false;
        anim.SetLayerWeight(1, 0);
        anim.Play("roll", 0, 0);
        tempDir = new Vector3(inputRaw.x, 0, inputRaw.y);

        yield return new WaitForSeconds(0.8f);

        anim.SetLayerWeight(1, 1);
        weaponHandler.weapon.SetActive(true);
        canRotate = true;
        topHalfAim.weight = 1;
        lookAt = true;
        weaponAim.weight = 1;
        weaponHandler.canShoot = true;

        isRolling = false;
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

    void RotateTowardsDir(Vector3 target)
    {
        float angleDifference = Vector3.Angle(transform.forward, target);
        // The step size is equal to speed times frame time.
        float singleStep = rotSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, target, singleStep, 0.0f);
        newDirection.y = 0;
        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);
        if (angleDifference < 30)
            return;
        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void FootR()
    {

    }

    void FootL()
    {

    }
}

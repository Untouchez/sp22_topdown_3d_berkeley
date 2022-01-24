using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public LayerMask ignore;
    public Transform mousePos;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public Transform muzzlePos;
    public LineRenderer LR;
    public float FireRate;
    private float lastFired;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (Time.time - lastFired > 1 / FireRate)
            {
                lastFired = Time.time;
                if (tempShoot != null)
                    StopCoroutine(tempShoot);
                tempShoot = StartCoroutine(Shoot());
            }
        }
    }
    Coroutine tempShoot;
    IEnumerator Shoot()
    {
        LR.enabled = true;
        muzzleFlash.Play(true);
        LR.SetPosition(0, muzzlePos.position);
        Vector3 direction = muzzlePos.forward*1000f;
        if(Physics.Raycast(origin: muzzlePos.position,direction: direction, out RaycastHit hit)) 
        {
            LR.SetPosition(1, hit.point);
            hitEffect.transform.position = hit.point;
            hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(new Vector3(90,0,0));
            hitEffect.Play(true);
            
            //foreach(Transform child in hitEffect.transform)
            //{
            //    child.GetComponent<ParticleSystem>().Emit(1);
            //}
            //hitEffect.Emit(1);
        } else {
            LR.SetPosition(1, direction);
        }
        yield return new WaitForSeconds(0.05f);
        LR.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Player player;

    public GameObject magazineInGun;
    public GameObject magazineInHand;
    public Transform hand;


    public Animator rigAnim;
    public GameObject weapon;
    public LayerMask ignore;
    public Transform mousePos;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public Transform muzzlePos;
    public LineRenderer LR;
    public int weaponDamage;
    public float FireRate;
    private float lastFired;
    public float weaponSpread;
    public bool isReloading;

    public bool canShoot;

    public float pen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canShoot || isReloading)
            return;
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

        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        player.lookAt = false;
        rigAnim.Play("reload", 0, 0);
        isReloading = true;
        yield return new WaitForSeconds(1.5f);
        isReloading = false;
    }

    Coroutine tempShoot;
    IEnumerator Shoot()
    {
        LR.enabled = true;
        muzzleFlash.Play(true);
        LR.SetPosition(0, muzzlePos.position);
        Vector3 direction = muzzlePos.forward + new Vector3(Random.Range(-weaponSpread, weaponSpread), 0, 0);
        if(Physics.Raycast(origin: muzzlePos.position,direction: direction*100f, out RaycastHit hit)) 
        {
            LR.SetPosition(1, hit.point);
            hit.transform.GetComponent<Health>()?.TakeDamage(weaponDamage);

            hitEffect.transform.position = hit.point;
            hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(new Vector3(90,0,0));
            hitEffect.Play(true);

            Vector3 exitPoint = hit.collider.ClosestPoint(hit.point + (direction * 1.1f));
            exitPoint.y = 1;
            Vector3 enterPoint = hit.point;
            enterPoint.y = 1;
            
            if(Vector3.Distance(exitPoint, hit.point) < pen) {
                Vector3 newDirection = direction + new Vector3(Random.Range(-0.2f,0.2f), 0, 0);
                if(Physics.Raycast(exitPoint, newDirection, out RaycastHit hit2))
                {
                    LR.SetPosition(2, hit2.point);
                    hit2.transform.GetComponent<Health>()?.TakeDamage(weaponDamage);

                    hitEffect.transform.position = hit2.point;
                    hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit2.normal) * Quaternion.Euler(new Vector3(90, 0, 0));
                    hitEffect.Play(true);
                } else {
                    LR.SetPosition(2, exitPoint+((newDirection) * 100f));
                }
            }else
            {
                LR.SetPosition(2, hit.point);
            }
        } else {
            LR.SetPosition(1, direction*100f);
        }
        yield return new WaitForSeconds(0.1f);
        LR.enabled = false;
    }

}

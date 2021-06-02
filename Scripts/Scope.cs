using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Animator animator;
    [SerializeField]
    private bool isScoped = false;
    private bool isReloadingAndScoping = false;
    public GameObject scopeOverlay;
    public GameObject weaponCamera;
    public Camera mainCamera;
    public float sniperFOV = 15.0f;
    public float scopedFOV = 45.0f;
    private float normalFOV=60.0f;

    private void Update()
    {
        Transform activeWeapon = FindObjectOfType<WeaponSwitching>().SelectWeapon();

        
        if (activeWeapon.GetComponentInChildren<Gun>().GetIsReloading()==false)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                //Debug.Log("samm");
                isScoped = !isScoped;
                animator.SetBool("Scoped", isScoped);

                if (isScoped)
                {

                    StartCoroutine(OnScoped());

                }
                else
                {
                    OnUnscoped();
                }
            }
        }
     

      

        if(isReloadingAndScoping)
        {
            if (isScoped && (activeWeapon.GetComponentInChildren<Gun>().GetIsReloading() == true))
            {
               
                OnUnscoped();
                
            }
            else if(isScoped && (activeWeapon.GetComponentInChildren<Gun>().GetIsReloading() == false))
            {
                
                   StartCoroutine(OnScoped());
                isReloadingAndScoping = false;
            }
         
        }
   
       
    }

    IEnumerator OnScoped()
    {
        //Transform selectedWeaponFromSwitching = FindObjectOfType<WeaponSwitching>().SelectWeapon();
       
        
       
        if (FindObjectOfType<WeaponSwitching>().SelectWeapon().gameObject.tag == "Sniper")
        {
            yield return new WaitForSeconds(0.15f);
            scopeOverlay.SetActive(true);
            weaponCamera.SetActive(false);
            //normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = sniperFOV;
        }
        else
        {
            
            //normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopedFOV;
        }
      
       
    }

    void OnUnscoped()
    {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);
        mainCamera.fieldOfView = normalFOV;
    }

    public bool GetIsScoped()
    {
        return isScoped;
    }
    public void SetIsReloadingAndScoping()
    {
        isReloadingAndScoping = true ;
    }
}

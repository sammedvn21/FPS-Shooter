using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject weaponPickupTxt;
    public GameObject weaponHolder;
    private bool inPickUpRegion = false;
    private bool isweaponPickedUp = false;
    private bool isScopedAndPickUp = false;
    private Transform weaponOnGround=null;
    private Collider otherCollider;
 
   
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (FindObjectOfType<Scope>().GetIsScoped() )
        {
            isScopedAndPickUp = true;
        }
        else
        {
            isScopedAndPickUp = false;
        }


        if(isScopedAndPickUp)
        {
            weaponPickupTxt.SetActive(false);
        }
        else if(!isScopedAndPickUp && inPickUpRegion==true)
        {
            weaponPickupTxt.SetActive(true);
        }

        if(!isScopedAndPickUp)
        {
            if (Input.GetKeyDown(KeyCode.F) && inPickUpRegion == true)
            {

                isweaponPickedUp = true;
                if (isweaponPickedUp)
                {

                    PickUpWeapon(otherCollider);
                    isweaponPickedUp = false;
                }
            }
        }
      

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WeaponPickup")
        {
           
            
                otherCollider = other;
                weaponPickupTxt.SetActive(true);
                inPickUpRegion = true;

          

        }
      
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "WeaponPickup")
        {
            
            weaponPickupTxt.SetActive(false);
            inPickUpRegion = false;
        }
    }

    public void PickUpWeapon(Collider other)
    {
        
        //Access Weapon On Ground
         weaponOnGround = other.gameObject.transform;
        
         Transform activeWeapon = null;
        //Access Acitve weapon 
        foreach (Transform key in weaponHolder.transform)
        {
            if(key.gameObject.activeSelf)
            {
                activeWeapon = key;
               
                break;
            }
        }



        //First Drop Weapon which is in Hand 
       
        Vector3 activeWeaponPosition = activeWeapon.localPosition;
        Vector3 activeWeaponRotation = activeWeapon.localEulerAngles;
        activeWeapon.SetParent(weaponOnGround.parent);
     
        activeWeapon.localPosition = weaponOnGround.localPosition;
        activeWeapon.localRotation = weaponOnGround.localRotation;

       // Debug.Log(weaponOnGround.localPosition);


        //Set Properties of weaponOnGround to activeWeapon
        activeWeapon.tag = "WeaponPickup";
        BoxCollider boxColliderOfWeapon = other.gameObject.GetComponent<BoxCollider>();
        
        
        //Destroy Box Collider weaponOnGround
        Destroy(other.gameObject.GetComponent<BoxCollider>());

        activeWeapon.gameObject.AddComponent<BoxCollider>();
        activeWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        activeWeapon.gameObject.GetComponent<BoxCollider>().center = boxColliderOfWeapon.center;
        activeWeapon.gameObject.GetComponent<BoxCollider>().size = boxColliderOfWeapon.size;
       
       
        
        //Pick Up Weapon On Ground
        other.gameObject.transform.SetParent(weaponHolder.transform);
        other.gameObject.transform.SetSiblingIndex(FindObjectOfType<WeaponSwitching>().selectedWeapon);
        other.gameObject.transform.localPosition = activeWeaponPosition;
        other.gameObject.transform.localEulerAngles = activeWeaponRotation;
        //Debug.Log(other.gameObject.transform.localEulerAngles);
        //Debug.Log(activeWeapon.position);
        if (other.gameObject.transform.name== "L96_Sniper_Rifle")
        {
            other.gameObject.transform.tag = "Sniper";
        }


        activeWeapon.GetComponentInChildren<Gun>().enabled = false;
        weaponOnGround.GetComponentInChildren<Gun>().enabled = true;




        ChangeMaterial(activeWeapon,weaponOnGround);


    }


    public void ChangeMaterial(Transform activeWeapon,Transform weaponOnGround)

    {
        SkinnedMeshRenderer skinnedMeshRendererActiveWeapon = activeWeapon.GetComponentInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer skinnedMeshRendererWeaponOnGround=weaponOnGround.GetComponentInChildren<SkinnedMeshRenderer>();
        //Debug.Log(skinnedMeshRendererActiveWeapon.gameObject + "\n" + skinnedMeshRendererWeaponOnGround.gameObject.transform.parent.name);
        int newmaterialArraySize = skinnedMeshRendererActiveWeapon.materials.Length;

        Material[] materialArrayActiveWeapon = new Material[newmaterialArraySize -1];
        Material[] materialArrayWeaponOnGround = new Material[newmaterialArraySize];
         Material hands=skinnedMeshRendererActiveWeapon.materials[2];

        for (int i = 0; i < skinnedMeshRendererActiveWeapon.materials.Length-1; i++)
        {
            materialArrayActiveWeapon[i] = skinnedMeshRendererActiveWeapon.materials[i];
           
        }
        skinnedMeshRendererActiveWeapon.materials = new Material[newmaterialArraySize - 1];
        skinnedMeshRendererActiveWeapon.materials = materialArrayActiveWeapon;


        for (int i = 0; i < materialArrayActiveWeapon.Length; i++)
        {
            materialArrayWeaponOnGround[i] = materialArrayActiveWeapon[i];
        }
        materialArrayWeaponOnGround[2] = hands;
        skinnedMeshRendererWeaponOnGround.materials = new Material[3];
        skinnedMeshRendererWeaponOnGround.materials = materialArrayWeaponOnGround;
        


    }

 

  
}

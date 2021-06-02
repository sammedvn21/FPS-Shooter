using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject weaponPickupTxt;
    public GameObject weaponHolder;
    private bool inPickUpRegion = false;
    private bool isweaponPickedUp = false;
    private bool isScopedAndPickUp = false;

    private Collider otherCollider;

    public GameObject[] realWeapons;
    public GameObject[] fakeWeapons;
    

    private void OnEnable()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Scope>().GetIsScoped())
        {
            isScopedAndPickUp = true;
        }
        else
        {
            isScopedAndPickUp = false;
        }


        if (isScopedAndPickUp)
        {
            weaponPickupTxt.SetActive(false);
        }
        else if (!isScopedAndPickUp && inPickUpRegion == true)
        {
            weaponPickupTxt.SetActive(true);
        }

        if (!isScopedAndPickUp)
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
        // Store Transforms of both Weapons
        Transform weaponOnGround = other.gameObject.transform;
       // Vector3 weaponOnGroundPosition = weaponOnGround.localPosition;
        //Vector3 weaponOnGroundRotation = weaponOnGround.localEulerAngles;
        Transform activeWeapon = null;

        //Access Acitve weapon 
        foreach (Transform key in weaponHolder.transform)
        {

            if (key.gameObject.activeSelf)
            {
                activeWeapon = key;

                break;
            }
        }

         //Search in Real Weapons ARray and Instatiate 
        GameObject weaponToBePickedUp =  SearchInRealWeapons(weaponOnGround.gameObject);
        GameObject pickedWeapon = Instantiate(weaponToBePickedUp, weaponHolder.transform);
        pickedWeapon.transform.SetSiblingIndex(activeWeapon.transform.GetSiblingIndex());
        pickedWeapon.name = weaponToBePickedUp.name;
       // pickedWeapon.GetComponent<Gun>().fpscam = Camera.main;
       
        GameObject weaponToBeDrop = SearchInFakeWeapons(activeWeapon.gameObject);
        Debug.Log(weaponToBeDrop);
        GameObject droppedWeapon=Instantiate(weaponToBeDrop,weaponOnGround.localPosition,Quaternion.Euler(weaponOnGround.localEulerAngles),weaponOnGround.parent);
        droppedWeapon.name = weaponToBeDrop.name;
        
        Destroy(weaponOnGround.gameObject);
        Destroy(activeWeapon.gameObject);




    }

    public GameObject SearchInRealWeapons(GameObject weaponOnGround)
    {
       GameObject pickableWeapon = null;

        foreach (GameObject item in realWeapons)
        {
           
            if (item.name.Equals(weaponOnGround.name))
            {
                pickableWeapon = item;
                break;

            }
        }
       // Debug.Log(pickableWeapon.name);
        return pickableWeapon; 
    }


    GameObject SearchInFakeWeapons(GameObject activeWeapon)
    {
        GameObject droppableWeapon = null;
        foreach (GameObject item in fakeWeapons)
        {
            
            if (item.name.Equals(activeWeapon.name))
            {
                droppableWeapon = item;
                break;

            }
        }
        return droppableWeapon;

    }

  

}

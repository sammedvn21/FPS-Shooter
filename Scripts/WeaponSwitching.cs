
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    Transform weaponGO;

    // Start is called before the first frame update
    void Start()
    {
        weaponGO = SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        //if (FindObjectOfType<Gun>().GetIsReloading() == false)
        //{
            if (FindObjectOfType<Scope>().GetIsScoped() == false)
            {
                int previousSelectedWeapon = selectedWeapon;

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (selectedWeapon >= (transform.childCount - 1))
                    {
                        selectedWeapon = 0;
                    }
                    else
                    {
                        selectedWeapon++;
                    }

                }


                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (selectedWeapon <= 0)
                    {
                        selectedWeapon = transform.childCount - 1;
                    }
                    else
                    {
                        selectedWeapon--;
                    }

                }

                if (previousSelectedWeapon != selectedWeapon)
                {
                    SelectWeapon();
                }

            }
       // }



    }

    public Transform SelectWeapon()
    {

        int i = 0;
        foreach (Transform weapon in transform)
        {

            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);

                weaponGO = weapon;
                //Debug.Log(weapon);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;

        }

        FindObjectOfType<PlayerMovement>().movementAnimator = weaponGO.GetComponent<Animator>();

        return weaponGO;
    }


}

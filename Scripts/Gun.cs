
using UnityEngine;
using System.Collections;
public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    private ParticleSystem muzzleFlash;
    private ParticleSystem cartridgeEffect;
    public Camera fpscam;

    public GameObject impactEffect;

    public float impactForce = 150f;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;


    //Reloading

    [SerializeField]
    private int maxAmmo = 10;
    [SerializeField]
    private int currentAmmo;
    public float reloadTime = 5.0f;
    [SerializeField]
    private bool isReloading = false;
    public Animator shootingAnimator;



    


    private void Start()
    {
        currentAmmo = maxAmmo;

    }


    private void OnEnable()
    {
        isReloading = false;
      
        shootingAnimator.SetInteger("Reload", -1);

        fpscam = Camera.main;
       
    }
    // Update is called once per frame
    void Update()
    {
        if(isReloading)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentAmmo < maxAmmo)
            {
                StartCoroutine(Reload());
                return;
            }
        }

        if (currentAmmo<=0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonUp("Fire1") )
        {
          
           // shootingAnimator.SetInteger("Movement", 0);
            shootingAnimator.SetInteger("Fire", -1);
           
        }

        if (Input.GetButton("Fire1") && Time.time>=nextTimeToFire)
        {
              
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            shootingAnimator.SetInteger("Fire", 2);
           // shootingAnimator.SetInteger("Movement", -1);
            

        }

     
       
    }

    void Shoot()
    {
        //Debug.Log("Fire");
        #region MuzzleFlash and cartridge effeect
        Transform selectedWeapon = FindObjectOfType<WeaponSwitching>().SelectWeapon();
        if (selectedWeapon.gameObject.name == "Machine Gun")
        {
            cartridgeEffect = selectedWeapon.GetChild(1).gameObject.GetComponent<ParticleSystem>();
            cartridgeEffect.Play();
            muzzleFlash = selectedWeapon.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            muzzleFlash.Play();
        }
        else
        {
           
            muzzleFlash = selectedWeapon.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            muzzleFlash.Play();
        }



        #endregion


        #region Sounds

        // shootingAnimator.SetInteger("Movement", -1);

        AudioSource weaponFireAudio = selectedWeapon.GetComponent<AudioSource>();
        weaponFireAudio.Play();

        



        #endregion

        #region MainFunction
        RaycastHit hit;
        if(Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            Target target=hit.transform.GetComponent<Target>();
            if(target!=null)
            {
                target.TakeDamage(damage,hit);
            }

            GameObject impactGO= Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);


            if(hit.rigidbody!=null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }


           
        }
        currentAmmo--;
        #endregion
    }

    IEnumerator Reload()
    {
        isReloading = true;
         FindObjectOfType<Scope>().SetIsReloadingAndScoping();
        shootingAnimator.SetInteger("Fire", -1);
        shootingAnimator.SetInteger("Reload", 1);
       
        
        yield return new WaitForSeconds(0.25f);

        AnimatorClipInfo[] animatorClipInfo= shootingAnimator.GetNextAnimatorClipInfo(0);
        //Debug.Log(animatorClipInfo.Length);
        //Debug.Log(animatorClipInfo[0].clip.name);
        shootingAnimator.SetInteger("Reload", -1);
       
        yield return new WaitForSeconds(animatorClipInfo[0].clip.length - 0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
      


        //Debug.Log(shootingAnimator.GetInteger("Movement"));

    }

    public bool GetIsReloading()
    {
        return isReloading;
    }

}

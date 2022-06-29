
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float shotgunDamage = 10f;
    public float shotgunRange = 100f;
    public float impactForce = 30f;

    public static int bullets = 3;
    private AudioSource asc;
    public AudioClip shot, reload, noAmmo, equip, ghostShot;
    public float fireRate = 30f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator shootingAnimator;

    public GameObject ghostBlood;
    public GameObject[] shellImages;
    private bool cantShoot = false;
    // Update is called once per frame

    void Start()
    {
        cantShoot = false;
        asc = this.GetComponent<AudioSource>();
        bullets = 3;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !cantShoot)
        {
            if (bullets > 0)
            {
                if (bullets == 1)
                {
                    shootingAnimator.SetBool("lastBullet", true);
                }
                Shoot();
            }
            else
            {
                shootingAnimator.Play("WeaponNoAmmo", -1, 0f);
                asc.PlayOneShot(noAmmo);
            }
        }
        if (PlayerController.collectAmmo)
        {
            PlayerController.collectAmmo = false;
            asc.PlayOneShot(equip);
            bullets = 3;
            shootingAnimator.Play("WeaponReload", -1, 0f);
            Invoke("reloadSound", 0.1f);
            shootingAnimator.SetBool("lastBullet", false);
            for (int i = 0; i < 3; i++)
            {
                if (!(shellImages[i].activeInHierarchy))
                {
                    shellImages[i].SetActive(true);
                }
            }
        }
    }

    void Shoot()
    {
        cantShoot = true;
        asc.PlayOneShot(shot);
        bullets--;
        updateShellImages();
        shootingAnimator.Play("WeaponFire", -1, 0f);

        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, shotgunRange))
        {
            //Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(shotgunDamage);
                hit.transform.GetComponent<AudioSource>().PlayOneShot(ghostShot);
                //     GameObject bloodImpact = Instantiate(ghostBlood, hit.point, Quaternion.LookRotation(hit.normal));
                //    Destroy(bloodImpact, 1.5f);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
        Invoke("reloadSound", 0.3f);
        Invoke("enableShooting", 1.0f);
    }

    void updateShellImages()
    {
        for (int i = 2; i > -1; i--)
        {
            if (shellImages[i].activeInHierarchy)
            {
                shellImages[i].SetActive(false);
                break;
            }
        }
    }

    void reloadShotgun()
    {
        PlayerController.collectAmmo = false;
        asc.PlayOneShot(equip);
        bullets = 3;
        shootingAnimator.Play("WeaponReload", -1, 0f);
        Invoke("reloadSound", 0.8f);
        shootingAnimator.SetBool("lastBullet", false);
        for (int i = 0; i < 3; i++)
        {
            if (!(shellImages[i].activeInHierarchy))
            {
                shellImages[i].SetActive(true);
            }
        }
    }

    void reloadSound()
    {
        if (bullets != 0)
        {
            asc.PlayOneShot(reload);
        }
    }

    void enableShooting()
    {
        cantShoot = false;
    }
}

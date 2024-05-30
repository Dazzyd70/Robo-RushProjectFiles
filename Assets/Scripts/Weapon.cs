using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    // public TextMeshProUGUI ammoDisplay;

    public bool isActiveWeapon;

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    public int bulletsPerBurst;
    public int burstBulletsLeft;

    public float spreadIntensity;
    public float hipspreadIntensity;
    public float adsSpreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    public Camera mainCamera;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    bool isADS;

    private int bulletsBeforeReload;

    public enum weaponModel
    {
        Pistol1911,
        M16
    }

    public weaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        readyToShoot = true;
        spreadIntensity = hipspreadIntensity;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;      
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon && !PauseManager.isPaused)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }

            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting && isReloading == false)
            {
                SoundManager.Instance.emptySound1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && isShooting && isReloading == false && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // uncomment if want to reload when empty
            //if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            //{
            //    Reload();
            //}

            
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        spreadIntensity = adsSpreadIntensity;
        mainCamera.fieldOfView = 35f;
        GlobalReferences.Instance.sensX = GlobalReferences.Instance.sensX * GlobalReferences.Instance.ADSmultiplier;
        GlobalReferences.Instance.sensY = GlobalReferences.Instance.sensY * GlobalReferences.Instance.ADSmultiplier;
        GlobalReferences.Instance.moveSpeed = GlobalReferences.Instance.moveSpeed * 0.6f;
    }

    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        spreadIntensity = hipspreadIntensity;
        mainCamera.fieldOfView = 60f;
        GlobalReferences.Instance.sensX = GlobalReferences.Instance.sensX / GlobalReferences.Instance.ADSmultiplier;
        GlobalReferences.Instance.sensY = GlobalReferences.Instance.sensY / GlobalReferences.Instance.ADSmultiplier;
        GlobalReferences.Instance.moveSpeed = GlobalReferences.Instance.moveSpeed / 0.6f;
    }

    private void FireWeapon()
    {
        bulletsLeft -= 1;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        
        if (isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }
        
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // point bullet in right direction
        bullet.transform.forward = shootingDirection;

        // shoot bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
       
        // destroy bullet
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        if (bulletsLeft > 0)
        {
            bulletsBeforeReload = magazineSize - bulletsLeft;

            SoundManager.Instance.PlayReloadSound(thisWeaponModel);
            animator.SetTrigger("RELOAD");
            isReloading = true;
            Invoke("ReloadCompleted", reloadTime);
        }

        else
        {
            bulletsBeforeReload = magazineSize;
            SoundManager.Instance.PlayReloadSound(thisWeaponModel);
            animator.SetTrigger("RELOAD");
            isReloading = true;
            Invoke("ReloadCompleted", reloadTime);
        }

        
        


    }

    private void ReloadCompleted()
    {
        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            DecreaseTotalAmmo(bulletsBeforeReload, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            DecreaseTotalAmmo(bulletsBeforeReload, thisWeaponModel);
        }

        isReloading = false;
    }


    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.weaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.weaponModel.M16:
                WeaponManager.Instance.totalRifleAmmo -= bulletsToDecrease;
                if (WeaponManager.Instance.totalRifleAmmo < 0)
                {
                    WeaponManager.Instance.totalRifleAmmo = 0;
                }
                break;

            case Weapon.weaponModel.Pistol1911:
                WeaponManager.Instance.totalPistolAmmo -= bulletsToDecrease;
                if (WeaponManager.Instance.totalPistolAmmo < 0)
                {
                    WeaponManager.Instance.totalPistolAmmo = 0;
                }
                break;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

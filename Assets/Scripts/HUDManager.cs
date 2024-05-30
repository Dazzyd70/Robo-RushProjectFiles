using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Weapon;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("HP")]
    public TextMeshProUGUI healthNumber;
    public Image damageOverlay;
    public float duration;
    public float fadeSpeed;
    private float durationTimer;

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;

    [Header("Time and Enemies")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeFinalText;
    public float timeElapsed;

    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI enemiesFinalText;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        healthNumber.text = $"{Mathf.RoundToInt(GlobalReferences.Instance.playerHealth)}";

        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.weaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.lethalsCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }

        if (WeaponManager.Instance.tacticalCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }

        timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        int hundreths = Mathf.FloorToInt((timeElapsed * 100) % 100);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundreths);
        timeFinalText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundreths);

        if (damageOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.b, damageOverlay.color.g, tempAlpha);
            }
        }

        if (GlobalReferences.Instance.playerHit == true)
        {
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.b, damageOverlay.color.g, 1);
            durationTimer = 0;
            GlobalReferences.Instance.playerHit = false;
        }


    }

    private Sprite GetWeaponSprite(Weapon.weaponModel model)
    {
        switch (model)
        {
            case Weapon.weaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;

            case Weapon.weaponModel.M16:
                return Resources.Load<GameObject>("M16_Weapon").GetComponent<SpriteRenderer>().sprite;

            //case Weapon.weaponModel.NAMEHERE:
            //    return Instantiate(Resources.Load<GameObject>("NAMEHERE_Weapon")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }
    
    private Sprite GetAmmoSprite(Weapon.weaponModel model)
    {
        switch (model)
        {
            case Weapon.weaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case Weapon.weaponModel.M16:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            //case Weapon.weaponModel.NAMEHERE:
            //    return Instantiate(Resources.Load<GameObject>("NAMEHERE_Ammo")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }

        return null;
    }

    internal void UpdateThrowables()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalCount}";

        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }

    public void UpdateEnemiesKilled(int enemiesRemaining)
    {
        enemiesText.text = $"Enemies remaining: {enemiesRemaining}";
    }
}

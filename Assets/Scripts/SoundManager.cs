using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    
    public AudioClip M16Shot;
    public AudioClip P1911Shot;

    public AudioSource reloadingSound1911;
    public AudioSource reloadingSoundM16;

    public AudioSource emptySound1911;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip EnemyShot;

    public AudioSource EnemySounds;
    public AudioClip EnemyTakeDamage;

    public AudioClip EnemyDeath;

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

    public void PlayEnemyShootingSound ()
    {
        ShootingChannel.PlayOneShot(EnemyShot);
    }

    public void PlayEnemyTakeDamageSound()
    {
        throwablesChannel.PlayOneShot(EnemyTakeDamage);
    }

    public void PlayEnemyDeathSound()
    {
        throwablesChannel.PlayOneShot(EnemyDeath);
    }

    public void PlayShootingSound(weaponModel weapon)
    {
        switch(weapon)
        {
            case weaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case weaponModel.M16:
                ShootingChannel.PlayOneShot(M16Shot);
                break;
        }
    }

    public void PlayReloadSound(weaponModel weapon)
    {
        switch (weapon)
        {
            case weaponModel.Pistol1911:
                reloadingSound1911.Play();
                break;
            case weaponModel.M16:
                reloadingSoundM16.Play();
                break;
        }
    }
}

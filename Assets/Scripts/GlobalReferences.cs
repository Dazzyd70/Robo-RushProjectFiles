using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; }


    public float moveSpeed = 11;
    public float sensX = 600f;
    public float sensY = 600f;
    public float ADSmultiplier = 0.3f;
    public float playerHealth;
    public float enemyDamage;

    public GameObject bulletImpactEffectPrefab;
    public GameObject grenadeExplosionEffect;
    public GameObject smokeGrenadeEffect;
    public GameObject deathEffect;

    public bool playerHit = false;

    private void Awake()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}

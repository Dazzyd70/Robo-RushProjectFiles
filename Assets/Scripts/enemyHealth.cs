using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isStandardGamemode;
    public bool isEndlessGamemode;

    public GameObject deathEffectHolder;
    public EndlessManager script;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject endlessManagerObj = GameObject.FindWithTag("EndlessManager");
        if (endlessManagerObj != null)
        {
            script = endlessManagerObj.GetComponent<EndlessManager>();
        }
        else
        {
            script = null;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {          
            

            SoundManager.Instance.PlayEnemyDeathSound();
            Die();
        }
        else if (currentHealth > 0)
        {
            SoundManager.Instance.PlayEnemyTakeDamageSound();
        }
    }

    public void Die()
    {
        if (isStandardGamemode)
        {
            StandardManager.Instance.EnemiesKilled += 1;
            int left = 5 - StandardManager.Instance.EnemiesKilled;
            HUDManager.Instance.UpdateEnemiesKilled(left);
        }
        else if (!isStandardGamemode)
        {
            EndlessManager.enemiesKilled += 1;
            script.spawnEnemy();
        }
        GameObject deathEffect = GlobalReferences.Instance.deathEffect;
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        

    }
}

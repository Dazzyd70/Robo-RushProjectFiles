using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damageToTarget;
    public bool isOnPlayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isOnPlayer)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + " !");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bottle"))
        {
            print("hit a bottle");
            collision.gameObject.GetComponent<Bottle>().Explode();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") && !isOnPlayer)
        {
            print("owch");
            Destroy(gameObject);
            GlobalReferences.Instance.playerHealth -= damageToTarget;
            GlobalReferences.Instance.playerHit = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHealth enemyHP = collision.collider.GetComponent<enemyHealth>();

            if (enemyHP != null)
            {
                enemyHP.TakeDamage(damageToTarget);
            }

            Destroy(gameObject);
        }

    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)

            );
        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}

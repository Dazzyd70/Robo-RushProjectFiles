using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public AiSensor sensor;

    [Header("General")]
    public NavMeshAgent agent;
    public Transform player;
    public float localDifficulty;
    public float difficultyModifier;

    public LayerMask whatIsGround, whatIsPlayer;
    public float health; 

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    public float timeBetweenBurstShots;
    public int burstAmount;
    bool alreadyAttacked;
    public GameObject projectile;
    public float projectileLifeTime;
    
    public GameObject bulletSpawnLeft;
    public GameObject bulletSpawnRight;
    public bool whichGun;

    public float spreadIntensity;
    public Transform projectileSpawnLeft;
    public Transform projectileSpawnRight;
    public bool randomizeBurstAmount;

    public float damageToPlayer;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool patrol;
    public int patrolTimer;

    public bool strafePointSet;
    public float strafePointRange;
    public Vector3 strafePoint;

    private AiSensor aiSensor;




    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<AiSensor>();
        agent.updateRotation = false;
        
        alreadyAttacked = false;
        patrol = true;
        sensor = GetComponent<AiSensor>();
        whichGun = false;

        localDifficulty = PlayerPrefs.GetInt("AIDifficulty");
        

        if (localDifficulty == 0)
        {
            difficultyModifier = 1f;
        }
        else if (localDifficulty == 1)
        {
            difficultyModifier = 1.3f;
        }
        else if (localDifficulty == 2)
        {
            difficultyModifier = 1.8f;
        }
        UpdateDifficulty();

    }

    private void Update()
    {
        playerInSightRange = sensor.IsInSight(player.gameObject);

        if (playerInSightRange)
        {
            AttackPlayer();
            patrol = false;
        }
        else
        {
            Patrolling();
            patrol = true;
        }

        if (patrol)
        {
            patrolTimer += 1;
            if (patrolTimer > 2000)
            {
                patrolTimer = 0;
                walkPointSet = false;
            } 
        }
    }

    private void UpdateDifficulty()
    {
        spreadIntensity = spreadIntensity / difficultyModifier;
        timeBetweenAttacks = (timeBetweenAttacks * 1.5f) - difficultyModifier;
        agent.speed = 11f * difficultyModifier;
        agent.acceleration = 8f * difficultyModifier;
        sensor.angle = 80 * difficultyModifier;
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            

        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            RotateTowards(agent.velocity);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
        
        
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Strafe()
    {
        if (!strafePointSet)
        {
            SearchStrafePoint();
        }

        if (strafePointSet)
        {
            agent.SetDestination(strafePoint);
        }

        Vector3 distanceToStrafePoint = transform.position - strafePoint;
        if (distanceToStrafePoint.magnitude < 1f)
        {
            strafePointSet = false;
        }
    }

    private void AttackPlayer()
    {
        Strafe();

        // agent.SetDestination(transform.position);
        RotateToFacePlayer();

        if (!alreadyAttacked)
        {
            
            StartCoroutine(fireBurst());

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private IEnumerator fireBurst()
    {
        if (randomizeBurstAmount)
        {
            int randomNumber = Random.Range(-2, 2);
            int thisBurst = burstAmount + randomNumber;

            for (int i = 0; i < thisBurst; i++)
            {
                GameObject bullet;

                if (whichGun)
                {
                    SoundManager.Instance.PlayEnemyShootingSound();
                    bullet = Instantiate(projectile, projectileSpawnLeft.position, Quaternion.identity);

                    Vector3 shootingDirection = CalculateDirectionAndSpreadLeft().normalized;

                    bullet.transform.forward = shootingDirection;
                    bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * 40f, ForceMode.Impulse);

                    whichGun = false;
                    StartCoroutine(DestroyBulletAfterTime(bullet, projectileLifeTime));
                }
                else if (!whichGun)
                {
                    SoundManager.Instance.PlayEnemyShootingSound();
                    bullet = Instantiate(projectile, projectileSpawnRight.position, Quaternion.identity);

                    Vector3 shootingDirection = CalculateDirectionAndSpreadRight().normalized;

                    bullet.transform.forward = shootingDirection;
                    bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * 40f, ForceMode.Impulse);

                    whichGun = true;
                    StartCoroutine(DestroyBulletAfterTime(bullet, projectileLifeTime));
                }

               

                yield return new WaitForSeconds(timeBetweenBurstShots);
            }
        }
        else
        {
            for (int i = 0; i < burstAmount; i++)
            {
                GameObject bullet;

                if (whichGun)
                {
                    SoundManager.Instance.PlayEnemyShootingSound();
                    bullet = Instantiate(projectile, projectileSpawnLeft.position, Quaternion.identity);

                    Vector3 shootingDirection = CalculateDirectionAndSpreadLeft().normalized;

                    bullet.transform.forward = shootingDirection;
                    bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * 40f, ForceMode.Impulse);

                    whichGun = false;
                    StartCoroutine(DestroyBulletAfterTime(bullet, projectileLifeTime));
                }
                else if (!whichGun)
                {
                    SoundManager.Instance.PlayEnemyShootingSound();
                    bullet = Instantiate(projectile, projectileSpawnRight.position, Quaternion.identity);

                    Vector3 shootingDirection = CalculateDirectionAndSpreadRight().normalized;

                    bullet.transform.forward = shootingDirection;
                    bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * 40f, ForceMode.Impulse);

                    whichGun = true;
                    StartCoroutine(DestroyBulletAfterTime(bullet, projectileLifeTime));
                }

                

                yield return new WaitForSeconds(timeBetweenBurstShots);
            }
        }  
    }

    private Vector3 CalculateDirectionAndSpreadLeft()
    {
        Vector3 direction = player.position - bulletSpawnLeft.transform.position;

        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }

    private Vector3 CalculateDirectionAndSpreadRight()
    {
        Vector3 direction = player.position - bulletSpawnRight.transform.position;

        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void SearchStrafePoint()
    {
        float randomX = Random.Range(-strafePointRange, strafePointRange);
        float randomZ = Random.Range(-strafePointRange, strafePointRange);

        strafePoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(strafePoint, -transform.up, 2f, whatIsGround))
        {
            strafePointSet = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void RotateToFacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 25f);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

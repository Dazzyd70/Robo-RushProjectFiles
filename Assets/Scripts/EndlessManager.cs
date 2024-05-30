using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessManager : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI enemiesTextFinal;
    public static int enemiesKilled = 0;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    void Awake()
    {
        enemiesKilled = 0;
    }

    void Update()
    {
        enemiesKilledText.text = $"Enemies killed: {enemiesKilled}";
        enemiesTextFinal.text = $"But you got\n {enemiesKilled} \nof them !!";

        if (GlobalReferences.Instance.playerHealth <= 0)
        {
            PauseManager.isLostEndless = true;
        }
    }

    public void spawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        if (spawnPoints.Length == 0)
        {
            return;
        }

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

    }
}

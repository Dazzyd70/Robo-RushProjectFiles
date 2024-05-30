using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardManager : MonoBehaviour
{
    public static StandardManager Instance { get; set; }

    public int EnemiesKilled;
    public int EnemiesLeft = 5;


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

    void Update()
    {
        EnemiesLeft = 5 - EnemiesKilled;

        if (EnemiesKilled == 5)
        {
            WinGameBool();
        }

        if (GlobalReferences.Instance.playerHealth <= 0)
        {
            PauseManager.isLostStandard = true;
        }
    }

    public void WinGameBool()
    {
        PauseManager.isWon = true;
    }


}

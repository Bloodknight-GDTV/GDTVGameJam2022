using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    int dungeonLevel = 0;
    int playerScore = 0;
    int playerLives = 5;
    int playerhealth = 100;


    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void ResetData()
    {
        dungeonLevel = 0;
        playerScore = 0;
        playerLives = 5;
        playerhealth = 100;
    }

    public void takeDamage(int value)
    {
        playerhealth -= value;
        if (playerhealth < 0)
        {
            playerLives -= 1;
            if (playerLives <= 0)
            {
                Debug.Log("Apparently death wasnt the beginning after all, bummer");
            }
        }
    }
}

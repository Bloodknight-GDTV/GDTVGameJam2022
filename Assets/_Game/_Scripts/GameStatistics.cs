using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    public int dungeonLevel = 0;
    public int playerScore = 0;
    public int playerLives = 5;
    public int playerhealth = 100;

    public string GetDungeonLevel()
    {
        return dungeonLevel.ToString();
    }

    public string GetPlayerScore()
    {
        return playerScore.ToString();
    }

    public string GetPlayerLives()
    {
        return playerLives.ToString();
    }

    public void LevelComplete()
    {
        dungeonLevel += 1;
    }




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

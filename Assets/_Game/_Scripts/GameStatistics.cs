using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    public int dungeonLevel = 0;
    public int playerScore = 0;
    public int playerLives = 5;
    public int playerhealth = 100;

    public static GameStatistics Instance;
    void Awake()
    {
        if (Instance) // is there already an Instance assigned?
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

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

    public void IncreaseScore(int value)
    {
        playerScore += value;
    }

    public void LevelComplete()
    {
        dungeonLevel += 1;
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

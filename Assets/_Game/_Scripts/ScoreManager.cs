using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    GameStatistics gameStatistics;
    [SerializeField] TMP_Text Lives;
    [SerializeField] TMP_Text Score;
    [SerializeField] TMP_Text Level;
    // Start is called before the first frame update
    void Start()
    {
        gameStatistics = GetComponentInChildren<GameStatistics>();

        Lives.text = gameStatistics.GetPlayerLives();
        Score.text = gameStatistics.GetPlayerScore();
        Level.text = gameStatistics.GetDungeonLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

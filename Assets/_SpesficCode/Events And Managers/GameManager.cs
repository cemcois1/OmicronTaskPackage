using System;
using _SpesficCode.UI;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public  class GameManager:MonoBehaviour
{
    public static Action LevelStarted;
    public static Action levelWined;
    public static Action levelFailed;
    public static Action<int> PrepareLevel;

    public static GameManager instance;
    public bool LevelFinished;



    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        LevelStarted += LevelStart;
        PrepareLevel += PrepareLevelMethod;
        levelWined += LevelWin;
        levelFailed += LevelFailed;
    }

    private void PrepareLevelMethod(int lvlCount)
    {
        LevelFinished = false;
    }

    private void OnDisable()
    {
        LevelStarted -= LevelStart;
        levelWined -= LevelWin;
        levelFailed -= LevelFailed;
    }

    private void LevelFailed()
    {
        LevelFinished = true;
    }
    private void LevelWin()
    {
        LevelFinished = true;
    }
    private void LevelStart()
    {
        LevelFinished = false;
    }
}
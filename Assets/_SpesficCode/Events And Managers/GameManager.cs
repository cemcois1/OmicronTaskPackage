using System;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public  class GameManager:MonoBehaviour
{
    public static Action LevelStarted;
    public static Action levelWined;
    public static Action levelFailed;
    public static Action<int> PrepareLevel;
}
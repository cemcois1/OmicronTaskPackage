using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartInitializer : MonoBehaviour
{
    private void Start()
    {
        GameManager.LevelStarted?.Invoke();
    }
}
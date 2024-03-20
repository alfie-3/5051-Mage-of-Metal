using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static GameManager Instance;
    [Header("Settings")]
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public int difficulty = 0;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
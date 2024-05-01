//Control script for game settings and 

using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game manager instance")]
    public static GameManager Instance;

    [Header("Settings")]
    public static float musicVolume = 0.5f;
    public static float sfxVolume = 0.5f;
    public static int difficulty = 0;

    public static int finalScore = 0;
    public static bool isLeaderboard = true;

    [Header("Instance checker")]
    private bool isLoaded=false;

    //Check if instance already exists
    private void Awake()
    {
        if (!isLoaded)
        {
            //isLoaded will be false for any new GameManager instance
            //If a GameManager object exists already this instance will destroy itself
            if (GameObject.FindGameObjectsWithTag("MainGameManager").Length > 1)
            {
                Destroy(gameObject);
            }
        }
        isLoaded = true;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Volume settings
    public void MusicChange(float volume)
    {
        musicVolume = volume;
        if (AudioManager.managerInstance != null)
        {
            AudioManager.managerInstance.musicInstance.setVolume(musicVolume);
        }
    }
    public void SFXChange(float volume)
    {
        sfxVolume = volume;
    }
    #endregion
}
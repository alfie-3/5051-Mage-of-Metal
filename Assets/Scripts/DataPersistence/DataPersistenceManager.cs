using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    //[Header("Fart")]
    //public GameObject test;
    [SerializeField] public FileDataHandler dataHandler; // private

    public static DataPersistenceManager instance { get; private set; }

    [SerializeField] bool loadFresh = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        //print("Fart test lol");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    private void Start()
    {
        //print("Found persistent objects. ");
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        loadFresh = true;
        this.gameData = new GameData();
        //FindObjectOfType<SceneLoader>().LoadScene(0);
        //SceneManager.LoadSceneAsync(0);
    }

    public void LoadGame()
    {
        //FindObjectOfType<SceneLoader>().LoadScene(0);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects(); // TEST
        // Load any saved data
        this.gameData = dataHandler.Load();
        // If no data, initialise new game
        if (this.gameData == null)
        {
            Debug.Log("No data found. Initialising to defaults.");
            NewGame();
        }
        // Push data to all scripts that require it.
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        //print("Loaded coins: " + gameData.astroBux);
    }

    public void SaveGame()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects(); // TEST
        // Pass data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        //print("Saved coins: " + gameData.astroBux);
        // save data to file using data handler
        dataHandler.Save(gameData);
        loadFresh = false;
        print("Saved game.");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!loadFresh)
            LoadGame();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

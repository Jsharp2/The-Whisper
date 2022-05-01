using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string nextSpawnPoint;

    public GameObject player;

    public Vector3 nextPostion;
    public Vector3 lastPostion;

    public string scenetoLoad;
    public string lastScene;

    public string previousScene;

    public Dictionary<string, int> healthRemaining = new Dictionary<string, int>();
    public Dictionary<string, int> magicRemaining = new Dictionary<string, int>();

    public List<GameObject> enemiestoSave = new List<GameObject>();

    public GameObject enemytoDestory;

    public bool hasKey = false;

    public Text test;
    //public Dictionary<string, int> exp= new Dictionary<string, int>();

    //The different states the game is in
    public enum GameStates
    {
        WORLD_STATE,
        TOWN_STATE,
        BATTLE_STATE,
        IDLE
    }

    //Maximum amount of enemies allowed to be spawned for a fight
    public int enemyAmount;

    //A list of enemies that are being spawned
    public List<GameObject> enemies = new List<GameObject>(); 

    //The current encounter area for the player
    public RegionData curRegions;

    public GameStates gameStates;

    public float enounter = 10000;

    public float multiHitMultiplier = .75f;

    //Ensures that if an instance doesn't exist, one is made. If one does exists, removes it
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //Hanles pause menu (might be changed slightly during fights)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.GoToMenu(MenuName.Pause);
        }

        //Handles the states of the game
        switch (gameStates)
        {
            //If in the overworld and walking, see if an encounter will happen
            case (GameStates.WORLD_STATE):
                break;
            case (GameStates.TOWN_STATE):
                break;
            case (GameStates.BATTLE_STATE):
                StartBattle();

                gameStates = GameStates.IDLE;
                break;
            case (GameStates.IDLE):
                break;
        }
    }

    //Loads the scene told to load
    public void LoadScene()
    {
        SceneManager.LoadScene(scenetoLoad);
    }

    //Loads the area the character just was in prior to battle
    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    //Handles getting the battle started
    public void StartBattle()
    {
        //Gets a random amount of enemies for the fight
        enemyAmount = Random.Range(1, curRegions.maxEnemies + 1);

        //For each enemy gets a random enemy that would be fightable
        for(int i = 0; i < enemyAmount; i++)
        {
            enemies.Add(curRegions.possibleEnemies[Random.Range(0, curRegions.possibleEnemies.Count)]);
        }

        //Saves player's location in the world
        lastPostion = GameObject.Find("Player").gameObject.transform.position;
        nextPostion = lastPostion;
        lastScene = SceneManager.GetActiveScene().name;

        //Loads the battle
        SceneManager.LoadScene(curRegions.battleScene);

        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Spawned Enemies");
        foreach (GameObject enem in spawnedEnemies)
        {
            if (enem != gameObject)
            {
                enemiestoSave.Add(enem);
                enem.SetActive(false);
            }
        }

    }

    public float Effectiveness(Type attackType, Type targetType)
    {
        if ((attackType == Type.AUQAS && targetType == Type.IGNIS) ||
            (attackType == Type.IGNIS && targetType == Type.NATURA) ||
            (attackType == Type.NATURA && targetType == Type.AUQAS) ||
            (attackType == Type.LUMON && targetType == Type.IGNIS) || (attackType == Type.LUMON && targetType == Type.NATURA) || (attackType == Type.LUMON && targetType == Type.LUMON) ||
            (attackType == Type.MORTON && targetType == Type.LUMON))
        {
            return 1.5f;
        }
        else if ((attackType == Type.AUQAS && targetType == Type.AUQAS) || (attackType == Type.AUQAS && targetType == Type.NATURA) || (attackType == Type.AUQAS && targetType == Type.MORTON) ||
            (attackType == Type.IGNIS && targetType == Type.IGNIS) || (attackType == Type.IGNIS && targetType == Type.AUQAS) || (attackType == Type.AUQAS && targetType == Type.MORTON) ||
            (attackType == Type.NATURA && targetType == Type.IGNIS) || (attackType == Type.NATURA && targetType == Type.NATURA) || (attackType == Type.NATURA && targetType == Type.MORTON) ||
            (attackType == Type.LUMON && targetType == Type.IGNIS) || (attackType == Type.LUMON && targetType == Type.NATURA) || (attackType == Type.LUMON && targetType == Type.LUMON) ||
            (attackType == Type.MORTON && targetType == Type.LUMON))
        {
            return (2f / 3);
        }
        else
        {
            return 1;
        }
    }

    public void DeleteEnemies()
    {
        foreach (GameObject enem in GameObject.FindGameObjectsWithTag("Spawned Enemies"))
        {
            enem.SetActive(false);
            Destroy(enem);
        }
    }
}

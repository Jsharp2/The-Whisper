using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    //An enum to handle what actions need to happen at a given time
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        CHECKALIVE,
        WIN,
        LOSE,
        ESCAPE
    }

    //An enum that controls what appears when 
    public enum HEROGUI
    {
        ACTIVATE,
        WAITING,
        DONE
    }

    public PerformAction battleStates;
    public HEROGUI heroInput;

    public List<HandleTurns> PerformList = new List<HandleTurns>();
    public List<GameObject> Heros = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();

    public List<GameObject> HerosToManage = new List<GameObject>();
    private HandleTurns playerChoice;

    public GameObject enemyButton;
    public Transform spacer;

    public GameObject heroPanel;
    public Transform heroSpacer;

    public GameObject attackPanel;
    public GameObject enemySelectPanel;
    public GameObject magicPanel;

    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject baseMagicButton;

    public Type attackType;

    public GameObject spellPanel;
    private List<GameObject> atkBtns = new List<GameObject>();

    private List<GameObject> enemyButtons = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();
    //private Dictionary<string, int> healthRemaining = new Dictionary<string, int>();

    private void Awake()
    {
        //Goes through all the enemies that will be spawned for the combat
        for (int i = 0; i < GameManager.instance.enemyAmount; i++)
        {
            //Instantiates and gives them a name (needs to be changed to have the different enemy types have numbers)
            GameObject NewEnemy = Instantiate(GameManager.instance.enemies[i], spawnPoints[i].position, Quaternion.identity) as GameObject;
            NewEnemy.name = i.ToString() + "_" + GameManager.instance.enemies[i].name;
            NewEnemy.GetComponent<Enemy>().name = NewEnemy.name;

            //Adds the enemy to a list of enemies for the fight
            Enemies.Add(NewEnemy);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        battleStates = PerformAction.WAIT;
        Heros.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
        heroInput = HEROGUI.ACTIVATE;

        //Used to create the panels for the characters
        for(int i  = 0; i < Heros.Count; i++)
        {
            //Creates the panel as an object and places it in the spacer to automize proper spacing
            GameObject panel = Object.Instantiate(Resources.Load("Hero Panel")) as GameObject;
            panel.transform.SetParent(heroSpacer, false);

            //Sets up a temp variable to help with the next few changes
            Friendly character = Heros[i].GetComponent<Friendly>();

            //Gets the battleManager the characters needs
            character.battleManager = gameObject.GetComponent<BattleManager>();

            //Sets the panel's name area to the character's name (Once I set it to get the player's name, this will do that too)
            Text panelName = panel.transform.Find("CharacterName").GetComponent<Text>();
            panelName.text =  character.name;

            //Gets the progress bar characters need
            character.progressBar = panel.transform.Find("ProgressBar").GetComponent<Image>();

            //Gets and fills the HP part of the panel
            character.hpDisplay = panel.transform.Find("HPText").GetComponent<Text>();
            character.hpDisplay.text = "HP: " + character.currHP.ToString() + " / " + character.maxHP.ToString();

            //Gets and fills the MP part of the panel
            character.mpDisplay = panel.transform.Find("MPText").GetComponent<Text>();
            character.mpDisplay.text = "MP: " + character.currMP.ToString() + " / " + character.maxMP.ToString();
        }

        //Sets all the panels needed for the player to inactive
        attackPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        magicPanel.SetActive(false);
        spellPanel.SetActive(false);

        GameManager.instance.healthRemaining.Clear();
        GameManager.instance.magicRemaining.Clear();


        //Creates the buttons needed for the combat.
        CreateButton();
    }

    // Update is called once per frame
    void Update()
    {
        // Looks through the different states of the battle to determine actions
        switch (battleStates)
        {
            //If a character is set to wait, they will perform
            case (PerformAction.WAIT):
                {
                    if(PerformList.Count > 0)
                    {
                        battleStates = PerformAction.TAKEACTION;
                    }
                    break;
                }
            //Depdning on the character type, it's either random or choosen by the playyer
            case (PerformAction.TAKEACTION):
                {
                    GameObject performer = GameObject.Find(PerformList[0].AttackerName);
                    if(PerformList[0].Type == "Enemy")
                    {
                        //While players are still alive, the enemy chooses one to attack
                        if (Heros.Count > 0)
                        {
                            Enemy enemy = performer.GetComponent<Enemy>();
                            for (int i = 0; i < Heros.Count; i++)
                            {
                                if (PerformList[0].Attacked == Heros[i])
                                {
                                    enemy.Hero = PerformList[0].Attacked;
                                    enemy.currentState = Enemy.TurnState.ACTION;
                                    break;
                                }
                            }
                        }
                    }
                    //If it's a player, gets the needed info, then sends the action. Proceeds to wait until ready to attack again.
                    if (PerformList[0].Type == "Hero")
                    {
                        Friendly friend = performer.GetComponent<Friendly>();
                        friend.EnemytoAttack = PerformList[0].Attacked;
                        friend.currentState = Friendly.TurnState.ACTION;
                    }
                    battleStates = PerformAction.WAIT;
                    break;
                }
            //CHecks for the win and lose conditions of the game (will pull up a screen in the next iteration)
            case (PerformAction.CHECKALIVE):
                {
                    if(Heros.Count < 1)
                    {
                        battleStates = PerformAction.LOSE;
                    }
                    else if (Enemies.Count < 1)
                    {
                        battleStates = PerformAction.WIN;
                    }
                    //Clears everything out to get ready for the next player to attack
                    else
                    {
                        clearAttackPanel();
                        heroInput = HEROGUI.ACTIVATE;
                    }
                    break;
                }
            //Temporarily boots player back to main screen upon game over. Sets the world over. Next iteration will have the player get a lose screen first
            case (PerformAction.LOSE):
                {
                    for (int i = 0; i < Heros.Count; i++)
                    {
                        SaveValues(Heros[i].name, Heros[i].GetComponent<Friendly>().currHP, Heros[i].GetComponent<Friendly>().currMP);
                    }
                    Debug.Log("You lost");
                    MenuManager.GoToMenu(MenuName.Main);
                    GameManager.instance.gameStates = GameManager.GameStates.WORLD_STATE;
                    GameManager.instance.enemies.Clear();
                    break;
                }
            //Temporarily boots player back the overworld. Next iteration will have player get a win screen with items recieved. 
            case (PerformAction.WIN):
                {
                    //Sets all the players to waiting before loading back in
                    for (int i = 0; i < Heros.Count; i++)
                    {
                        Heros[i].GetComponent<Friendly>().currentState = Friendly.TurnState.WAITING;
                        SaveValues(Heros[i].name, Heros[i].GetComponent<Friendly>().currHP, Heros[i].GetComponent<Friendly>().currMP);
                    }
                    GameManager.instance.LoadSceneAfterBattle();
                    GameManager.instance.gameStates = GameManager.GameStates.WORLD_STATE;
                    GameManager.instance.enemies.Clear();

                    foreach (GameObject enem in GameManager.instance.enemiestoSave)
                    {
                        if (enem != gameObject)
                        {
                            enem.SetActive(true);
                        }
                    }
                    GameManager.instance.enemiestoSave.Clear();
                    Destroy(GameManager.instance.enemytoDestory);
                    break;
                }
            //If the player succeeds in escaping, they do the same idea as the win screen right now.
            case (PerformAction.ESCAPE):
                {

                    for (int i = 0; i < Heros.Count; i++)
                    {
                        Heros[i].GetComponent<Friendly>().currentState = Friendly.TurnState.WAITING;
                        SaveValues(Heros[i].name, Heros[i].GetComponent<Friendly>().currHP, Heros[i].GetComponent<Friendly>().currMP);
                    }
                    GameManager.instance.LoadSceneAfterBattle();
                    GameManager.instance.gameStates = GameManager.GameStates.WORLD_STATE;
                    GameManager.instance.enemies.Clear();
                    break;
                }
        }

        switch(heroInput)
        {
            //When it's the player turn, turns on all needed items (selector, the options, etc.)
            case (HEROGUI.ACTIVATE):
            {
                if(HerosToManage.Count > 0)
                    {
                        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                        playerChoice = new HandleTurns();
                        attackPanel.SetActive(true);

                        CreateAttackButtons();
                        heroInput = HEROGUI.WAITING;
                    }
                break;
            }
            //Once the player has made thier choices, clears everything u
            case (HEROGUI.DONE):
            {
                    HeroInputDone();
                    break;
            }
            //A state just to hold the player while waiting to be charged
            case (HEROGUI.WAITING):
                {
                    break;
                }
        }
    }

    //Collects the inputs on menus to send for an attack.
    public void CollectActions(HandleTurns input)
    {
        PerformList.Add(input);
    }    

    //Clears out the array of enemy choices, then adds them back (in chase of changes)
    public void CreateButton()
    {
        //Clears out array of enemy choices
        foreach(GameObject enemyButton in enemyButtons)
        {
            Destroy(enemyButton);
        }

        enemyButtons.Clear();

        //Creates the buttons again for a player attack
        foreach (GameObject enemy in Enemies)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            Enemy curEnemy = enemy.GetComponent<Enemy>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            string[] splitArray = curEnemy.name.Split(char.Parse("_"));
            buttonText.text = splitArray[1];

            button.Enemy = enemy;

            enemyButtons.Add(newButton);

            newButton.transform.SetParent(spacer,false);
        }
    }

    //The choice to attack the enemies
    public void Attack()
    {
        playerChoice.AttackerName = HerosToManage[0].name;
        clearAttackPanel();
        playerChoice.Attacker = HerosToManage[0];
        playerChoice.Type = "Hero";
        playerChoice.choosenAttack = HerosToManage[0].GetComponent<Friendly>().attacks[0];
        attackType = playerChoice.choosenAttack.type;
        attackPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    //Establishes which enemy the player is attacking
    public void EnemytoAttack(GameObject choosenEnemy)
    {
        playerChoice.Attacked = choosenEnemy;
        HerosToManage[0].GetComponent<Friendly>().currMP -= playerChoice.choosenAttack.attackCost;
        HerosToManage[0].GetComponent<Friendly>().mpDisplay.text = "MP: " + HerosToManage[0].GetComponent<Friendly>().currMP.ToString() + "/" + HerosToManage[0].GetComponent<Friendly>().maxMP.ToString();
        heroInput = HEROGUI.DONE;
    }

    //Sets the player to attack, then clears all the informatoin for them attacking
    void HeroInputDone()
    {
        PerformList.Add(playerChoice);
        clearAttackPanel();
        
        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HerosToManage.RemoveAt(0);
        heroInput = HEROGUI.ACTIVATE;

    }

    //Removes all panels and buttons after an attack has been declared.
    void clearAttackPanel()
    {
        enemySelectPanel.SetActive(false);
        attackPanel.SetActive(false);
        magicPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    //Creates the buttons needed for an attack
    void CreateAttackButtons()
    {
        //Creates the attack button and gives it proper text
        GameObject attackButton = Instantiate(actionButton) as GameObject;
        Text attackButtonText = attackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        attackButtonText.text = "Attack";

        //Sets it's listener and parent
        attackButton.GetComponent<Button>().onClick.AddListener(() => Attack());
        attackButton.transform.SetParent(actionSpacer, false);

        //Adds it to the list of buttons made
        atkBtns.Add(attackButton);

        //Creates the magic button and gives it proper text
        GameObject magicButton = Instantiate(actionButton) as GameObject;
        Text magicButtonText = magicButton.transform.Find("Text").gameObject.GetComponent<Text>();
        magicButtonText.text = "Magic";

        //Sets it's listener and parent
        magicButton.GetComponent<Button>().onClick.AddListener(() => MagicAttack());
        magicButton.transform.SetParent(actionSpacer, false);

        //Adds it to the list of buttons made
        atkBtns.Add(magicButton);

        //Creates the block button and gives it proper text
        GameObject blockButton = Instantiate(actionButton) as GameObject;
        Text blockButtonText = blockButton.transform.Find("Text").gameObject.GetComponent<Text>();
        blockButtonText.text = "Block";

        //Sets it's listener and parent
        blockButton.GetComponent<Button>().onClick.AddListener(() => Block());
        blockButton.transform.SetParent(actionSpacer, false);

        //Adds it to the list of buttons made
        atkBtns.Add(blockButton);

        //Creates the escape button and gives it proper text
        GameObject escapeButton = Instantiate(actionButton) as GameObject;
        Text escapeButtonText = escapeButton.transform.Find("Text").gameObject.GetComponent<Text>();
        escapeButtonText.text = "Escape";

        //Sets it's listener and parent
        escapeButton.GetComponent<Button>().onClick.AddListener(() => Escape());
        escapeButton.transform.SetParent(actionSpacer, false);

        //Adds it to the list of buttons made
        atkBtns.Add(escapeButton);

        //If the character has any magic spells, creates a specific button for them
        if (HerosToManage[0].GetComponent<Friendly>().magicAttacks.Count > 0)
        {
            foreach (Attack magicAttack in HerosToManage[0].GetComponent<Friendly>().magicAttacks)
            {
                GameObject button = Instantiate(baseMagicButton) as GameObject;
                Text buttonText = button.transform.Find("Text").gameObject.GetComponent<Text>();
                buttonText.text = magicAttack.attackName;

                MagicAttackButton ATB = button.GetComponent<MagicAttackButton>();
                ATB.magicAttackToPerform = magicAttack;
                button.transform.SetParent(magicSpacer, false);
                
                if(HerosToManage[0].GetComponent<Friendly>().currMP < magicAttack.attackCost)
                {
                    button.GetComponent<Button>().interactable = false;
                }

                atkBtns.Add(button);
            }
        }
        //Otherwise, the magic button is disabled
        else
        {
            magicButton.GetComponent<Button>().interactable = false;
        }
    }

    //Sends information related to whcih magic attack the player used out
    public void MagictoUse(Attack choosenMagic)
    {
        playerChoice.AttackerName = HerosToManage[0].name;
        playerChoice.Attacker = HerosToManage[0];
        playerChoice.Type = "Hero";

        playerChoice.choosenAttack = choosenMagic;
        attackType = playerChoice.choosenAttack.type;
        magicPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }
    
    //Player declares a magic attack. Brings up the correct menus
    public void MagicAttack()
    {
        attackPanel.SetActive(false);
        magicPanel.SetActive(true);
    }
    
    //Player decides to block. Removes all the unneed items and sets them to block until their next go around.
    public void Block()
    {
        Friendly character = HerosToManage[0].GetComponent<Friendly>();
        character.isBlock = true;
        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        character.battleManager.battleStates = BattleManager.PerformAction.WAIT;
        character.curCharge /= 2;
        character.currentState = Friendly.TurnState.CHARGING;
        HerosToManage.Clear();
        clearAttackPanel();
        heroInput = HEROGUI.ACTIVATE;
    }

    //Compares the player team's health to the enemies to decide their chances to esapce (in the future will declare if they succeed or not).
    public void Escape()
    {
        int alliedHealth = 0;
        int enemyHealth = 0;

        for (int i = 0; i < Heros.Count; i++)
        {
            alliedHealth += Heros[i].GetComponent<Friendly>().maxHP;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            enemyHealth += Enemies[i].GetComponent<Enemy>().maxHP;
        }

        int escapeChance = (int)(alliedHealth + 60 / (enemyHealth / 2 + 30) * 100);

        if (escapeChance >= Random.Range(0, 100))
        {
            battleStates = PerformAction.WIN;
        }

        //Same stuff as the block, minus the block bonus.
        else
        {
            Friendly character = HerosToManage[0].GetComponent<Friendly>();
            HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
            character.battleManager.battleStates = BattleManager.PerformAction.WAIT;
            character.curCharge = 0f;
            character.currentState = Friendly.TurnState.CHARGING;
            HerosToManage.Clear();
            clearAttackPanel();
            heroInput = HEROGUI.ACTIVATE; 
        }
    }

    private void SaveValues(string character, int HP, int MP)
    {
        GameManager.instance.healthRemaining[character] = HP;
        GameManager.instance.magicRemaining[character] = MP;
    }
}

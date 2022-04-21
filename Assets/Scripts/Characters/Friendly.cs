using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Friendly : Character
{
    public BattleManager battleManager;
    public enum TurnState
    {
        CHARGING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    //All the needed items to work with a panel
    public Image progressBar;
    public Text hpDisplay;
    public Text mpDisplay;

    private static bool created = false;
    private static string objectName;

    public GameObject EnemytoAttack;
    public bool actionStarted = false;
    private Vector3 startPosition;

    private float animspeed = 20f;

    public GameObject Selector;
    public GameObject indicator;

    private bool Alive = true;

    public bool isBlock = false;

    public TurnState currentState;
    // Start is called before the first frame update
    new public void Start()
    {
        //Calls the start of it's parent
        base.Start();

        //Sets variables and componenets to how they need to be
        Selector.SetActive(false);
        currentState = TurnState.CHARGING;
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        startPosition = transform.position;
        
    }
    private void Awake()
    {
        foreach (KeyValuePair<string, int> character in GameManager.instance.healthRemaining)
        {
            if (character.Key == this.name)
            {
                currHP = character.Value;
                if (currHP > maxHP)
                {
                    currHP = maxHP;
                }
            }
        }

        foreach (KeyValuePair<string, int> character in GameManager.instance.magicRemaining)
        {
            if (character.Key == this.name)
            {
                currMP = character.Value;
                if (currMP > maxMP)
                {
                    currMP = maxMP;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Hanles all the different states of the player
        switch(currentState)
        {
            //Player is waiting for their meter to fill
            case TurnState.CHARGING:
                {
                    UpgradeProgressBar();
                    break;
                }
            //Once it's filled, they're added to a list for actions
            case TurnState.ADDTOLIST:
                {
                    battleManager.HerosToManage.Add(this.gameObject);
                    currentState = TurnState.WAITING;
                    isBlock = false;
                    break;
                }
            case TurnState.WAITING:
                {
                    break;
                }
            case TurnState.SELECTING:
                {
                    break;
                }
            //Player starts their choosen action
            case TurnState.ACTION:
                {
                    StartCoroutine(TimeForAction());
                    break;
                }
            //If they are dead, completes all the need proceedings
            case TurnState.DEAD:
                {
                    if(!Alive)
                    {
                        return;
                    }
                    else
                    {
                        //Sets them to a different tag
                        this.gameObject.tag = "Dead Friendly";

                        //Removes them from the active list
                        battleManager.Heros.Remove(this.gameObject);

                        //Turns off the selector if it's on
                        Selector.SetActive(false);

                        //Sets the panels to not active
                        battleManager.attackPanel.SetActive(false);
                        battleManager.enemySelectPanel.SetActive(false);

                        //If there are more friendly character left, helps the game continue running
                        if (battleManager.Heros.Count > 0)
                        {
                            for (int i = 0; i < battleManager.PerformList.Count; i++)
                            {
                                if (i != 0)
                                {
                                    if (battleManager.PerformList[i].Attacker == this.gameObject)
                                    {
                                        battleManager.PerformList.Remove(battleManager.PerformList[i]);
                                    }

                                    if (battleManager.PerformList[i].Attacked == this.gameObject)
                                    {
                                        battleManager.PerformList[i].Attacked = battleManager.Heros[Random.Range(0, battleManager.Heros.Count)];
                                    }
                                }
                            }
                        }
                        //Has the game check if the whole team is dead
                        battleManager.battleStates = BattleManager.PerformAction.CHECKALIVE;
                        
                        //Removes the character from who needs to be watched and is set to be dead
                        battleManager.HerosToManage.Remove(this.gameObject);
                        Alive = false;
                    }
                    break;
                }
        }
    }

    //Hanldes increasing the bar and updating the visual
    void UpgradeProgressBar()
    {
        curCharge += Time.deltaTime;
        float calcCharge = curCharge / maxCharge;
        progressBar.transform.localScale = new Vector3(Mathf.Clamp(calcCharge,0,1), progressBar.transform.localScale.y, progressBar.transform.localScale.z);

        //If they reach full charge, adds them to the list
        if(curCharge >= maxCharge)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }

    //Handles the character attacking
    private IEnumerator TimeForAction()
    {
        //Ensures function doesn't run more than once per time
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;
        //Locates the enemy they are attacking
        Vector3 enemyPosition = new Vector3(EnemytoAttack.transform.position.x - 2f, EnemytoAttack.transform.position.y, EnemytoAttack.transform.position.z);

        //Moves towards the enemy
        while (MoveTowardTarget(enemyPosition))
        {
            yield return null;
        }
        
        //Waits half a second, then attacks
        yield return new WaitForSeconds(.5f);

        doDamage();

        //Moves back to position
        while (MoveTowardTarget(startPosition))
        {
            yield return null;
        }

        //Removes them from the list to attack
        battleManager.PerformList.RemoveAt(0);

        //If the fight isn't over, go back to charging
        if(battleManager.battleStates != BattleManager.PerformAction.WIN && battleManager.battleStates != BattleManager.PerformAction.LOSE)
        {
            battleManager.battleStates = BattleManager.PerformAction.WAIT;
            actionStarted = false;
            curCharge = 0f;
            currentState = TurnState.CHARGING;
        }

        //Otherwise, wait for the game to continue
        else
        {
            currentState = TurnState.WAITING;
        }
        
    }

    //Moves the character towards their designated location
    private bool MoveTowardTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animspeed * Time.deltaTime));
    }

    //Hanldes the player taking damage from an enemy (will likely be adapated to be from any source).
    public void TakeDamage(int damageAmount)
    {
        currHP -= damageAmount;
        GameObject damage = Instantiate(indicator, gameObject.transform.position, Quaternion.identity) as GameObject;

        //Creates the number that floats when a character takes damage
        damage.GetComponentInChildren<TextMesh>().text = damageAmount.ToString();
        if (currHP <= 0)
        {
            currentState = TurnState.DEAD;
        }
        hpDisplay.text = "HP: " + currHP.ToString() + "/" + maxHP.ToString();
    }
    //Hanldes dealing damage to enemies
    void doDamage()
    {
        //This is a very inefficient system to handle the type chart I have for the game. Hopefully I'll find a better way between iterations.
        
        Attack attack = battleManager.PerformList[0].choosenAttack;
        Type heroType = EnemytoAttack.GetComponent<Enemy>().type;

        float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(attack.type, heroType);

        //Calculates the damage an enemy will take. If ti would be less than 1, makes sure they take at least 1 damage
        int calc_damage = (int)((this.currAttack + attack.attackDamage - EnemytoAttack.GetComponent<Enemy>().defense) * Random.Range(.8f,1.2f) * elemintalModifier);
        if (calc_damage < 1)
        {
            calc_damage = 1;
        }

        //Enemy takes given damage
        EnemytoAttack.GetComponent<Enemy>().TakeDamage(calc_damage);
    }
}

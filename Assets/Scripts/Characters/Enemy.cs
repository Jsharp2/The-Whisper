using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    //RNG Health for somewhat intersting fights
    public float minHealthVarience = .8f;
    public float maxHealthVarience = 1.2f;

    private BattleManager battleManager;

    //Character state
    public enum TurnState
    {
        CHARGING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    public GameObject Selector;

    private Vector3 startPosition;

    public GameObject healthBar;

    private bool actionStarted = false;
    public GameObject Hero;
    private float animspeed = 20f;

    public GameObject indicator;

    public float calcHealth;

    private bool alive = true;

    public bool isBlock = false;

    public float baseScale;
    // Start is called before the first frame update
    new public void Start()
    {
        //Sets HP with modifier before calling paren
        this.maxHP = (int)(Random.Range(minHealthVarience, maxHealthVarience) * this.maxHP);
        currHP = maxHP;
        base.Start();
        currentState = TurnState.CHARGING;
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        startPosition = transform.position;

        baseScale = healthBar.transform.localScale.x;
        //Calculates HP (if any enemy was to start at not full) and sets up the health bar accordingly
        calcHealth = currHP / maxHP;
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(calcHealth, 0, 1) * baseScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Similar idea to the player's states
        switch (currentState)
        {
            case TurnState.CHARGING:
                {
                    UpgradeProgressBar();
                    break;
                }
            case TurnState.CHOOSEACTION:
                {
                    ChooseAction();
                    currentState = TurnState.WAITING;
                    break;
                }
            case TurnState.ACTION:
                {
                    StartCoroutine(TimeForAction());
                    break;
                }
            case TurnState.DEAD:
                {
                    if (!alive)
                    {
                        return;
                    }
                    else
                    {
                        Debug.Log("Enemy Dead");
                        //Hanles the same way the player does when dying
                        this.gameObject.tag = "Dead Enemy";

                        battleManager.Enemies.Remove(this.gameObject);
                        if (battleManager.Enemies.Count > 0)
                        {
                            for (int i = 0; i < battleManager.PerformList.Count; i++)
                            {
                                if (i != 0)
                                {
                                    if (battleManager.PerformList[i].Attacker == this.gameObject)
                                    {
                                        battleManager.PerformList.Remove(battleManager.PerformList[i]);
                                        Debug.Log("Removed from Perform List");
                                    }

                                    if (battleManager.PerformList[i].Attacked == this.gameObject)
                                    {
                                        Debug.Log("Attack Redirected");
                                        battleManager.PerformList[i].Attacked = battleManager.Enemies[Random.Range(0, battleManager.Enemies.Count)];
                                    }
                                }
                            }
                        }
                        Destroy(gameObject);

                        alive = false;

                        battleManager.CreateButton();

                        battleManager.battleStates = BattleManager.PerformAction.CHECKALIVE;

                    }
                    break;
                }
        }
    }
    //Upgrades the health bar they aren't full
    void UpgradeProgressBar()
    {
        curCharge += Time.deltaTime;
        if (curCharge >= maxCharge)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }

    //Randomly chooses a player to target, as well as an attack to use for their attacks
    void ChooseAction()
    {
        if(battleManager.Heros.Count > 0)
        {
            HandleTurns myAttack = new HandleTurns();

            myAttack.AttackerName = gameObject.name;
            myAttack.Attacker = this.gameObject;
            myAttack.Attacked = battleManager.Heros[Random.Range(0, battleManager.Heros.Count)];
            myAttack.Type = "Enemy";

            int attack = Random.Range(0, this.attacks.Count + this.magicAttacks.Count);

            if(attack < this.attacks.Count)
            {
                Debug.Log("Physical");
                myAttack.choosenAttack = this.attacks[attack];
            }
            else
            {
                attack = Random.Range(0, this.magicAttacks.Count);
                myAttack.choosenAttack = this.magicAttacks[attack];
            }
            
            battleManager.CollectActions(myAttack);
        }
    }

    //Attacks simailar to the player
    private IEnumerator TimeForAction()
    {
        if(actionStarted)
        {
            yield break;
        }

        actionStarted = true;
        if(battleManager.PerformList[0].choosenAttack.target == Attack.Target.Single)
        {
            gameObject.transform.Find("Dirt Patch").gameObject.SetActive(false);
            Vector3 heroPosition = new Vector3(Hero.transform.position.x + 2f, Hero.transform.position.y, Hero.transform.position.z);
            while (MoveTowardTarget(heroPosition))
            {
                yield return null;
            }

            yield return new WaitForSeconds(.5f);

            DoDamage();

            while (MoveTowardTarget(startPosition))
            {
                yield return null;
            }
            gameObject.transform.Find("Dirt Patch").gameObject.SetActive(true);
        }
        else
        {
            //Waits half a second, then attacks
            yield return new WaitForSeconds(.5f);
            DoDamage();
        }

        battleManager.PerformList.RemoveAt(0);

        battleManager.battleStates = BattleManager.PerformAction.WAIT;
        actionStarted = false;
        curCharge = 0f;
        currentState = TurnState.CHARGING;
    }

    //Moves towards the player the same as the player does to enemies
    private bool MoveTowardTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animspeed * Time.deltaTime));
    }

    //How enemies handle how much damage they do
    void DoDamage()
    {
        Attack attack = battleManager.PerformList[0].choosenAttack;
        if (attack.style == Attack.Style.Phyiscal)
        {
            if (attack.target == Attack.Target.Multiple)
            {
                foreach (GameObject ally in battleManager.Heros)
                {
                    Type heroType = ally.GetComponent<Friendly>().type;
                    float blockModifier = 0;
                    if (ally.GetComponent<Friendly>().isBlock)
                    {
                        blockModifier = .5f;
                    }

                    float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(attack.type, heroType);

                    int calcDamage = (int)((this.currAttack + attack.attackDamage - ally.GetComponent<Friendly>().currDefense) * Random.Range(.8f, 1.2f) * (1 - blockModifier) * elemintalModifier * .8f);

                    if (calcDamage < 1)
                    {
                        calcDamage = 1;
                    }
                    ally.GetComponent<Friendly>().TakeDamage(calcDamage);
                }
            }
            else
            {
                Type heroType = Hero.GetComponent<Friendly>().type;
                float blockModifier = 0;
                if (Hero.GetComponent<Friendly>().isBlock)
                {
                    blockModifier = .5f;
                }

                float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(attack.type, heroType);

                int calcDamage = (int)((this.currAttack + attack.attackDamage - Hero.GetComponent<Friendly>().currDefense) * Random.Range(.8f, 1.2f) * (1 - blockModifier) * elemintalModifier);

                if (calcDamage < 1)
                {
                    calcDamage = 1;
                }
                Hero.GetComponent<Friendly>().TakeDamage(calcDamage);
            }
        }
        
        else
        {
            if (attack.target == Attack.Target.Multiple)
            {
                foreach (GameObject ally in battleManager.Heros)
                {
                    Type heroType = ally.GetComponent<Friendly>().type;

                    float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(attack.type, heroType);

                    int calcDamage = (int)((this.currMagic + attack.attackDamage - ally.GetComponent<Friendly>().currMagic) * Random.Range(.8f, 1.2f) * elemintalModifier * .8f);

                    if (calcDamage < 1)
                    {
                        calcDamage = 1;
                    }
                    ally.GetComponent<Friendly>().TakeDamage(calcDamage);
                }
            }
            else
            {
                Type heroType = Hero.GetComponent<Friendly>().type;

                float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(attack.type, heroType);

                int calcDamage = (int)((this.currMagic + attack.attackDamage - Hero.GetComponent<Friendly>().currMagic) * Random.Range(.8f, 1.2f) * elemintalModifier);

                if (calcDamage < 1)
                {
                    calcDamage = 1;
                }
                Hero.GetComponent<Friendly>().TakeDamage(calcDamage);
            }
        }
    }

    //Similar way of hanlding helath
    public void TakeDamage(int damageAmount)
    {
        currHP -= damageAmount;
        
        //The percent health they have has be calculated this way as they're both ints
        calcHealth = ((float)currHP / maxHP);
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(calcHealth, 0, 1) * baseScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        //Creates the number that floats when a character takes damage
        GameObject damage = Instantiate(indicator, gameObject.transform.position, Quaternion.identity) as GameObject;
        damage.GetComponentInChildren<TextMesh>().text = damageAmount.ToString();
        if (currHP <= 0)
        {
            currHP = 0;
            currentState = TurnState.DEAD;
        }

    }
}

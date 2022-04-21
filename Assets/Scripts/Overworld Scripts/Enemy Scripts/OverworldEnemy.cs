using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemy : MonoBehaviour
{
    public enum Movement
    {
        Roaming,
        Horizontal,
        Vertical
    }

    public enum Direction
    {
        North,
        South,
        East,
        West,
        Wait
    }
    public SpawnRegion home;
    //Access to movment Enum
    public Movement movement;

    //Access to direction Enum
    public Direction direction;

    //Used to handle horizontal or vertical npcs
    private Direction previousDirection;

    //How fast NPC moves
    public float moveSpeed = 2f;

    //Handles if they can move around
    bool canMove = true;

    //Handles how long an NPC will stand still until they move again
    public float minWait = 2f;
    public float maxWait = 4f;
    private float wait;

    //Handles the amonut of time they've waited
    private float time = 0f;

    //Where they want to rotate to
    private float goal;

    //Their starting location so they don't stray too far
    private Vector3 startingLocation;

    //How far they can go from the start
    public float maxDistanceFromStart = 3f;

    public float distance = 15f;

    //A value for their random distance allowed
    float allowedRoamDistance;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the starting location of the NPC
        startingLocation = transform.position;

        //Gets a random amount of time to wait
        wait = Random.Range(minWait, maxWait);

        //Gets a random distance to move
        allowedRoamDistance = Random.Range(0, maxDistanceFromStart);
        direction = (Direction)System.Enum.ToObject(typeof(Direction), Random.Range(0, 3));
    }


    // Update is called once per frame
    void Update()
    {
        //Save the position of the NPC
        Vector3 position = transform.position;
       #region Stop Moving
        //If it's the correct distance way, stops moving, and waits for a bit.
        if (direction == Direction.West && startingLocation.x - transform.position.x >= allowedRoamDistance)
        {
            previousDirection = direction;
            direction = Direction.Wait;
            canMove = false;
        }
        //If it's the correct distance way, stops moving, and waits for a bit.
        else if (direction == Direction.East && transform.position.x - startingLocation.x >= allowedRoamDistance)
        {
            previousDirection = direction;
            direction = Direction.Wait;
            canMove = false;
        }
        //If it's the correct distance way, stops moving, and waits for a bit.
        else if (direction == Direction.South && startingLocation.y - transform.position.y >= allowedRoamDistance)
        {
            previousDirection = direction;
            direction = Direction.Wait;
            canMove = false;
        }
        //If it's the correct distance way, stops moving, and waits for a bit.
        else if (direction == Direction.North && transform.position.y - startingLocation.y >= allowedRoamDistance)
        {
            previousDirection = direction;
            direction = Direction.Wait;
            canMove = false;
        }
        #endregion
        #region Movement
        //If the NPC is allowed to move, moves depending on where it's facing
        if (canMove)
        {
            if (direction == Direction.West)
            {
                position.x -= moveSpeed * Time.deltaTime;
            }
            else if (direction == Direction.North)
            {
                position.y += moveSpeed * Time.deltaTime;
            }
            else if (direction == Direction.East)
            {
                position.x += moveSpeed * Time.deltaTime;
            }
            else if (direction == Direction.South)
            {
                position.y -= moveSpeed * Time.deltaTime;
            }
            transform.position = position;
        }
        #endregion
        #region Wait
        //If it's waiting tracks how long it's waited
        if (direction == Direction.Wait)
        {
            time += Time.deltaTime;
            //If it's waited long enough, gets a new distance to go and time to wait. 
            if (time >= wait)
            {
                allowedRoamDistance = Random.Range(0, maxDistanceFromStart);
                wait = Random.Range(minWait, maxWait);
                time = 0f;
                //If it isn't a roaming NPC, goes the opposite direction is was before
                if (movement != Movement.Roaming)
                {
                    if (previousDirection == Direction.West)
                    {
                        direction = Direction.East;
                    }
                    else if (previousDirection == Direction.North)
                    {
                        direction = Direction.South;
                    }
                    else if (previousDirection == Direction.East)
                    {
                        direction = Direction.West;
                    }
                    else if (previousDirection == Direction.South)
                    {
                        direction = Direction.North;
                    }
                }
                //If it was a romaing NPC, gets a new random direction to go
                else
                {
                    previousDirection = direction;
                    direction = (Direction)System.Enum.ToObject(typeof(Direction), Random.Range(0, 4));
                }
            }
            canMove = true;
        }
        #endregion
        float xDiff = Mathf.Pow((transform.position.x - GameObject.Find("Player").transform.position.x), 2);
        float yDiff = Mathf.Pow((transform.position.y - GameObject.Find("Player").transform.position.y), 2);
        if (Mathf.Sqrt(xDiff + yDiff) > 15f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Handles if the Enemy runs into the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.instance.enemiestoSave.Clear();
            GameManager.instance.curRegions = gameObject.GetComponent<RegionData>();
            GameManager.instance.enemytoDestory = gameObject;
            GameManager.instance.StartBattle();
        }
    }
}

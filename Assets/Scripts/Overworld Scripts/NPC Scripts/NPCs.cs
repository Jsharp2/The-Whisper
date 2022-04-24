using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    public enum Movement
    {
        Roaming,
        Horizontal,
        Vertical,
        Healer
    }

    public enum Direction
    { 
        North,
        South,
        East,
        West,
        Wait,
    }

    //Access to movment Enum
    public Movement movement;

    //Access to direction Enum
    public Direction direction;

    //Used to handle horizontal or vertical npcs
    private Direction previousDirection;

    //How fast NPC moves
    public float moveSpeed = 2f;

    //How fast they rotate
    public float orientationSpeed = 180f;

    //How far they can be off before snapping to correct direction
    public float offset = 5f;

    //Handles if they can move around
    bool canMove = false;

    //Handles how long an NPC will stand still until they move again
    public float minWait = 2f;
    public float maxWait = 4f;
    private float wait;

    //Handles the amonut of time they've waited
    private float time = 0f;

    //The angle they're going to rotate
    private string rotationAngle = null;

    //Where they want to rotate to
    private float goal;

    //Their starting location so they don't stray too far
    private Vector3 startingLocation;

    //How far they can go from the start
    public float maxDistanceFromStart = 3f;

    //A value for their random distance allowed
    float allowedRoamDistance;

    List<string> characters = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //Gets the starting location of the NPC
        startingLocation = transform.position;

        //Gets a random amount of time to wait
        wait = Random.Range(minWait, maxWait);
        
        //Gets a random distance to move
        allowedRoamDistance = Random.Range(0, maxDistanceFromStart);
    }

    // Update is called once per frame
    void Update()
    {
        //Save the position of the NPC
        Vector3 position = transform.position;
        #region Orientation
        //Checks which way the NPC goes
        if(direction == Direction.West)
        {
            //If the NPC can't move, checks rotation code
            if(!canMove)
            {
                //If the NPC is in within the range to stop rotating, orientates them correctly, allows them to move, the stop the rotation
                if (transform.rotation.eulerAngles.z <= 180 + offset && transform.rotation.eulerAngles.z >= 180 - offset)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    canMove = true;
                    rotationAngle = null;
                }
                //Otherwise, it sees if it has the correct way to rotate
                else
                {
                    if (rotationAngle == null)
                    {
                        //Ensures that it takes the fastest way to rotate.
                        if (transform.rotation.eulerAngles.z <= 180)
                        {
                            rotationAngle = "Counterclockwise";
                        }
                        else
                        {
                            rotationAngle = "Clockwise";
                        }
                    }
                }
            }
        }
        //Same stuff as above, but for East
        else if (direction == Direction.East)
        {
            if(!canMove)
            {
                if (transform.rotation.eulerAngles.z <= 0 + offset && transform.rotation.eulerAngles.z >= 0 - offset)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    canMove = true;
                    rotationAngle = null;
                }
                else
                {
                    if (rotationAngle == null)
                    {
                        if (transform.rotation.eulerAngles.z > 180)
                        {
                            rotationAngle = "Counterclockwise";
                        }
                        else
                        {
                            rotationAngle = "Clockwise";
                        }
                    }
                }
            }
        }
        //Same stuff as above, but for North
        else if (direction == Direction.North)
        {
            if(!canMove)
            {
                if (transform.rotation.eulerAngles.z <= 90 + offset && transform.rotation.eulerAngles.z >= 90 - offset)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    canMove = true;
                    rotationAngle = null;
                }
                else
                {
                    if (rotationAngle == null)
                    {
                        if (transform.rotation.eulerAngles.z <= 270 && transform.rotation.eulerAngles.z >= 90)
                        {
                            rotationAngle = "Clockwise";
                        }
                        else
                        {
                            rotationAngle = "Counterclockwise";
                        }
                    }
                }
            }
        }
        //Same stuff as above, but for South
        else if (direction == Direction.South)
        {
            if (!canMove)
            {
                if (transform.rotation.eulerAngles.z <= 270 + offset && transform.rotation.eulerAngles.z >= 270 - offset)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                    canMove = true;
                    rotationAngle = null;
                }
                else
                {
                    if (rotationAngle == null)
                    {
                        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
                        {
                            rotationAngle = "Counterclockwise";
                        }
                        else
                        {
                            rotationAngle = "Clockwise";
                        }
                    }
                    canMove = false;
                }
            }
        }
        //If it's supposed to be waiting, stops rotating (fixes a bug)
        else if(direction == Direction.Wait)
        {
            rotationAngle = null;
        }

        //If the character's fastest way is clock wise, go that way. Otherwise, goes counterclockwise
        if(rotationAngle == "Clockwise")
        {
            transform.Rotate(0, 0, -orientationSpeed * Time.deltaTime);
        }
        else if(rotationAngle == "Counterclockwise")
        {
            transform.Rotate(0, 0, orientationSpeed * Time.deltaTime);
        }

        #endregion
        #region Stop Moving
        //If it's the correct distance way, stops moving, and waits for a bit.
        if (direction == Direction.West && startingLocation.x - transform.position.x >= allowedRoamDistance)
        {
            previousDirection = direction;
            direction = Direction.Wait;
            canMove = false;
        }
        //If it's the correct distance way, stops moving, and waits for a bit.
        else if (direction == Direction.East && transform.position.x -startingLocation.x >= allowedRoamDistance)
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
        if (direction == Direction.Wait && movement != Movement.Healer)
        {
            time += Time.deltaTime;
            //If it's waited long enough, gets a new distance to go and time to wait. 
            if(time >= wait)
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
                    direction = (Direction)System.Enum.ToObject(typeof(Direction),Random.Range(0,3));
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Handles if the NPC runs into anything with a box collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the NPC is able to move (allows it to not stop while rotating)
        if(canMove)
        {
            //Stops moving and waits.
            direction = Direction.Wait;
            canMove = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.transform.Find("Interact").transform.gameObject.SetActive(true);
        }    
    }

    public void healTeam()
    {
        if (movement == Movement.Healer)
        {
            //if(GameManager.instance.gold >= Cost)
            //{
            foreach (KeyValuePair<string, int> character in GameManager.instance.healthRemaining)
            {
                characters.Add(character.Key);
            }
            GameManager.instance.healthRemaining.Clear();
            GameManager.instance.magicRemaining.Clear();
            foreach (string character in characters)
            {
                GameManager.instance.healthRemaining[character] = 9999;
                GameManager.instance.magicRemaining[character] = 9999;
            }
        }
    }
}

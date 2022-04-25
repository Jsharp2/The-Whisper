using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	//Sets the movespeed for the character
    float movespeed = 3.5f;

	//Tracks positions when needed
	Vector3 curPosition, lastPosition;

	[SerializeField] GameObject interact;

	private bool facingLeft = false;

    private void Start()
    {
		//See if the spawn point has been given. If not, sets it
		if(GameManager.instance.nextSpawnPoint != "")
        {
			GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint);
			transform.position = spawnPoint.transform.position;

			GameManager.instance.nextSpawnPoint = "";
        }
		//If the spawn is at (0,0,0), set the spawn point there
		else if(GameManager.instance.lastPostion != Vector3.zero)
        {
			transform.position = GameManager.instance.lastPostion;
			GameManager.instance.lastPostion = Vector3.zero;
        }
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		//Gets the positio of the player
		Vector3 position = transform.position;

		// get new horizontal position
		float horizontalInput = Input.GetAxis("Horizontal");
		if (horizontalInput < 0)
		{
			position.x += horizontalInput * movespeed *
				Time.deltaTime;
			facingLeft = true;
		}
		else if (horizontalInput > 0)
		{
			position.x += horizontalInput * movespeed *
				Time.deltaTime;
			facingLeft = false;
		}

		// get new vertical position
		float verticalInput = Input.GetAxis("Vertical");
		if (verticalInput < 0 ||
			verticalInput > 0)
		{
			position.y += verticalInput * movespeed *
				Time.deltaTime;
		}

		if(facingLeft && transform.localScale.x != -1)
        {
			transform.localScale = new Vector3(-1,1,1);
			Debug.Log(transform.Find("Interact"));
			transform.Find("Interact").transform.localScale = new Vector3(-1, 1, 1);

		}
		else if(!facingLeft && transform.localScale.x != 1)
        {
			transform.localScale = new Vector3(1, 1, 1);
			transform.Find("Interact").transform.localScale = new Vector3(1, 1, 1);
		}

		// move and clamp in screen
		transform.position = position;
		curPosition = transform.position;

		lastPosition = curPosition;
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
		CollisionHandler handler = collision.gameObject.transform.gameObject.GetComponent<CollisionHandler>();
		NPCs NPC = collision.gameObject.transform.gameObject.GetComponent<NPCs>();

		if (handler != null)
        {
			interact.SetActive(true);
			if (Input.GetAxis("Interact") != 0)
			{
				GameManager.instance.nextSpawnPoint = handler.spawnPointName;
				GameManager.instance.scenetoLoad = handler.sceneToLoad;
				GameManager.instance.LoadScene();
				GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Spawned Enemies");
				GameManager.instance.DeleteEnemies();
			}
		}
		else if(NPC != null)
        {
			//interact.SetActive(true);
			if (NPC.movement == NPCs.Movement.Healer)
            {
				if(Input.GetAxis("Interact") != 0)
                {
					NPC.healTeam();
                }
            }
		}
		else
		{
			interact.SetActive(false);
		}

	}

    private void OnCollisionExit2D(Collision2D collision)
    {
		interact.SetActive(false);
	}

}

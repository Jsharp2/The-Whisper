using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float maxDistance = 3f;

    public float speed = 1f;
    Vector3 startingPosition;
    bool isMovingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Vector3 position = transform.position;
    //    if (position.y <= startingPosition.y + maxDistance)
    //    {
    //        isMovingUp = false;
    //    }
    //    else if(position.y >= startingPosition.y - maxDistance)
    //    {
    //        isMovingUp = true;
    //    }
        
    //    if(isMovingUp)
    //    {
    //        position.y += speed * Time.deltaTime;
    //    }
    //    else
    //    {
    //        position.y -= speed * Time.deltaTime;
    //    }
        
    //    Debug.Log(isMovingUp);
    //    transform.position = position;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.hasKey = true;
            Destroy(gameObject);
        }
    }
}

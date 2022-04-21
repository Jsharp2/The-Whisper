using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    //Gets the object that needs to be followed
    [SerializeField]
    GameObject character;

    // Update is called once per frame
    void Update()
    {
        //Follows the character around the world
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y, this.transform.position.z);
    }
}

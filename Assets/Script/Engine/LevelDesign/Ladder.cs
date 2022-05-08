using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ladder : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CharacterControl player = other.GetComponent<CharacterControl>();
            float y = Input.GetAxis("Horizontal") * Time.deltaTime * player.GetSpeed * player.transform.localScale.x;
            player.transform.Translate(0, y, 0);
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");
        if (other.tag == "Player")
        {
            CharacterControl player = other.GetComponent<CharacterControl>();
            player.GetComponent<Rigidbody2D>().gravityScale = 1;

        }
    }
}

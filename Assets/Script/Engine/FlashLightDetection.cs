using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightDetection : MonoBehaviour
{
    [SerializeField]
    CharacterControl character;

    [SerializeField,Range(0,20)]
    float flashlightDetectionRadius = 2;  




    private void Awake()
    {
        this.GetComponent<CircleCollider2D>().radius = flashlightDetectionRadius * 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HighLight")
        {
            //ADD TO HIGHLIGHT CHARACTER LIST
            character.AddHighLightedObject(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HighLight")
        {
            //REMOVE FROM HIGHLIGHT CHARACTER LIST
            character.RemoveHighLightedObject(collision.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, flashlightDetectionRadius);
    }

}

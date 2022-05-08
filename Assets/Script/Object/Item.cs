using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "LevelNav/Item")]
public class Item : ScriptableObject
{
   [SerializeField]
   private string itemName;
    public Sprite itemSprite;

   //GETTER
    public string GetItemName{get=>itemName;}
     
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelNav/Level")]
public class Level : ScriptableObject
{
   [SerializeField]
   private int levelNumber=0;

    [SerializeField]
    private Item itemToFind;

    public string levelName;

    //GETTER
    public int GetLevelNumber{get=>levelNumber;}
    public Item GetItemToFind{get=>itemToFind;}
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Kovnir.FastTweener;
using TMPro;
public class MapNav : MonoBehaviour
{
    [Serializable]
    class LevelData{
        public Level lvl;
        public Transform mapPoint;
    }
    [SerializeField]
    GameObject player;
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    List<LevelData> levels;

    [SerializeField,Header("Movement Settings")]
    float moveSpeed = 1,cameraSpeed=1;

    bool hasBeenTriggered = false;

    [SerializeField]
    TextMeshProUGUI txt;
    public List<Transform> getAllPoints(){
        List<Transform> points = new List<Transform>();
        points.Add(startPoint);
        foreach(LevelData ld in levels){
            points.Add(ld.mapPoint);
        }
        return points;


    }


    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = getCurrentPos();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !hasBeenTriggered){
            StartCoroutine(MovePlayerChangeLevel());

        
        }
    }

    IEnumerator MovePlayerChangeLevel(){
        hasBeenTriggered = true;
        yield return new WaitForSeconds(2f);
        txt.text = levels[Progression.currentLvl - 2].lvl.levelName;
        //FADE TEXT ALPHA 
        FastTweener.Float(0,1,1,(value) => {
            txt.alpha = value;
        });
        

        //MOVE THE PLAYER
        Vector3 startPos = getCurrentPos();
        Vector3 endPos = levels[Progression.currentLvl-2].mapPoint.position;
        endPos.y = startPos.y;
        float t = 0;
        while(t<1){
            t += Time.deltaTime * moveSpeed;
            player.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        //FADE VOLUME
        AudioController.instance.TargetVolume(0,cameraSpeed,Ease.OutExpo);
        AudioController.instance.PlaySoundEffect("Zoom");
    
        //ZOOM IN 
        float zoom = 0;
        Camera.main.GetComponent<Animator>().enabled = false;
        Vector3 startPosCam = Camera.main.transform.position;
        while(zoom<1){
            zoom += Time.deltaTime * cameraSpeed;
            Camera.main.transform.position = Vector3.Lerp(startPosCam, endPos, zoom);
            yield return null;
        }
        player.GetComponent<CharacterMenu>().KillProcess();
        //LOAD LEVEL
        SceneManager.LoadScene(Progression.currentLvl);


    }


    Vector3 getCurrentPos(){
        if(Progression.currentLvl - 3 >= 0){
            Vector3 pos = levels[Progression.currentLvl-3].mapPoint.position;
            pos.y = player.transform.position.y;
            return pos;
        }else return player.transform.position;
    }
}

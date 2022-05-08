using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterControl : MonoBehaviour
{

    [SerializeField]
    float speed = 5.0f, jumpForce = 10.0f, flashlightIntensity = 10,flashSpeed=3;

    [SerializeField]
    Light2D flash;

    [SerializeField]
    Color highLightColor = Color.yellow;

    private List<GameObject> highLightedObjects = new List<GameObject>();
    
    Transform tr; 
    Rigidbody2D rb;
    [SerializeField]
    Animator animator;

    SpriteAnimator[] spritesAnim;

    [SerializeField]
    bool extendedMoving = false;

    //getter
    public float GetSpeed{get=>speed;}

    void Start()
    {
        tr = this.transform;
        rb = this.GetComponent<Rigidbody2D>();
        flash.intensity = 0;
        spritesAnim = GetComponentsInChildren<SpriteAnimator>();
        StartCoroutine(Winking());
    }

    float flashTimer = 0, flashDelay = 1.5f;
    public bool isFlashing = false;
    // Update is called once per frame
    void Update()
    {

        //MOVEMENT
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed * transform.localScale.x;
        float y = 0;
        if(extendedMoving){
            y = Input.GetAxis("Vertical") * Time.deltaTime * speed * transform.localScale.y;
        }

        //RUNNING ANIMATION
        if(x != 0 || y != 0){
            animator.SetBool("Running",true);

            //AUDIO EFFECT IF NOT PLAYING
            if(!AudioController.instance.soundEffectSource.isPlaying){
                AudioController.instance.PlaySoundEffect("Run",0.03f);
            }

            //MIRROR
            if(!extendedMoving){
            Vector3 scale = transform.localScale;
            scale.x *= (x/Mathf.Abs(x)) * -1 ;
            transform.localScale = scale;
            }
            

        }else{
            animator.SetBool("Running",false);

            if(AudioController.instance.soundEffectSource.isPlaying && AudioController.instance.currentSoundEffect == "Run"){
                AudioController.instance.soundEffectSource.Stop();
            }
        }
        

        


        if(transform.localScale.x < 0){
            x *= -1;
        }

        tr.Translate(x, y, 0);

        //JUMP AND CHECK GROUNDED
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            AudioController.instance.PlaySoundEffect("Jump",0.05f);
            animator.SetTrigger("Jumping");
        }

        //FLASH LIGHT F Button
        if (Input.GetKeyDown(KeyCode.F) && canFlash())
        {
            flashTimer = 0;
            AudioController.instance.PlaySoundEffect("Flash",0.04f);
            StartCoroutine(FlashLight());
        }

        flashTimer += Time.deltaTime;




    }
    public bool canFlash(){
        if(flashTimer < flashDelay){
            return false;
        }else{
            return true;
        }
    }

    bool canJump = true;


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" ||col.gameObject.tag == "HighLight")
        {
            canJump = true;
        }
        if(col.gameObject.tag == "Golf"){
            AudioController.instance.PlaySoundEffect("GolfTap",0.05f);
        }
    }

    IEnumerator FlashLight()
    {
        //SAVE ORIGINAL COLOR
        List<Color> originalColors = new List<Color>();

        List<GameObject> objects = new List<GameObject>(highLightedObjects);

        foreach (GameObject obj in highLightedObjects)
        {
            if(obj.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            {
                originalColors.Add(sr.color);
            }
        }

        while (flash.intensity < flashlightIntensity)
        {
            //FLASH THE LIGHT
            flash.intensity += flashSpeed * Time.deltaTime;
            yield return null;

            //HI-LIGHT THE OBJECTS
            for(int i = 0; i < objects.Count; i++)
            {
                if(objects[i].TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                {
                    sr.color = Color.Lerp(originalColors[i],highLightColor,flash.intensity/flashlightIntensity);
                }
            }
            
        }
        isFlashing = true;
        while(flash.intensity > 0)
        {
            //UNFLASH THE LIGHT
            flash.intensity -= flashSpeed * Time.deltaTime;
            yield return null;

            //UNHIGHLIGHT THE OBJECTS
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                {
                    sr.color = Color.Lerp(highLightColor,originalColors[i],1 - flash.intensity / flashlightIntensity);
                }
            }
        }

        isFlashing = false;

    }
    IEnumerator Winking(){
        while(true){
            yield return new WaitForSeconds(0.5f);
            //1/20 chance to wink
             if(Random.Range(0,20) == 0){
                 foreach(SpriteAnimator anim in spritesAnim){
                        anim.Engine.PlaySpriteAnim();
                    }
                
             }

        }
    }

    //PUBLIC
    public void AddHighLightedObject(GameObject obj)
    {
        highLightedObjects.Add(obj);
    }
    public void RemoveHighLightedObject(GameObject obj)
    {
        highLightedObjects.Remove(obj);
    }
    public void Jump(){
        rb.AddForce(Vector2.up * jumpForce);
    }

 
}

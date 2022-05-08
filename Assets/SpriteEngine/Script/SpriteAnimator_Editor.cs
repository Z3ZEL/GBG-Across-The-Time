using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;



[CustomEditor(typeof(SpriteAnimator))]
public class SpriteAnimator_Editor : Editor
{


    Sprite currentSprite = null;
    float currentTimeGapPerCent=0;
    int currentSpriteIndex = 0;
    int customTimeIndex = -1;

    float totalTimeGap = 99;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        SpriteAnimator animator = (SpriteAnimator)target;
        if (customTimeIndex >= 0)
        {
            CustomTime currentCustomTime = animator.CustomTimes[customTimeIndex];
        }


        GUILayout.Space(30);
        EditorGUILayout.LabelField("Custom Sprite Timing", EditorStyles.boldLabel);
        GUILayout.Space(10);

  
        


            if (animator.Sprites == null) return;
            else if (animator.Sprites.Length == 0) return;


      


            //IF THERE IS NO CUSTOM TIME YET
            if (animator.CustomTimes.Count <= 0)
            {
                if (Event.current.type == EventType.DragPerform) return;
                if (GUILayout.Button("Create new custom sprite timing"))
                {
                    CustomTime customTime = new CustomTime(animator.Duration, animator.Sprites.Length, 0);
                    animator.CustomTimes.Add(customTime);
                    currentSprite = animator.Sprites[customTime.spriteIndex];
                    currentTimeGapPerCent = customTime.timeGapPerCent;
                    currentSpriteIndex = customTime.spriteIndex;
                    customTimeIndex = 0;
                    
                }

            }
            else if (animator.CustomTimes.Count >= 1 && customTimeIndex < 0)
            {
                currentSprite = animator.Sprites[animator.CustomTimes[animator.CustomTimes.Count - 1].spriteIndex];
                currentSpriteIndex = animator.CustomTimes[animator.CustomTimes.Count - 1].spriteIndex;
                currentTimeGapPerCent = animator.CustomTimes[animator.CustomTimes.Count - 1].timeGapPerCent;
                customTimeIndex = animator.CustomTimes.Count - 1;

                CalculatePorcent(animator);
            }


            //WHEN THERE IS AT LEAST ONE CUSTOM TIME
            if (customTimeIndex >= 0)
            {


                //REFRESH THE DISPLAYED SPRITE
                EditorGUILayout.BeginHorizontal();
                currentSprite = animator.Sprites[currentSpriteIndex];
                EditorGUILayout.ObjectField(currentSprite, typeof(Sprite), false, GUILayout.Height(64), GUILayout.Width(64));
                if (animator.CustomTimes.Count >= 1)
                {
                    SerializedProperty property = serializedObject.FindProperty("customTimes");
                    if (property.arraySize >= 1)
                    {
                        SerializedProperty arrayFind = property.GetArrayElementAtIndex(customTimeIndex);
                        property = arrayFind.FindPropertyRelative("Event");
                        EditorGUILayout.PropertyField(property);
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                EditorGUILayout.EndHorizontal();

                //FIRST LINE//
                EditorGUILayout.BeginHorizontal();
                //FIELD TO CHANGE PROPERTY
                currentSpriteIndex = (int)EditorGUILayout.Slider("Sprite Index :",currentSpriteIndex, 0, animator.Sprites.Length - 1);
                //DELETE CUSTOM TIME
                if (GUILayout.Button("DELETE"))
                {

                    animator.CustomTimes.RemoveAt(customTimeIndex);

                    customTimeIndex = 0;

                    if (animator.CustomTimes.Count == 0)
                    {
                        customTimeIndex = -1;
                        return;
                    }
                    else
                    {
                        currentSpriteIndex = animator.CustomTimes[customTimeIndex].spriteIndex;
                        currentTimeGapPerCent = animator.CustomTimes[customTimeIndex].timeGapPerCent;
                        CalculatePorcent(animator);
                    }


                }
                EditorGUILayout.EndHorizontal();


                //SECOND LINE//
                EditorGUILayout.BeginHorizontal();
                //FIELD TO CHANGE PROPERTY
                currentTimeGapPerCent = EditorGUILayout.Slider("Time (PerCent):", currentTimeGapPerCent, 0, totalTimeGap);
                //LABEL OUT OF 
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                GUILayout.Label($"{customTimeIndex + 1} / {animator.CustomTimes.Count}", style);
                EditorGUILayout.EndHorizontal();



                //SAVE DATA INTO THE CUSTOMTIME OBJECT
                animator.CustomTimes[customTimeIndex].spriteIndex = currentSpriteIndex;
                animator.CustomTimes[customTimeIndex].timeGapPerCent = currentTimeGapPerCent;








                EditorGUILayout.BeginHorizontal();

                //NAVIGATION

                float btWidth = Screen.width/3;

                //PREVIOUS ITEM
                if (customTimeIndex != 0)
                {
                    if (GUILayout.Button("<---", GUILayout.Width(btWidth), GUILayout.ExpandWidth(true)))
                    {
                        customTimeIndex--;
                        currentSpriteIndex = animator.CustomTimes[customTimeIndex].spriteIndex;
                        currentTimeGapPerCent = animator.CustomTimes[customTimeIndex].timeGapPerCent;

                        CalculatePorcent(animator);
                    }
                }
                else GUILayout.Space(btWidth);

                //NEW ITEM
                if (GUILayout.Button("+++", GUILayout.Width(btWidth), GUILayout.ExpandWidth(false)))
                {
                    CustomTime customTime = new CustomTime(animator.Duration, animator.Sprites.Length, 0);
                    animator.CustomTimes.Add(customTime);
                    currentSprite = animator.Sprites[customTime.spriteIndex];
                    currentTimeGapPerCent = customTime.timeGapPerCent;
                    currentSpriteIndex = customTime.spriteIndex;
                    customTimeIndex++;

                    CalculatePorcent(animator);
                    if (animator.CustomTimes[customTimeIndex].timeGapPerCent > totalTimeGap)
                    {
                        currentTimeGapPerCent = totalTimeGap;
                    }
                }

                //NEXT ITEM
                if (customTimeIndex != animator.CustomTimes.Count - 1)
                {
                    if (GUILayout.Button("--->", GUILayout.Width(btWidth), GUILayout.ExpandWidth(true)))
                    {
                        customTimeIndex++;
                        currentSpriteIndex = animator.CustomTimes[customTimeIndex].spriteIndex;
                        currentTimeGapPerCent = animator.CustomTimes[customTimeIndex].timeGapPerCent;

                        CalculatePorcent(animator);

                    }

                }
                else GUILayout.Space(btWidth);

                EditorGUILayout.EndHorizontal();




            }





        
    }
    private void CalculatePorcent(SpriteAnimator animator)
    {
        //CALCULATE PORCENT
        totalTimeGap = 99;
        for (int i = 0; i < animator.CustomTimes.Count; i++)
        {
            if (i != customTimeIndex)
            {
                totalTimeGap -= animator.CustomTimes[i].timeGapPerCent;
            }
        }

     
    }
}
#endif
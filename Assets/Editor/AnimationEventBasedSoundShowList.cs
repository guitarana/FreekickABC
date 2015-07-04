using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AnimationEventBasedSound))]

public class AnimationEventBasedSoundShowList : Editor {
	
	enum DisplayFieldType {DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields}
	DisplayFieldType displayFieldType;
	
	AnimationEventBasedSound t;
	SerializedObject getTarget;
	SerializedProperty thisList;
	int listSize;
	bool[] showPosition;

	void OnEnable(){
		t = (AnimationEventBasedSound)target;
		getTarget = new SerializedObject(t);
		thisList = getTarget.FindProperty("animEvent"); 
		showPosition = new bool[t.animEvent.Count+1];
	}
	
	public override void OnInspectorGUI(){

		getTarget.Update();
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		if(GUILayout.Button("Add New Animation Event Based Sound")){
			t.animEvent.Add(new AnimationEventBasedSound.AnimEvent());
			showPosition = new bool[t.animEvent.Count+1];
		}
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		
		//Display our list to the inspector window
		
		for(int i = 0; i < thisList.arraySize; i++){
			SerializedProperty myListRef = thisList.GetArrayElementAtIndex(i);
			SerializedProperty myAnimationName = myListRef.FindPropertyRelative("animationName");
			SerializedProperty mySfx = myListRef.FindPropertyRelative("sfx");
			SerializedProperty myFrames = myListRef.FindPropertyRelative("frames");

			showPosition[i] = EditorGUILayout.Foldout(showPosition[i],"Animation Event " + i.ToString());
			if(showPosition[i]){
				myAnimationName.stringValue = EditorGUILayout.TextField("Animation Name",myAnimationName.stringValue);
				mySfx.objectReferenceValue = EditorGUILayout.ObjectField("SFX",mySfx.objectReferenceValue, typeof(AudioClip), true);
					
				
				// Array fields with remove at index
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				if(GUILayout.Button("Add KeyFrame",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
					myFrames.InsertArrayElementAtIndex(myFrames.arraySize);
					myFrames.GetArrayElementAtIndex(myFrames.arraySize -1).floatValue = 0;
				}
				
				for(int a = 0; a < myFrames.arraySize; a++){
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("keyframe (" + a.ToString() + ")",GUILayout.MaxWidth(120));
					myFrames.GetArrayElementAtIndex(a).floatValue = EditorGUILayout.FloatField("",myFrames.GetArrayElementAtIndex(a).floatValue, GUILayout.MaxWidth(100));
					if(GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
						myFrames.DeleteArrayElementAtIndex(a);
					}
					EditorGUILayout.EndHorizontal();
				}
						
				EditorGUILayout.Space ();
				
				//Remove this index from the List
				if(GUILayout.Button("Remove Animation Event (" + i.ToString() + ")")){
					thisList.DeleteArrayElementAtIndex(i);
					showPosition = new bool[t.animEvent.Count+1];
				}
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
			}
			EditorGUILayout.Space ();


		}
		
		//Apply the changes to our list
		getTarget.ApplyModifiedProperties();
	}
}
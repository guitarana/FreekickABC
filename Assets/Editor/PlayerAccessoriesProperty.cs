using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PlayerAccessoriesList))]

public class PlayerAccessoriesProperty : Editor {
	
	enum DisplayFieldType {DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields}
	DisplayFieldType displayFieldType;
	
	PlayerAccessoriesList t;
	SerializedObject getTarget;
	SerializedProperty thisList;
	int listSize;
	bool[] showPosition;

	string labelName = "Player Accessories";
	
	void OnEnable(){
		t = (PlayerAccessoriesList)target;
		getTarget = new SerializedObject(t);
		thisList = getTarget.FindProperty("accessories"); 
		showPosition = new bool[t.accessories.Count+1];
	}
	
	public override void OnInspectorGUI(){
		
		getTarget.Update();
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		
		if(GUILayout.Button("Add New Accesories")){
			t.accessories.Add(new PlayerAccessoriesList.Accessories());
			showPosition = new bool[t.accessories.Count+1];
		}
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		
		//Display our list to the inspector window
		
		for(int i = 0; i < thisList.arraySize; i++){
			SerializedProperty myListRef = thisList.GetArrayElementAtIndex(i);
			SerializedProperty myObjectName = myListRef.FindPropertyRelative("name");
			SerializedProperty myGO = myListRef.FindPropertyRelative("go");
			SerializedProperty myBool = myListRef.FindPropertyRelative("available");
			SerializedProperty myType = myListRef.FindPropertyRelative("type");
			SerializedProperty myIndex = myListRef.FindPropertyRelative("index");

			if(myObjectName.stringValue !="") labelName = myObjectName.stringValue;
			showPosition[i] = EditorGUILayout.Foldout(showPosition[i],labelName);

			if(showPosition[i]){
				myObjectName.stringValue = EditorGUILayout.TextField("Name",myObjectName.stringValue);
				myGO.objectReferenceValue = EditorGUILayout.ObjectField("GO",myGO.objectReferenceValue, typeof(GameObject), true);
				myBool.boolValue = EditorGUILayout.Toggle("Is Available",myBool.boolValue);
				EditorGUILayout.PropertyField( myType );
				myIndex.intValue = EditorGUILayout.IntField("Index",myIndex.intValue);

				// Array fields with remove at index
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				
//				if(GUILayout.Button("Add KeyFrame",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
//					myBool.InsertArrayElementAtIndex(myBool.arraySize);
//					myBool.GetArrayElementAtIndex(myBool.arraySize -1).floatValue = 0;
//				}
//				
//				for(int a = 0; a < myBool.arraySize; a++){
//					EditorGUILayout.BeginHorizontal();
//					EditorGUILayout.LabelField("keyframe (" + a.ToString() + ")",GUILayout.MaxWidth(120));
//					myBool.GetArrayElementAtIndex(a).floatValue = EditorGUILayout.FloatField("",myBool.GetArrayElementAtIndex(a).floatValue, GUILayout.MaxWidth(100));
//					if(GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
//						myBool.DeleteArrayElementAtIndex(a);
//					}
//					EditorGUILayout.EndHorizontal();
//				}
				
				EditorGUILayout.Space ();
				
				//Remove this index from the List
				if(GUILayout.Button("Remove GO(" + i.ToString() + ")")){
					thisList.DeleteArrayElementAtIndex(i);
					showPosition = new bool[t.accessories.Count+1];
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
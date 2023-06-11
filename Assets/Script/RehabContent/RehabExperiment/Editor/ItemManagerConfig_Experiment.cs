using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ItemManager_Experiment))]
public class ItemManagerConfig_Experiment : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		ItemManager_Experiment manager = (ItemManager_Experiment)target;
		if (GUILayout.Button("아이템 생성"))
		{
			manager.CreateDynamicItem();
		}
	}
}
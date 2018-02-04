using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Provides a convenient way to attach a quest objective to a script from within Unity's inspector
[Serializable]
public class QuestTrigger {

    public Quest quest;
    public int objectiveIndex;

	public void Trigger()
    {
        quest.objectives[objectiveIndex].Trigger();
    }

}

[CustomPropertyDrawer(typeof(QuestTrigger))]
public class QuestTriggerPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position.height = EditorGUIUtility.singleLineHeight;

        // Label
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

        // Quest field
        EditorGUI.indentLevel++;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("quest"), new GUIContent("Quest"));
        
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

        // Extract relevant properties from the serialized trigger object
        Quest quest = (Quest)property.FindPropertyRelative("quest").objectReferenceValue;
        int selected = property.FindPropertyRelative("objectiveIndex").intValue;

        if (quest)
        {
            // Objective field label
            Rect objectivePosition = EditorGUI.PrefixLabel(position, new GUIContent("Objective"));
            EditorGUI.indentLevel--;

            // Enumerate objectives
            int numObjectives = quest.objectives.Length;

            string[] options = new string[numObjectives];
            int[] values = new int[numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                options[i] = quest.objectives[i].name;
                values[i] = i;
            }

            // Objective field selector
            int selection = EditorGUI.IntPopup(objectivePosition, selected, options, values);

            property.FindPropertyRelative("objectiveIndex").intValue = selection;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.FindPropertyRelative("quest").objectReferenceValue != null)
        {
            return 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
        }
        else
        {
            return 2 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }

}
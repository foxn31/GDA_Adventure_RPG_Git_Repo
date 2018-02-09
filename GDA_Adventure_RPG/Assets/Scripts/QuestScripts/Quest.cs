using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest")]
public class Quest : ScriptableObject {

    [Header("Quest Info")]
    public string questName;
    [TextArea]
    public string flavorText;

    [Header("Prerequisites")]
    [Tooltip("Quests that must be completed before this one is available")]
    public List<Quest> prerequisites;
    [Tooltip("Auto start this quest once its prerequisites are completed")]
    public bool autoStart = true;

    [Header("Objective")]
    public string objective;
    public int requiredCount = 1;

}
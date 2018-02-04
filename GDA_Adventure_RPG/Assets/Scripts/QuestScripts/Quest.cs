using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest")]
public class Quest : ScriptableObject {

    [Serializable]
    public class QuestObjective
    {
        public string name;
        public int countRequired = 1;

        private int currentCount = 0;

        public bool IsComplete
        {
            get
            {
                return currentCount >= countRequired;
            }
        }

        public event Action<QuestObjective> OnComplete;
        public void Trigger()
        {
            currentCount += 1;
            if (currentCount == countRequired)
            {
                if (OnComplete != null) OnComplete(this);
            }
        }
    }

    [Header("Quest Info")]
    public string questName;
    [TextArea]
    public string questDescription;
    
    [Header("Dependencies")]
    [Tooltip("Quests that must be completed before this one is available")]
    public Quest[] dependencies;
    [Tooltip("Auto start this quest once its depenencies are completed")]
    public bool autoStart = true;
    
    [Header("Objectives")]
    public QuestObjective[] objectives;

    public bool IsComplete
    {
        get
        {
            return complete;
        }
    }

    private bool complete = false;
    public event Action<Quest> OnComplete;

    public void Awake()
    {
        foreach (QuestObjective o in objectives)
        {
            o.OnComplete += ObjectiveCompleteHandler;
        }
        foreach (Quest q in dependencies)
        {
            q.OnComplete += DependencyCompleteHandler;
        }
    }

    private void ObjectiveCompleteHandler(QuestObjective objective)
    {
        foreach (QuestObjective q in objectives)
        {
            if (!q.IsComplete) return;
        }
        // All objectives are complete
        complete = true;
        if (OnComplete != null) OnComplete(this);
    }

    private void DependencyCompleteHandler(Quest quest)
    {
        foreach (Quest q in dependencies)
        {
            if (!q.IsComplete) return;
        }
        // All dependencies are complete

    }

}

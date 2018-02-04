using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest")]
public class Quest : ScriptableObject {

    [CreateAssetMenu(fileName = "New Simple Objective", menuName = "Quest/Simple Objective")]
    public class QuestObjective : ScriptableObject {
        public string objectiveName;

        public event Action OnTrigger;
        public virtual void Trigger()
        {
            if (OnTrigger != null) OnTrigger();
        }
    }

    [CreateAssetMenu(fileName = "New Count Objective", menuName = "Quest/Count Objective")]
    public class CountQuestObjective : QuestObjective
    {
        public int goalCount = 0;
        private int count = 0;

        public override void Trigger()
        {
            count++;
            if (count == goalCount)
            {
                base.Trigger();
            }
        }
    }

    public string questName;
    [TextArea]
    public string questDescription;
    public List<QuestObjective> objectives;

}

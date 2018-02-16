using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public QuestUI questUI;

    public static HashSet<Quest> activeQuests { get; private set; }
    private static Dictionary<Quest, int> progress;
    private static Dictionary<Quest, HashSet<Quest>> questUnlocks;

    public static event System.Action OnQuestListChanged;
    private static void QuestListChanged()
    {
        if (OnQuestListChanged != null) OnQuestListChanged();
    }

    public static bool IsComplete(Quest q)
    {
        return GetProgress(q) >= q.requiredCount;
    }

    public static bool IsAvailable(Quest q)
    {
        foreach (Quest q2 in q.prerequisites)
        {
            if (!IsComplete(q2)) return false;
        }
        return true;
    }

    public static int GetProgress(Quest q)
    {
        int prog;
        if (progress.TryGetValue(q, out prog))
        {
            return prog;
        }
        return 0;
    }

    public static void StartQuest(Quest q)
    {
        // Quests that are already complete can't be started
        if (IsAvailable(q) && !IsComplete(q))
        {
            activeQuests.Add(q);
            QuestListChanged();
        }
    }

    public static void AbandonQuest(Quest q)
    {
        activeQuests.Remove(q);
        QuestListChanged();
    }

    public static bool IsQuestActive(Quest q)
    {
        return activeQuests.Contains(q);
    }

    public static void TriggerQuestProgress(Quest q)
    {
        if (activeQuests.Contains(q))
        {
            if (progress.ContainsKey(q))
            {
                progress[q]++;
            }
            else
            {
                progress[q] = 1;
            }

            if (IsComplete(q))
            {
                activeQuests.Remove(q);

                Debug.Log(q.questName + ": " + questUnlocks[q].Count);

                // Quest just completed; there may be some quests that should be auto-started
                foreach (Quest nextQ in questUnlocks[q])
                {
                    // Attempt to start any quest labeled as auto start
                    // StartQuest() will make sure it actually can be started
                    if (nextQ.autoStart) StartQuest(nextQ);
                }

                QuestListChanged();
            }
        }
    }

    public static QuestManager instance;

    //public Quest[] startAutomatically;

    public void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        activeQuests = new HashSet<Quest>();
        progress = new Dictionary<Quest, int>();
        questUnlocks = new Dictionary<Quest, HashSet<Quest>>();

        Quest[] allQuests = Resources.FindObjectsOfTypeAll<Quest>();
        
        foreach (Quest q in allQuests)
        {
            questUnlocks[q] = new HashSet<Quest>();
        }
        foreach (Quest q in allQuests)
        {
            foreach (Quest q2 in q.prerequisites)
            {
                questUnlocks[q2].Add(q);
            }

            if (q.prerequisites.Count == 0 && q.autoStart)
            {
                StartQuest(q);
            }
        }

        QuestListChanged();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameUI.instance.ToggleUIElement(questUI.gameObject);
        }
    }

    public void OnGUI()
    {
        Rect pos = new Rect(10, 10, 200, 24);

        foreach (Quest q in activeQuests)
        {
            GUI.Box(pos, q.questName);
            pos.x += pos.height;
        }
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour {

    public RectTransform background;
    public RectTransform viewport;
    public RectTransform contents;
    public Scrollbar scrollbar;

    public GameObject questElementPrefab;

	// Use this for initialization
	void Start () {
        scrollbar.onValueChanged.AddListener(OnScroll);
        QuestManager.OnQuestListChanged += OnQuestListChanged;

        OnQuestListChanged();
	}

    public void OnScroll(float y)
    {
        float scrollRange = Mathf.Max(0, contents.rect.height - viewport.rect.height);
        contents.offsetMax = new Vector2(0, y * scrollRange);
    }

    public void OnQuestListChanged()
    {
        List<Quest> quests = QuestManager.activeQuests.ToList();
        quests.Sort(delegate (Quest a, Quest b)
        {
            return a.questName.CompareTo(b.questName);
        });

        VerticalLayoutGroup group = contents.GetComponent<VerticalLayoutGroup>();
        
        foreach (Transform child in contents)
        {
            Destroy(child.gameObject);
        }
        foreach (Quest q in quests)
        {
            GameObject obj = Instantiate(questElementPrefab);
            QuestListElement qle = obj.GetComponent<QuestListElement>();
            qle.Quest = q;
            obj.transform.SetParent(contents, false);
        }

        scrollbar.size = Mathf.Clamp(viewport.rect.height / contents.rect.height, 0, 1);
    }
}

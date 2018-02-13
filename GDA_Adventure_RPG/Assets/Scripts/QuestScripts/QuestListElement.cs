using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListElement : MonoBehaviour {

    public Text title;
    public Text objective;
    public Text flavor;

    public Quest Quest {
        set
        {
            title.text = value.questName;
            objective.text = value.objective + ": " + QuestManager.GetProgress(value) + "/" + value.requiredCount;
            flavor.text = value.flavorText;
        }
    }

}

using UnityEngine;

public class DialogueNPC : Interactable
{

    public static event System.Action ShowTalkPrompt;
    public static event System.Action HideTalkPrompt;

    bool promptVisible = true;

    public Dialogue dialogue;

    public Quest quest;
    public int questTriggerSentence;

    bool active = false;
    float timeAtLastDiologue;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        DialogueManager.EndAllDialogue += EndThisDialogue;
    }

    public override void Interact()
    {

        base.Interact();

        if (!active)
        {
            StartDialogue();
            timeAtLastDiologue = Time.time;
            HidePrompt();
        }
        else if (Time.time >= (timeAtLastDiologue + .1))
        {
            ContinueDialogue();
            timeAtLastDiologue = Time.time;
        }

    }

    public override void Update()
    {
        base.Update();
    }

    void LateUpdate()
    {
        if (playerIsInRange() && !active)
        {
            ShowPrompt();
        }
        else
        {
            HidePrompt();
        }
    }

    public void CheckQuestTrigger()
    {
        if (quest != null && DialogueManager.instance.SentenceIndex == questTriggerSentence)
        {
            QuestManager.TriggerQuestProgress(quest);
        }
    }

    public void StartDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        CheckQuestTrigger();
        active = true;
    }

    public void ContinueDialogue()
    {
        DialogueManager.instance.ContinueDialogue();
        CheckQuestTrigger();
    }

    void ShowPrompt()
    {
        if (!promptVisible && ShowTalkPrompt != null)
        {
            ShowTalkPrompt();
            promptVisible = true;
        }
    }

    void HidePrompt()
    {
        if (promptVisible && HideTalkPrompt != null)
        {
            HideTalkPrompt();
            promptVisible = false;
        }
    }

    void EndThisDialogue()
    {
        isInteracting = false;
        active = false;
    }
}

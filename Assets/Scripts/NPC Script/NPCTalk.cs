using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public DialogueSO dialogueSO;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DialogueManager.Instance.isDialogueActive)
                DialogueManager.Instance.AdvanceDialogue();
            else
                DialogueManager.Instance.StartDialogue(dialogueSO);
        }
    }
}

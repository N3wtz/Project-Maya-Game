using System.Collections;
using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
    public DialogueSO startingDialogue;

    private void Start()
    {
        if (startingDialogue != null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(startingDialogue);
        }
        else
        {
            Debug.LogWarning("startingDialogue belum di-assign atau DialogueManager.Instance belum siap.");
        }
    }

    private void Update()
    {
        if (DialogueManager.Instance != null && Input.GetKeyDown(KeyCode.Space))
        {
            if (DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else if (startingDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(startingDialogue);
            }
        }
    }
}

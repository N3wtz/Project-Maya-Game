using System.Collections;
using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueSO startingDialogue;

    [Header("Delay Settings")]
    [Tooltip("Waktu jeda sebelum dialog muncul (dalam detik)")]
    public float startDelay = 1.7f;

    private void Start()
    {
        StartCoroutine(StartDialogueWithDelay());
    }

    private IEnumerator StartDialogueWithDelay()
    {
        // Tunggu selama waktu yang ditentukan
        yield return new WaitForSeconds(startDelay);

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

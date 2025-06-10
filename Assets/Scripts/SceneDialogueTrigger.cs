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
        yield return new WaitForSeconds(startDelay);

        // Tunggu sampai DialogueManager.Instance tersedia
        while (DialogueManager.Instance == null)
        {
            yield return null;
        }

        if (startingDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(startingDialogue);
        }
        else
        {
            Debug.LogWarning("startingDialogue belum di-assign.");
        }
    }
}

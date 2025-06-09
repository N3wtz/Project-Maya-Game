using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    // public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;

    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private float typingSpeed = 0.05f;

    private bool isInCooldown = false;
    public float cooldownAfterDialogue = 1f; // bisa diatur dari Unity Inspector

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        if (isDialogueActive || isInCooldown) return; // Cegah saat masih aktif atau dalam cooldown

        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        if (isTyping)
        {
            // Langsung tampilkan seluruh teks
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = currentDialogue.lines[dialogueIndex - 1].text;
            isTyping = false;
        }
        else if (dialogueIndex < currentDialogue.lines.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        // portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line.text));

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        dialogueIndex++;
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Mulai cooldown agar tidak bisa langsung dijalankan lagi
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownAfterDialogue);
        isInCooldown = false;
    }
}
